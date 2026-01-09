using Microsoft.Extensions.Logging;
using NTwain.Data;
using NTwain.Events;
using NTwain.Imaging;
using NTwain.Triplets;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace NTwain;

// this file contains the actual transfer loop logic, not bogged down with thread handling.

partial class TransferLoopThread
{
    struct XferMechs
    {
        public TWSX ImageMech;
        public TWSX AudioMech;
    }

    XferMechs DetermineTransferMechs()
    {
        // don't think there are any actual audio twain devices so this is mostly theoretical
        var hasAudio = DGControl.XferGroup.Get(_twain.AppIdentity, _twain.CurrentSource!, out DG xferType) == TWRC.SUCCESS &&
            xferType.HasFlag(DG.AUDIO);

        var imgMech = _twain.Caps.ICAP_XFERMECH.GetCurrent().FirstOrDefault();
        var audioMech = hasAudio ? _twain.Caps.ACAP_XFERMECH.GetCurrent().FirstOrDefault() : TWSX.NATIVE;

        return new XferMechs() { ImageMech = imgMech, AudioMech = audioMech };
    }

    STS EnterTransferLoop()
    {
        _twain.State = STATE.S6;

        // initial info gathering
        var xferMechs = DetermineTransferMechs();
        if (_twain.Logger.IsEnabled(LogLevel.Trace))
            _twain.Logger.LogTrace("Determined transfer mech for image: {iMech}, audio: {aMech}.", xferMechs.ImageMech, xferMechs.AudioMech);

        string phase = "";

        TW_PENDINGXFERS pending = TW_PENDINGXFERS.DONTCARE();
        var sts = _twain.WrapInSTS(DGControl.PendingXfers.Get(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
        if (sts.RC != TWRC.SUCCESS)
        {
            phase = "PendingXfers.Get";
            var args = new TransferErrorEventArgs(sts, phase);
            _twain.RaiseTransferError(args);
            return sts;
        }

        // test making this reusable
        var readyArgs = new TransferReadyEventArgs(_twain, xferMechs.ImageMech, xferMechs.AudioMech);

        while (sts.RC == TWRC.SUCCESS && pending.Count != 0)
        {
            // give app chance to cancel
            readyArgs.UpdatePending(ref pending);
            _twain.RaiseTransferReady(readyArgs);

            if (readyArgs.Cancel == CancelType.EndNow || CloseDsRequested)
            {
                sts = ForceEndTransfer();
            }
            else if (readyArgs.Cancel == CancelType.SkipCurrent)
            {
                pending = TW_PENDINGXFERS.DONTCARE();
                sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
                if (sts.RC == TWRC.SUCCESS)
                {
                    if (readyArgs.XferType == XferType.Audio)
                    {
                        _twain.State = STATE.S6;
                    }
                    else
                    {
                        _twain.State = pending.Count == 0 ? STATE.S5 : STATE.S6;
                    }
                }
            }
            else
            {
                if (readyArgs.Cancel == CancelType.Graceful)
                {
                    // ignore rc in this and continue transfer as normal
                    pending = TW_PENDINGXFERS.DONTCARE();
                    DGControl.PendingXfers.StopFeeder(_twain.AppIdentity, _twain.CurrentSource!, ref pending);
                }

                try
                {
                    if (readyArgs.XferType == XferType.Audio)
                    {
                        switch (xferMechs.AudioMech)
                        {
                            case TWSX.NATIVE:
                                phase = "TransferNativeAudio";
                                sts = TransferNativeAudio(ref pending);
                                break;
                            case TWSX.FILE:
                                phase = "TransferFileAudio";
                                sts = TransferFileAudio(ref pending);
                                break;
                        }
                    }
                    else
                    {
                        switch (xferMechs.ImageMech)
                        {
                            case TWSX.NATIVE:
                                phase = "TransferNativeImage";
                                sts = TransferNativeImage(ref pending);
                                break;
                            case TWSX.FILE:
                                phase = "TransferFileImage";
                                sts = TransferFileImage(ref pending);
                                break;
                            case TWSX.MEMORY:
                                phase = "TransferMemoryImage";
                                sts = TransferMemoryImage(ref pending);
                                break;
                            case TWSX.MEMFILE:
                                phase = "TransferMemoryFileImage";
                                sts = TransferMemoryFileImage(ref pending);
                                break;
                        }
                    }

                }
                catch (Exception ex)
                {
                    _twain.RaiseTransferError(new TransferErrorEventArgs(ex, phase));
                }
                HandleTransferResult(ref sts, out bool exitTransferLoop);
                if (exitTransferLoop) break;
            }
        }
        return sts;
    }

    STS ForceEndTransfer()
    {
        var pending = TW_PENDINGXFERS.DONTCARE();
        var sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
        pending = TW_PENDINGXFERS.DONTCARE();
        sts = _twain.WrapInSTS(DGControl.PendingXfers.Reset(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
        if (sts.RC == TWRC.SUCCESS) _twain.State = STATE.S5;
        return sts;
    }

    private void HandleTransferResult(ref STS sts, out bool exitTransferLoop)
    {
        exitTransferLoop = false;
        switch (sts.RC)
        {
            case TWRC.SUCCESS:
            case TWRC.XFERDONE:
                // do nothing
                break;
            case TWRC.CANCEL:
                // might eventually have option to cancel this or all like transfer ready
                sts = ForceEndTransfer();
                exitTransferLoop = true;
                break;
            default:
                // TODO: raise error event
                switch (sts.STATUS.ConditionCode)
                {
                    case TWCC.SEQERROR:
                        // special break down to state 5
                        sts = ForceEndTransfer();
                        exitTransferLoop = true;
                        break;
                    case TWCC.DAMAGEDCORNER:
                    case TWCC.DOCTOODARK:
                    case TWCC.DOCTOOLIGHT:
                    case TWCC.FOCUSERROR:
                    case TWCC.NOMEDIA:
                    case TWCC.PAPERDOUBLEFEED:
                    case TWCC.PAPERJAM:
                        var pending = TW_PENDINGXFERS.DONTCARE();
                        sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
                        break;
                    case TWCC.OPERATIONERROR:
                        var indicators = _twain.Caps.CAP_INDICATORS.GetCurrent().FirstOrDefault();
                        if (_twain.EnabledWithOption == SourceEnableOption.NoUI && indicators == TW_BOOL.False)
                        {
                            // todo: alert user and drop to S4
                            sts = ForceEndTransfer();
                            exitTransferLoop = true;
                        }
                        break;
                }
                break;
        }
    }

    private STS TransferNativeAudio(ref TW_PENDINGXFERS pending)
    {
        IntPtr dataPtr = IntPtr.Zero;
        IntPtr lockedPtr = IntPtr.Zero;
        try
        {
            var sts = _twain.WrapInSTS(DGAudio.AudioNativeXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, out dataPtr));
            if (sts.RC == TWRC.XFERDONE)
            {
                _twain.State = STATE.S7;
                lockedPtr = _twain.MemoryManager.Lock(dataPtr);
                BufferedData? data = default;

                // TODO: don't know how to read wav/aiff from pointer yet

                if (data != null)
                {
                    try
                    {
                        DGAudio.AudioInfo.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_AUDIOINFO info);
                        var args = new TransferredEventArgs(info, data);
                        _twain.RaiseTransferred(args);
                    }
                    catch
                    {
                        data.Dispose();
                    }
                }
            }

            pending = TW_PENDINGXFERS.DONTCARE();
            sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
            if (sts.RC == TWRC.SUCCESS)
            {
                _twain.State = STATE.S6;
            }
            return sts;
        }
        finally
        {
            if (lockedPtr != IntPtr.Zero) _twain.MemoryManager.Unlock(lockedPtr);
            if (dataPtr != IntPtr.Zero) _twain.MemoryManager.Free(dataPtr);
        }
    }

    private STS TransferFileAudio(ref TW_PENDINGXFERS pending)
    {
        // assuming user already configured the transfer in transferready event,
        // get what will be transferred
        var rc = DGControl.SetupFileXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_SETUPFILEXFER fileSetup);
        if (rc != TWRC.SUCCESS) return _twain.WrapInSTS(rc);

        // and just start it
        var sts = _twain.WrapInSTS(DGAudio.AudioFileXfer.Get(_twain.AppIdentity, _twain.CurrentSource!));

        if (sts.RC == TWRC.XFERDONE)
        {
            _twain.State = STATE.S7;
            try
            {
                DGAudio.AudioInfo.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_AUDIOINFO info);
                var args = new TransferredEventArgs(info, fileSetup);
                _twain.RaiseTransferred(args);
            }
            catch { }

            pending = TW_PENDINGXFERS.DONTCARE();
            sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
            if (sts.RC == TWRC.SUCCESS)
            {
                _twain.State = STATE.S6;
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
            DGImage.ImageInfo.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_IMAGEINFO info);
            var sts = _twain.WrapInSTS(DGImage.ImageNativeXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, out dataPtr));
            if (sts.RC == TWRC.XFERDONE)
            {
                _twain.State = STATE.S7;
                lockedPtr = _twain.MemoryManager.Lock(dataPtr);
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
                        var args = new TransferredEventArgs(_twain, info, null, data);
                        _twain.RaiseTransferred(args);
                    }
                    catch
                    {
                        data.Dispose();
                    }
                }


