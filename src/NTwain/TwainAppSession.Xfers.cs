using NTwain.Data;
using NTwain.Native;
using NTwain.Triplets;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NTwain
{
  // this file contains various xfer methods

  partial class TwainAppSession
  {
    // experiment using array pool for things transferred in memory.
    // this can pool up to a "normal" max of legal size paper in 24 bit at 300 dpi (~31MB)
    // so the array max is made with 32 MB. Typical usage should be a lot less.
    static readonly ArrayPool<byte> XferMemPool = ArrayPool<byte>.Create(32 * 1024 * 1024, 8);

    internal STS GetImageInfo(out TW_IMAGEINFO info)
    {
      return WrapInSTS(DGImage.ImageInfo.Get(ref _appIdentity, ref _currentDS, out info));
    }


    /// <summary>
    /// Start the transfer loop.
    /// This should be called after receiving 
    /// <see cref="MSG.XFERREADY"/> in the background thread.
    /// </summary>
    void EnterTransferRoutine()
    {
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
      if (xferImage) GetCapCurrent(CAP.ICAP_XFERMECH, out imgXferMech);
      else if (xferAudio) GetCapCurrent(CAP.ACAP_XFERMECH, out audXferMech);

      TW_PENDINGXFERS pending = default;
      var sts = WrapInSTS(DGControl.PendingXfers.Get(ref _appIdentity, ref _currentDS, ref pending));
      if (sts.RC == TWRC.SUCCESS)
      {
        do
        {
          var readyArgs = new TransferReadyEventArgs(this, pending.Count, (TWEJ)pending.EOJ);
          _uiThreadMarshaller.Invoke(() =>
          {
            try
            {
              TransferReady?.Invoke(this, readyArgs);
            }
            catch { } // don't let consumer kill the loop if they have exception
          });

          if (readyArgs.Cancel == CancelType.EndNow || _closeDsRequested)
          {
            sts = WrapInSTS(DGControl.PendingXfers.Reset(ref _appIdentity, ref _currentDS, ref pending));
            if (sts.RC == TWRC.SUCCESS) State = STATE.S5;
            break;
          }
          if (readyArgs.Cancel == CancelType.Graceful)
          {
            // ignore rc in this
            DGControl.PendingXfers.StopFeeder(ref _appIdentity, ref _currentDS, ref pending);
          }

          if (readyArgs.Cancel != CancelType.SkipCurrent &&
            DataTransferred != null)
          {
            // transfer normally and only if someone's listening
            // to DataTransferred event
            if (xferImage)
            {
              switch (imgXferMech)
              {
                case TWSX.NATIVE:
                  sts = TransferNativeImage();
                  break;
                case TWSX.FILE:
                  sts = TransferFileImage();
                  break;
                case TWSX.MEMORY:
                  sts = TransferMemoryImage();
                  break;
                case TWSX.MEMFILE:
                  sts = TransferMemoryFileImage();
                  break;
              }
            }
            else if (xferAudio)
            {
              switch (audXferMech)
              {
                case TWSX.NATIVE:
                  sts = TransferNativeAudio();
                  break;
                case TWSX.FILE:
                  sts = TransferFileAudio();
                  break;
              }
            }
          }

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

        } while (sts.RC == TWRC.SUCCESS && pending.Count != 0);
      }
      else
      {
        HandleNonSuccessXferCode(sts);
      }

      //if (State > STATE.S5)
      //{
      //if (_closeDsRequested)
      //{
        _uiThreadMarshaller.BeginInvoke(() =>
        {
          DisableSource();
        });
      //}
    }

    private STS TransferFileAudio()
    {
      return default;
    }

    private STS TransferNativeAudio()
    {
      return default;
    }

    private STS TransferMemoryFileImage()
    {
      return default;
    }

    private STS TransferMemoryImage()
    {
      return default;
    }

    private STS TransferFileImage()
    {
      return default;
    }

    private STS TransferNativeImage()
    {
      IntPtr dataPtr = IntPtr.Zero;
      IntPtr lockedPtr = IntPtr.Zero;
      STS sts = default;
      try
      {
        sts = WrapInSTS(DGImage.ImageNativeXfer.Get(ref _appIdentity, ref _currentDS, out dataPtr));
        if (sts.RC == TWRC.XFERDONE)
        {
          State = STATE.S7;

          lockedPtr = Lock(dataPtr);

          byte[]? data = null;


          if (ImageTools.IsDib(lockedPtr))
          {
            data = ImageTools.GetBitmapData(XferMemPool, lockedPtr);
          }
          else if (ImageTools.IsTiff(lockedPtr))
          {
            data = ImageTools.GetTiffData(XferMemPool, lockedPtr);
          }

          if (data != null)
          {
            try
            {
              var args = new DataTransferredEventArgs(this, true, data);
              DataTransferred?.Invoke(this, args);
            }
            catch { }
            finally
            {
              XferMemPool.Return(data);
            }
          }
        }
        else
        {
          HandleNonSuccessXferCode(sts);
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
      finally
      {
        State = STATE.S6;
        if (lockedPtr != IntPtr.Zero)
        {
          Unlock(dataPtr);
        }
        if (dataPtr != IntPtr.Zero)
        {
          Free(dataPtr);
        }
      }
      return sts;
    }

    private void HandleNonSuccessXferCode(STS sts)
    {
      switch (sts.RC)
      {
        case TWRC.SUCCESS:
        case TWRC.XFERDONE:
          // ok to keep going
          break;
        case TWRC.CANCEL:
          TW_PENDINGXFERS pending = default;
          DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending);
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
              DGControl.PendingXfers.EndXfer(ref _appIdentity, ref _currentDS, ref pending);
              break;
            case TWCC.OPERATIONERROR:
              GetCapCurrent(CAP.CAP_INDICATORS, out TW_BOOL showIndicator);
              if (_userInterface.ShowUI == 0 && showIndicator == TW_BOOL.False)
              {
                // todo: alert user and drop to S4
              }
              break;
          }
          break;
      }
    }
  }
}
