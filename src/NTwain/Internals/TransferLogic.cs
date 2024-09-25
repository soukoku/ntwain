using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Internals
{
    /// <summary>
    /// Contains the actual data transfer logic since TwainSession is getting too large.
    /// </summary>
    static class TransferLogic
    {
        /// <summary>
        /// Performs the TWAIN transfer routine at state 6. 
        /// </summary>
        public static void DoTransferRoutine(ITwainSessionInternal session)
        {
            #region get xfer types

            bool xferImage = true; // default to always xfer image
            bool xferAudio = false;
            DataGroups xferGroup = DataGroups.None;
            XferMech imgXferMech = XferMech.Native;
            XferMech audXferMech = XferMech.Native;
            if (session.DGControl.XferGroup.Get(ref xferGroup) == ReturnCode.Success)
            {
                xferAudio = (xferGroup & DataGroups.Audio) == DataGroups.Audio;
                // check for Plustek OpticSlim 2680H, this scanner returns wrong xferGroup after first scanning
                if (session.CurrentSource.Identity.ProductName.IndexOf("Plustek", StringComparison.OrdinalIgnoreCase) > -1 && 
                    session.CurrentSource.Identity.ProductName.IndexOf("OpticSlim", StringComparison.OrdinalIgnoreCase) > -1 && 
                    session.CurrentSource.Identity.ProductName.IndexOf("2680H", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    xferImage = true;
                }
                else
                {
                    xferImage = xferGroup == DataGroups.None || (xferGroup & DataGroups.Image) == DataGroups.Image;
                }
            }
            // some DS end up getting none but we will assume it's image
            if (xferImage)
            {
                imgXferMech = session.CurrentSource.Capabilities.ICapXferMech.GetCurrent();
            }
            if (xferAudio)
            {
                var mech = session.CurrentSource.Capabilities.ACapXferMech.GetCurrent();
            }

            #endregion

            var pending = new TWPendingXfers();
            ReturnCode rc;
            if (xferImage)
            {
                rc = ReturnCode.Success;
            }
            else
            {
                rc = session.DGControl.PendingXfers.Get(pending);
            }

            if (rc == ReturnCode.Success)
            {
                do
                {
                    #region raise xfer ready

                    var preXferArgs = new TransferReadyEventArgs(session.CurrentSource, pending.Count, pending.EndOfJob); ;
                    session.SafeSyncableRaiseEvent(preXferArgs);

                    #endregion

                    #region actually handle xfer

                    if (preXferArgs.CancelAll || session.CloseDSRequested)
                    {
                        rc = session.DGControl.PendingXfers.Reset(pending);
                    }
                    else
                    {
                        if (!preXferArgs.CancelCurrent)
                        {
                            if (xferImage)
                            {
                                switch (imgXferMech)
                                {
                                    case XferMech.Memory:
                                        rc = DoImageMemoryXfer(session);
                                        break;
                                    case XferMech.File:
                                        rc = DoImageFileXfer(session);
                                        break;
                                    case XferMech.MemFile:
                                        rc = DoImageMemoryFileXfer(session);
                                        break;
                                    case XferMech.Native:
                                    default: // always assume native
                                        rc = DoImageNativeXfer(session);
                                        break;
                                }
                            }
                            if (xferAudio)
                            {
                                switch (audXferMech)
                                {
                                    case XferMech.File:
                                        rc = DoAudioFileXfer(session);
                                        break;
                                    case XferMech.Native:
                                    default: // always assume native
                                        rc = DoAudioNativeXfer(session);
                                        break;
                                }
                            }
                        }

                        if (rc != ReturnCode.Success && session.StopOnTransferError)
                        {
                            // end xfer without setting rc to exit (good/bad?)
                            session.DGControl.PendingXfers.Reset(pending);
                        }
                        else
                        {
                            rc = session.DGControl.PendingXfers.EndXfer(pending);
                        }
                    }
                    #endregion

                } while (rc == ReturnCode.Success && pending.Count != 0 && !session.CloseDSRequested);
            }
            else
            {
                HandleReturnCode(session, rc);
            }

            // some poorly written scanner drivers return failure on EndXfer so also check for pending count now.
            // this may break with other sources but we'll see
            if (//pending.Count == 0 && 
                session.State > 5)
            {
                session.ChangeState(5, true);
                session.DisableSource();
            }
        }

        private static void HandleReturnCode(ITwainSessionInternal session, ReturnCode rc)
        {
            switch (rc)
            {
                case ReturnCode.Success:
                case ReturnCode.XferDone:
                    // ok to keep going
                    break;
                case ReturnCode.Cancel:
                    session.SafeSyncableRaiseEvent(new TransferCanceledEventArgs());
                    break;
                default:
                    var status = session.CurrentSource.GetStatus();
                    session.SafeSyncableRaiseEvent(new TransferErrorEventArgs(rc, status));
                    break;
            }
        }

        #region audio xfers

        static ReturnCode DoAudioNativeXfer(ITwainSessionInternal session)
        {
            IntPtr dataPtr = IntPtr.Zero;
            IntPtr lockedPtr = IntPtr.Zero;
            ReturnCode xrc = ReturnCode.Failure;
            try
            {
                xrc = session.DGAudio.AudioNativeXfer.Get(ref dataPtr);
                if (xrc == ReturnCode.XferDone)
                {
                    session.ChangeState(7, true);
                    if (dataPtr != IntPtr.Zero)
                    {
                        lockedPtr = PlatformInfo.Current.MemoryManager.Lock(dataPtr);
                    }

                    session.SafeSyncableRaiseEvent(new DataTransferredEventArgs(session.CurrentSource, lockedPtr));
                }
                else
                {
                    HandleReturnCode(session, xrc);
                }
            }
            catch (Exception ex)
            {
                session.SafeSyncableRaiseEvent(new TransferErrorEventArgs(ex));
            }
            finally
            {
                session.ChangeState(6, true);
                // data here is allocated by source so needs to use shared mem calls
                if (lockedPtr != IntPtr.Zero)
                {
                    //PlatformInfo.Current.MemoryManager.Unlock(lockedPtr);
                    PlatformInfo.Current.MemoryManager.Unlock(dataPtr);
                    lockedPtr = IntPtr.Zero;
                }
                if (dataPtr != IntPtr.Zero)
                {
                    PlatformInfo.Current.MemoryManager.Free(dataPtr);
                    dataPtr = IntPtr.Zero;
                }
            }
            return xrc;
        }

        static ReturnCode DoAudioFileXfer(ITwainSessionInternal session)
        {
            string filePath = null;
            TWSetupFileXfer setupInfo;
            ReturnCode xrc = session.DGControl.SetupFileXfer.Get(out setupInfo);
            if (xrc == ReturnCode.Success)
            {
                filePath = setupInfo.FileName;

                xrc = session.DGAudio.AudioFileXfer.Get();
                if (xrc == ReturnCode.XferDone)
                {
                    session.SafeSyncableRaiseEvent(new DataTransferredEventArgs(session.CurrentSource, filePath, (FileFormat)0));
                }
                else
                {
                    HandleReturnCode(session, xrc);
                }
            }
            return xrc;
        }

        #endregion

        #region image xfers

        static ReturnCode DoImageNativeXfer(ITwainSessionInternal session)
        {
            IntPtr dataPtr = IntPtr.Zero;
            IntPtr lockedPtr = IntPtr.Zero;
            ReturnCode xrc = ReturnCode.Failure;
            try
            {
                xrc = session.DGImage.ImageNativeXfer.Get(ref dataPtr);
                if (xrc == ReturnCode.XferDone)
                {
                    session.ChangeState(7, true);
                    if (dataPtr != IntPtr.Zero)
                    {
                        lockedPtr = PlatformInfo.Current.MemoryManager.Lock(dataPtr);
                    }
                    DoImageXferredEventRoutine(session, lockedPtr, null, null, null, (FileFormat)0);
                }
                else
                {
                    HandleReturnCode(session, xrc);
                }
            }
            catch (Exception ex)
            {
                session.SafeSyncableRaiseEvent(new TransferErrorEventArgs(ex));
            }
            finally
            {
                session.ChangeState(6, true);
                // data here is allocated by source so needs to use shared mem calls
                if (lockedPtr != IntPtr.Zero)
                {
                    //PlatformInfo.Current.MemoryManager.Unlock(lockedPtr);
                    PlatformInfo.Current.MemoryManager.Unlock(dataPtr);
                    lockedPtr = IntPtr.Zero;
                }
                if (dataPtr != IntPtr.Zero)
                {
                    PlatformInfo.Current.MemoryManager.Free(dataPtr);
                    dataPtr = IntPtr.Zero;
                }
            }
            return xrc;
        }

        static ReturnCode DoImageFileXfer(ITwainSessionInternal session)
        {
            string filePath = null;
            TWSetupFileXfer setupInfo;
            if (session.DGControl.SetupFileXfer.Get(out setupInfo) == ReturnCode.Success)
            {
                filePath = setupInfo.FileName;
            }

            var xrc = session.DGImage.ImageFileXfer.Get();
            if (xrc == ReturnCode.XferDone)
            {
                DoImageXferredEventRoutine(session, IntPtr.Zero, null, null, filePath, setupInfo.Format);
            }
            else
            {
                HandleReturnCode(session, xrc);
            }
            return xrc;
        }

        static ReturnCode DoImageMemoryXfer(ITwainSessionInternal session)
        {
            TWSetupMemXfer memInfo;
            ReturnCode xrc = session.DGControl.SetupMemXfer.Get(out memInfo);
            if (xrc == ReturnCode.Success)
            {
                TWImageMemXfer xferInfo = new TWImageMemXfer();
                try
                {
                    // how to tell if going to xfer in strip vs tile?
                    // if tile don't allocate memory in app?

                    xferInfo.Memory = new TWMemory
                    {
                        Flags = MemoryFlags.AppOwns | MemoryFlags.Pointer,
                        Length = memInfo.Preferred,
                        TheMem = PlatformInfo.Current.MemoryManager.Allocate(memInfo.Preferred)
                    };


                    do
                    {
                        xrc = session.DGImage.ImageMemXfer.Get(xferInfo);

                        if (xrc == ReturnCode.Success ||
                            xrc == ReturnCode.XferDone)
                        {
                            if (session.State != 7) { session.ChangeState(7, true); }

                            // optimize and allocate buffer only once instead of inside the loop?
                            byte[] buffer = new byte[(int)xferInfo.BytesWritten];

                            IntPtr lockPtr = IntPtr.Zero;
                            try
                            {
                                lockPtr = PlatformInfo.Current.MemoryManager.Lock(xferInfo.Memory.TheMem);
                                Marshal.Copy(lockPtr, buffer, 0, buffer.Length);
                            }
                            finally
                            {
                                if (lockPtr != IntPtr.Zero)
                                {
                                    PlatformInfo.Current.MemoryManager.Unlock(xferInfo.Memory.TheMem);
                                }
                            }
                            DoImageXferredEventRoutine(session, IntPtr.Zero, xferInfo, buffer, null, (FileFormat)0);
                        }
                    } while (xrc == ReturnCode.Success);

                    HandleReturnCode(session, xrc);
                }
                catch (Exception ex)
                {
                    session.SafeSyncableRaiseEvent(new TransferErrorEventArgs(ex));
                }
                finally
                {
                    session.ChangeState(6, true);
                    if (xferInfo.Memory.TheMem != IntPtr.Zero)
                    {
                        PlatformInfo.Current.MemoryManager.Free(xferInfo.Memory.TheMem);
                    }
                }

            }
            return xrc;
        }

        static ReturnCode DoImageMemoryFileXfer(ITwainSessionInternal session)
        {
            // since it's memory-file xfer need info from both (maybe)
            TWSetupMemXfer memInfo;
            TWSetupFileXfer fileInfo;
            ReturnCode xrc = ReturnCode.Failure;
            if (session.DGControl.SetupMemXfer.Get(out memInfo) == ReturnCode.Success &&
                session.DGControl.SetupFileXfer.Get(out fileInfo) == ReturnCode.Success)
            {
                TWImageMemXfer xferInfo = new TWImageMemXfer();
                var tempFile = Path.GetTempFileName();
                string finalFile = null;
                try
                {
                    // no strip or tile here, just chunks
                    xferInfo.Memory = new TWMemory
                    {
                        Flags = MemoryFlags.AppOwns | MemoryFlags.Pointer,
                        Length = memInfo.Preferred,
                        TheMem = PlatformInfo.Current.MemoryManager.Allocate(memInfo.Preferred)
                    };

                    xrc = ReturnCode.Success;
                    using (var outStream = File.OpenWrite(tempFile))
                    {
                        do
                        {
                            xrc = session.DGImage.ImageMemFileXfer.Get(xferInfo);

                            if (xrc == ReturnCode.Success ||
                                xrc == ReturnCode.XferDone)
                            {
                                session.ChangeState(7, true);
                                byte[] buffer = new byte[(int)xferInfo.BytesWritten];

                                IntPtr lockPtr = IntPtr.Zero;
                                try
                                {
                                    lockPtr = PlatformInfo.Current.MemoryManager.Lock(xferInfo.Memory.TheMem);
                                    Marshal.Copy(lockPtr, buffer, 0, buffer.Length);
                                }
                                finally
                                {
                                    if (lockPtr != IntPtr.Zero)
                                    {
                                        PlatformInfo.Current.MemoryManager.Unlock(xferInfo.Memory.TheMem);
                                        //PlatformInfo.Current.MemoryManager.Unlock(lockPtr);
                                    }
                                }
                                outStream.Write(buffer, 0, buffer.Length);
                            }
                        } while (xrc == ReturnCode.Success);
                    }

                    if (xrc == ReturnCode.XferDone)
                    {
                        finalFile = fileInfo.ChangeExtensionByFormat(tempFile);
                        File.Move(tempFile, finalFile);
                    }
                    else
                    {
                        HandleReturnCode(session, xrc);
                    }
                }
                catch (Exception ex)
                {
                    session.SafeSyncableRaiseEvent(new TransferErrorEventArgs(ex));
                }
                finally
                {
                    session.ChangeState(6, true);
                    if (xferInfo.Memory.TheMem != IntPtr.Zero)
                    {
                        PlatformInfo.Current.MemoryManager.Free(xferInfo.Memory.TheMem);
                    }
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }

                if (File.Exists(finalFile))
                {
                    DoImageXferredEventRoutine(session, IntPtr.Zero, null, null, finalFile, fileInfo.Format);
                }
            }
            return xrc;
        }

        static void DoImageXferredEventRoutine(ITwainSessionInternal session, IntPtr dataPtr, TWImageMemXfer memInfo, byte[] memData, string filePath, FileFormat format)
        {
            DataTransferredEventArgs args = null;

            if (dataPtr != IntPtr.Zero)
            {
                args = new DataTransferredEventArgs(session.CurrentSource, dataPtr);
            }
            else if (memData != null)
            {
                args = new DataTransferredEventArgs(session.CurrentSource, memInfo, memData);
            }
            else
            {
                args = new DataTransferredEventArgs(session.CurrentSource, filePath, format);
            }
            session.SafeSyncableRaiseEvent(args);
        }

        #endregion
    }
}