                pending = TW_PENDINGXFERS.DONTCARE();
                sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
                if (sts.RC == TWRC.SUCCESS)
                {
                    _twain.State = pending.Count == 0 ? STATE.S5 : STATE.S6;
                }
            }
            return sts;
        }
        finally
        {
            if (lockedPtr != IntPtr.Zero) _twain.MemoryManager.Unlock(lockedPtr);
            if (dataPtr != IntPtr.Zero) _twain.MemoryManager.Free(dataPtr);
        }
    }

    private STS TransferFileImage(ref TW_PENDINGXFERS pending)
    {
        // TODO: verify this still works

        // assuming user already configured the transfer in transferready event,
        // get what will be transferred
        DGControl.SetupFileXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_SETUPFILEXFER fileSetup);
        // and just start it

        int tries = 0;
    RETRY:
        var sts = _twain.WrapInSTS(DGImage.ImageFileXfer.Get(_twain.AppIdentity, _twain.CurrentSource!));

        if (sts.RC == TWRC.XFERDONE)
        {
            _twain.State = STATE.S7;
            try
            {
                DGImage.ImageInfo.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_IMAGEINFO info);
                var args = new TransferredEventArgs(_twain, info, fileSetup, default);
                _twain.RaiseTransferred(args);
            }
            catch { }

            pending = TW_PENDINGXFERS.DONTCARE();
            sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
            if (sts.RC == TWRC.SUCCESS)
            {
                _twain.State = pending.Count == 0 ? STATE.S5 : STATE.S6;
            }
        }
        else
        {
            // sometimes it errors only due to timing so wait a bit and try again
            if (sts.RC == TWRC.FAILURE && (sts.ConditionCode == TWCC.None || sts.ConditionCode == TWCC.SEQERROR))
            {
                if (tries++ < 3)
                {
                    _twain.Logger.LogDebug("Using fileXfer timing workaround try {Tries}.", tries);
                    Thread.Sleep(500);
                    goto RETRY;
                }
            }
            else
            {
                Debugger.Break();
            }
        }
        return sts;
    }

    private STS TransferMemoryImage(ref TW_PENDINGXFERS pending)
    {
        // TODO: build the image from strips before firing off transferred event


        var rc = DGControl.SetupMemXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_SETUPMEMXFER memSetup);
        if (rc != TWRC.SUCCESS) return _twain.WrapInSTS(rc);
        rc = DGImage.ImageInfo.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_IMAGEINFO info);
        if (rc != TWRC.SUCCESS) return _twain.WrapInSTS(rc);
        rc = DGImage.ImageLayout.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_IMAGELAYOUT layout);
        if (rc != TWRC.SUCCESS) return _twain.WrapInSTS(rc);

        uint buffSize = memSetup.DetermineBufferSize();
        var memPtr = _twain.MemoryManager.Alloc(buffSize);

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
                  DGImage.ImageMemXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, ref memXferOSX) :
                  DGImage.ImageMemXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, ref memXfer);

                if (rc == TWRC.SUCCESS || rc == TWRC.XFERDONE)
                {
                    try
                    {
                        var written = TWPlatform.IsMacOSX ?
                          memXferOSX.BytesWritten : memXfer.BytesWritten;

                        IntPtr lockedPtr = _twain.MemoryManager.Lock(memPtr);

                        // assemble!

                        //Marshal.Copy(lockedPtr, dotnetBuff, 0, (int)written);
                        //outStream.Write(dotnetBuff, 0, (int)written);
                    }
                    finally
                    {
                        _twain.MemoryManager.Unlock(memPtr);
                    }
                }
            } while (rc == TWRC.SUCCESS);
        }
        finally
        {
            if (memPtr != IntPtr.Zero) _twain.MemoryManager.Free(memPtr);
            BufferedData.MemPool.Return(dotnetBuff);
        }


        if (rc == TWRC.XFERDONE)
        {
            try
            {
                DGImage.ImageInfo.Get(_twain.AppIdentity, _twain.CurrentSource!, out info);
                //var args = new DataTransferredEventArgs(info, null, outStream.ToArray());
                //DataTransferred?.Invoke(this, args);
            }
            catch { }

            pending = TW_PENDINGXFERS.DONTCARE();
            var sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
            if (sts.RC == TWRC.SUCCESS)
            {
                _twain.State = pending.Count == 0 ? STATE.S5 : STATE.S6;
            }
            return sts;
        }
        return _twain.WrapInSTS(rc);
    }

    private STS TransferMemoryFileImage(ref TW_PENDINGXFERS pending)
    {
        // TODO: verify this still works

        var rc = DGControl.SetupFileXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_SETUPFILEXFER fileSetup);
        if (rc != TWRC.SUCCESS) return _twain.WrapInSTS(rc);
        rc = DGControl.SetupMemXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_SETUPMEMXFER memSetup);
        if (rc != TWRC.SUCCESS) return _twain.WrapInSTS(rc);

        uint buffSize = memSetup.DetermineBufferSize();
        var memPtr = _twain.MemoryManager.Alloc(buffSize);

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
                  DGImage.ImageMemFileXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, ref memXferOSX) :
                  DGImage.ImageMemFileXfer.Get(_twain.AppIdentity, _twain.CurrentSource!, ref memXfer);

                if (rc == TWRC.SUCCESS || rc == TWRC.XFERDONE)
                {
                    try
                    {
                        var written = TWPlatform.IsMacOSX ?
                          memXferOSX.BytesWritten : memXfer.BytesWritten;

                        IntPtr lockedPtr = _twain.MemoryManager.Lock(memPtr);
                        Marshal.Copy(lockedPtr, dotnetBuff, 0, (int)written);
                        outStream.Write(dotnetBuff, 0, (int)written);
                    }
                    finally
                    {
                        _twain.MemoryManager.Unlock(memPtr);
                    }
                }
            } while (rc == TWRC.SUCCESS);
        }
        finally
        {
            if (memPtr != IntPtr.Zero) _twain.MemoryManager.Free(memPtr);
            BufferedData.MemPool.Return(dotnetBuff);
        }

        if (rc == TWRC.XFERDONE)
        {
            try
            {
                DGImage.ImageInfo.Get(_twain.AppIdentity, _twain.CurrentSource!, out TW_IMAGEINFO info);
                // ToArray bypasses the XferMemPool but I guess this will have to do for now
                var args = new TransferredEventArgs(_twain, info, fileSetup, new BufferedData(outStream.ToArray(), (int)outStream.Length, false));
                _twain.RaiseTransferred(args);
            }
            catch { }

            pending = TW_PENDINGXFERS.DONTCARE();
            var sts = _twain.WrapInSTS(DGControl.PendingXfers.EndXfer(_twain.AppIdentity, _twain.CurrentSource!, ref pending));
            if (sts.RC == TWRC.SUCCESS)
            {
                _twain.State = pending.Count == 0 ? STATE.S5 : STATE.S6;
            }
            return sts;
        }
        return _twain.WrapInSTS(rc);
    }
}
