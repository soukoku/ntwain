using NTwain.Data;
using NTwain.Native;
using NTwain.Triplets;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace NTwain
{
  // this file contains various xfer methods

  partial class TwainAppSession
  {
    /// <summary>
    /// Can only be called in state 7, so it's hidden here and 
    /// only exposed in data transferred event.
    /// </summary>
    /// <param name="container"></param>
    /// <returns></returns>
    internal STS GetExtendedImageInfo(ref TW_EXTIMAGEINFO container)
    {
      return WrapInSTS(DGImage.ExtImageInfo.Get(ref _appIdentity, ref _currentDS, ref container));
    }


    /// <summary>
    /// Start the transfer loop.
    /// This should be called after receiving 
    /// <see cref="MSG.XFERREADY"/> in the background thread.
    /// </summary>
    void EnterTransferRoutine()
    {
      // TODO: currently implemented routine doesn't quite work for audio as described in spec


      // default options if source doesn't support changing them or whatever
      bool xferImage = true;
      bool xferAudio = false;
      if (DGControl.XferGroup.Get(ref _appIdentity, ref _currentDS, out DG xferType) == TWRC.SUCCESS)
      {
        xferAudio = (xferType & DG.AUDIO) == DG.AUDIO;
        var dsName = _currentDS.ProductName.ToString();
        // check for Plustek OpticSlim 2680H, this scanner returns wrong xferGroup after first scanning
        if (dsName.IndexOf("Plustek", StringComparison.OrdinalIgnoreCase) > -1 &&
            dsName.IndexOf("OpticSlim", StringComparison.OrdinalIgnoreCase) > -1 &&
            dsName.IndexOf("2680H", StringComparison.OrdinalIgnoreCase) > -1)
        {
          xferImage = true;
        }
        else
        {
          // some DS end up getting none but we will assume it's image
          xferImage = xferType == 0 || (xferType & DG.IMAGE) == DG.IMAGE;
        }
      }

      var imgXferMech = TWSX.NATIVE;
      var audXferMech = TWSX.NATIVE;
      if (xferImage) imgXferMech = Caps.ICAP_XFERMECH.GetCurrent().FirstOrDefault();
      else if (xferAudio) audXferMech = Caps.ACAP_XFERMECH.GetCurrent().FirstOrDefault();

      TW_PENDINGXFERS pending = default;
      var sts = WrapInSTS(DGControl.PendingXfers.Get(ref _appIdentity, ref _currentDS, ref pending));
      if (sts.RC == TWRC.SUCCESS)
      {
        do
        {
          var readyArgs = new TransferReadyEventArgs(pending.Count, (TWEJ)pending.EOJ);
          try
          {
            TransferReady?.Invoke(this, readyArgs);
          }
          catch { } // don't let consumer kill the loop if they have exception

          if (readyArgs.Cancel == CancelType.EndNow || _closeDsRequested)
          {
            // TODO: need to call EndXfer first?
            sts = WrapInSTS(DGControl.PendingXfers.Reset(ref _appIdentity, ref _currentDS, ref pending));
            if (sts.RC == TWRC.SUCCESS && xferImage) State = STATE.S5;
          }
          else if (readyArgs.Cancel == CancelType.SkipCurrent)
          {
            sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
            if (sts.RC == TWRC.SUCCESS)
            {
              if (xferImage)
              {
                State = pending.Count == 0 ? STATE.S5 : STATE.S6;
              }
              else if (xferAudio)
              {
                State = STATE.S6;
              }
            }
          }
          else
          {
            if (readyArgs.Cancel == CancelType.Graceful)
            {
              // ignore rc in this and continue transfer as normal
              DGControl.PendingXfers.StopFeeder(ref _appIdentity, ref _currentDS, ref pending);
            }

            try
            {
              if (readyArgs.Cancel != CancelType.SkipCurrent)
              {
                // transfer normally and only if someone's listening
                // to DataTransferred event
                if (xferImage)
                {
                  switch (imgXferMech)
                  {
                    case TWSX.NATIVE:
                      sts = TransferNativeImage(ref pending);
                      break;
                    case TWSX.FILE:
                      sts = TransferFileImage(ref pending);
                      break;
                    case TWSX.MEMORY:
                      sts = TransferMemoryImage(ref pending);
                      break;
                    case TWSX.MEMFILE:
                      sts = TransferMemoryFileImage(ref pending);
                      break;
                  }
                }
                else if (xferAudio)
                {
                  switch (audXferMech)
                  {
                    case TWSX.NATIVE:
                      sts = TransferNativeAudio(ref pending);
                      break;
                    case TWSX.FILE:
                      sts = TransferFileAudio(ref pending);
                      break;
                  }
                }
                HandleXferCode(ref sts, ref pending);
              }
            }
            catch (Exception ex)
            {
              try
              {
                TransferError?.Invoke(this, new TransferErrorEventArgs(ex));
              }
              catch { }
            }
          }
        } while (sts.RC == TWRC.SUCCESS && pending.Count != 0);
      }

      HandleXferCode(ref sts, ref pending);

      if (State >= STATE.S5)
      {
        DisableSource();
      }
      _inTransfer = false;
    }

    private void HandleXferCode(ref STS sts, ref TW_PENDINGXFERS pending)
    {
      switch (sts.RC)
      {
        case TWRC.SUCCESS:
        case TWRC.XFERDONE:
          // ok to keep going
          break;
        case TWRC.CANCEL:
          // might eventually have option to cancel this or all like transfer ready
          try
          {
            TransferCanceled?.Invoke(this, new TransferCanceledEventArgs());
          }
          catch { }
          sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
          sts = WrapInSTS(DGControl.PendingXfers.Reset(ref _appIdentity, ref _currentDS, ref pending));
          if (sts.RC == TWRC.SUCCESS) State = STATE.S5;
          break;
        default:
          // TODO: raise error event
          switch (sts.STATUS.ConditionCode)
          {
            case TWCC.DAMAGEDCORNER:
            case TWCC.DOCTOODARK:
            case TWCC.DOCTOOLIGHT:
            case TWCC.FOCUSERROR:
            case TWCC.NOMEDIA:
            case TWCC.PAPERDOUBLEFEED:
            case TWCC.PAPERJAM:
              pending = default;
              sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
              break;
            case TWCC.OPERATIONERROR:
              var indicators = Caps.CAP_INDICATORS.GetCurrent().FirstOrDefault();
              if (_userInterface.ShowUI == 0 && indicators == TW_BOOL.False)
              {
                // todo: alert user and drop to S4
                sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
                sts = WrapInSTS(DGControl.PendingXfers.Reset(ref _appIdentity, ref _currentDS, ref pending));
              }
              break;
          }
          break;
      }
    }

    private STS TransferFileAudio(ref TW_PENDINGXFERS pending)
    {
      // assuming user already configured the transfer in transferready event,
      // get what will be transferred
      var rc = DGControl.SetupFileXfer.Get(ref _appIdentity, ref _currentDS, out TW_SETUPFILEXFER fileSetup);
      if (rc != TWRC.SUCCESS) return WrapInSTS(rc);

      // and just start it
      var sts = WrapInSTS(DGAudio.AudioFileXfer.Get(ref _appIdentity, ref _currentDS));
      if (sts.RC == TWRC.XFERDONE)
      {
        State = STATE.S7;
        try
        {
          DGAudio.AudioInfo.Get(ref _appIdentity, ref _currentDS, out TW_AUDIOINFO info);
          var args = new TransferredEventArgs(info, fileSetup);
          Transferred?.Invoke(this, args);
        }
        catch { }
;
        sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
        if (sts.RC == TWRC.SUCCESS)
        {
          State = STATE.S6;
        }
      }
      return sts;
    }

    private STS TransferNativeAudio(ref TW_PENDINGXFERS pending)
    {
      IntPtr dataPtr = IntPtr.Zero;
      IntPtr lockedPtr = IntPtr.Zero;
      try
      {
        var sts = WrapInSTS(DGAudio.AudioNativeXfer.Get(ref _appIdentity, ref _currentDS, out dataPtr));
        if (sts.RC == TWRC.XFERDONE)
        {
          State = STATE.S7;
          lockedPtr = Lock(dataPtr);
          BufferedData? data = default;

          // TODO: don't know how to read wav/aiff from pointer yet

          if (data != null)
          {
            try
            {
              DGAudio.AudioInfo.Get(ref _appIdentity, ref _currentDS, out TW_AUDIOINFO info);
              var args = new TransferredEventArgs(info, data);
              Transferred?.Invoke(this, args);
            }
            catch
            {
              data.Dispose();
            }
          }
        }

        sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
        if (sts.RC == TWRC.SUCCESS)
        {
          State = STATE.S6;
        }
        return sts;
      }
      finally
      {
        if (lockedPtr != IntPtr.Zero) Unlock(dataPtr);
        if (dataPtr != IntPtr.Zero) Free(dataPtr);
      }
    }

    private STS TransferMemoryImage(ref TW_PENDINGXFERS pending)
    {
      var rc = DGControl.SetupMemXfer.Get(ref _appIdentity, ref _currentDS, out TW_SETUPMEMXFER memSetup);
      if (rc != TWRC.SUCCESS) return WrapInSTS(rc);
      rc = DGImage.ImageInfo.Get(ref _appIdentity, ref _currentDS, out TW_IMAGEINFO info);
      if (rc != TWRC.SUCCESS) return WrapInSTS(rc);
      rc = DGImage.ImageLayout.Get(ref _appIdentity, ref _currentDS, out TW_IMAGELAYOUT layout);
      if (rc != TWRC.SUCCESS) return WrapInSTS(rc);

      uint buffSize = memSetup.DetermineBufferSize();
      var memPtr = Alloc(buffSize);

      TW_IMAGEMEMXFER memXfer = TW_IMAGEMEMXFER.DONTCARE();
      TW_IMAGEMEMXFER_MACOSX memXferOSX = TW_IMAGEMEMXFER_MACOSX.DONTCARE();
      memXfer.Memory = new TW_MEMORY
      {
        Flags = (uint)(TWMF.APPOWNS | TWMF.POINTER),
        Length = buffSize,
        TheMem = memPtr
      };
      memXferOSX.Memory = memXfer.Memory;

      byte[] dotnetBuff = BufferedData.MemPool.Rent((int)buffSize);
      try
      {
        do
        {
          rc = TWPlatform.IsMacOSX ?
            DGImage.ImageMemXfer.Get(ref _appIdentity, ref _currentDS, ref memXferOSX) :
            DGImage.ImageMemXfer.Get(ref _appIdentity, ref _currentDS, ref memXfer);

          if (rc == TWRC.SUCCESS || rc == TWRC.XFERDONE)
          {
            try
            {
              var written = TWPlatform.IsMacOSX ?
                memXferOSX.BytesWritten : memXfer.BytesWritten;

              IntPtr lockedPtr = Lock(memPtr);

              // assemble!

              //Marshal.Copy(lockedPtr, dotnetBuff, 0, (int)written);
              //outStream.Write(dotnetBuff, 0, (int)written);
            }
            finally
            {
              Unlock(memPtr);
            }
          }
        } while (rc == TWRC.SUCCESS);
      }
      finally
      {
        if (memPtr != IntPtr.Zero) Free(memPtr);
        BufferedData.MemPool.Return(dotnetBuff);
      }


      if (rc == TWRC.XFERDONE)
      {
        try
        {
          DGImage.ImageInfo.Get(ref _appIdentity, ref _currentDS, out info);
          //var args = new DataTransferredEventArgs(info, null, outStream.ToArray());
          //DataTransferred?.Invoke(this, args);
        }
        catch { }

        var sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
        if (sts.RC == TWRC.SUCCESS)
        {
          State = pending.Count == 0 ? STATE.S5 : STATE.S6;
        }
        return sts;
      }
      return WrapInSTS(rc);
    }

    private STS TransferMemoryFileImage(ref TW_PENDINGXFERS pending)
    {
      var rc = DGControl.SetupFileXfer.Get(ref _appIdentity, ref _currentDS, out TW_SETUPFILEXFER fileSetup);
      if (rc != TWRC.SUCCESS) return WrapInSTS(rc);
      rc = DGControl.SetupMemXfer.Get(ref _appIdentity, ref _currentDS, out TW_SETUPMEMXFER memSetup);
      if (rc != TWRC.SUCCESS) return WrapInSTS(rc);

      uint buffSize = memSetup.DetermineBufferSize();
      var memPtr = Alloc(buffSize);

      TW_IMAGEMEMXFER memXfer = TW_IMAGEMEMXFER.DONTCARE();
      TW_IMAGEMEMXFER_MACOSX memXferOSX = TW_IMAGEMEMXFER_MACOSX.DONTCARE();
      memXfer.Memory = new TW_MEMORY
      {
        Flags = (uint)(TWMF.APPOWNS | TWMF.POINTER),
        Length = buffSize,
        TheMem = memPtr
      };
      memXferOSX.Memory = memXfer.Memory;

      // TODO: how to get actual file size before hand? Is it imagelayout?
      // otherwise will just write to stream with lots of copies
      byte[] dotnetBuff = BufferedData.MemPool.Rent((int)buffSize);
      using var outStream = new MemoryStream();
      try
      {
        do
        {
          rc = TWPlatform.IsMacOSX ?
            DGImage.ImageMemFileXfer.Get(ref _appIdentity, ref _currentDS, ref memXferOSX) :
            DGImage.ImageMemFileXfer.Get(ref _appIdentity, ref _currentDS, ref memXfer);

          if (rc == TWRC.SUCCESS || rc == TWRC.XFERDONE)
          {
            try
            {
              var written = TWPlatform.IsMacOSX ?
                memXferOSX.BytesWritten : memXfer.BytesWritten;

              IntPtr lockedPtr = Lock(memPtr);
              Marshal.Copy(lockedPtr, dotnetBuff, 0, (int)written);
              outStream.Write(dotnetBuff, 0, (int)written);
            }
            finally
            {
              Unlock(memPtr);
            }
          }
        } while (rc == TWRC.SUCCESS);
      }
      finally
      {
        if (memPtr != IntPtr.Zero) Free(memPtr);
        BufferedData.MemPool.Return(dotnetBuff);
      }

      if (rc == TWRC.XFERDONE)
      {
        try
        {
          DGImage.ImageInfo.Get(ref _appIdentity, ref _currentDS, out TW_IMAGEINFO info);
          // ToArray bypasses the XferMemPool but I guess this will have to do for now
          var args = new TransferredEventArgs(this, info, fileSetup, new BufferedData(outStream.ToArray(), (int)outStream.Length, false));
          Transferred?.Invoke(this, args);
        }
        catch { }

        var sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
        if (sts.RC == TWRC.SUCCESS)
        {
          State = pending.Count == 0 ? STATE.S5 : STATE.S6;
        }
        return sts;
      }
      return WrapInSTS(rc);
    }

    private STS TransferFileImage(ref TW_PENDINGXFERS pending)
    {
      // assuming user already configured the transfer in transferready event,
      // get what will be transferred
      DGControl.SetupFileXfer.Get(ref _appIdentity, ref _currentDS, out TW_SETUPFILEXFER fileSetup);
      // and just start it
      var sts = WrapInSTS(DGImage.ImageFileXfer.Get(ref _appIdentity, ref _currentDS));
      if (sts.RC == TWRC.XFERDONE)
      {
        State = STATE.S7;
        try
        {
          DGImage.ImageInfo.Get(ref _appIdentity, ref _currentDS, out TW_IMAGEINFO info);
          var args = new TransferredEventArgs(this, info, fileSetup, default);
          Transferred?.Invoke(this, args);
        }
        catch { }

        sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
        if (sts.RC == TWRC.SUCCESS)
        {
          State = pending.Count == 0 ? STATE.S5 : STATE.S6;
        }
      }
      return sts;
    }

    private STS TransferNativeImage(ref TW_PENDINGXFERS pending)
    {
      IntPtr dataPtr = IntPtr.Zero;
      IntPtr lockedPtr = IntPtr.Zero;
      try
      {
        DGImage.ImageInfo.Get(ref _appIdentity, ref _currentDS, out TW_IMAGEINFO info);
        var sts = WrapInSTS(DGImage.ImageNativeXfer.Get(ref _appIdentity, ref _currentDS, out dataPtr));
        if (sts.RC == TWRC.XFERDONE)
        {
          State = STATE.S7;
          lockedPtr = Lock(dataPtr);
          BufferedData? data = default;

          if (ImageTools.IsDib(lockedPtr))
          {
            data = ImageTools.GetBitmapData(lockedPtr);
          }
          else if (ImageTools.IsTiff(lockedPtr))
          {
            data = ImageTools.GetTiffData(lockedPtr);
          }
          else
          {
            // PicHandle?
            // don't support more formats :(
          }

          if (data != null)
          {
            try
            {
              // some sources do not support getting image info in state 7 so
              // it's up there in the beginning now.
              var args = new TransferredEventArgs(this, info, null, data);
              Transferred?.Invoke(this, args);
            }
            catch
            {
              data.Dispose();
            }
          }


          sts = WrapInSTS(DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending));
          if (sts.RC == TWRC.SUCCESS)
          {
            State = pending.Count == 0 ? STATE.S5 : STATE.S6;
          }
        }
        return sts;
      }
      finally
      {
        if (lockedPtr != IntPtr.Zero) Unlock(dataPtr);
        if (dataPtr != IntPtr.Zero) Free(dataPtr);
      }
    }

  }
}
