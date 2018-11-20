using NTwain.Data;
using NTwain.Data.Win32;
using NTwain.Internals;
using NTwain.Threading;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace NTwain
{
    partial class TwainSession
    {
        internal TW_USERINTERFACE _lastEnableUI;
        internal bool _disableDSNow;

        ReturnCode Handle32BitCallback(TW_IDENTITY origin, TW_IDENTITY destination,
            DataGroups dg, DataArgumentType dat, Message msg, IntPtr data)
        {
            Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {nameof(Handle32BitCallback)}({dg}, {dat}, {msg}, {data})");
            BeginInvoke(() =>
            {
                Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: in BeginInvoke {nameof(Handle32BitCallback)}({dg}, {dat}, {msg}, {data})");
                HandleSourceMsg(msg);
            });
            Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: after BeginInvoke {nameof(Handle32BitCallback)}({dg}, {dat}, {msg}, {data})");
            return ReturnCode.Success;
        }

        internal bool HandleWindowsMessage(ref MSG msg)
        {
            var handled = false;
            if (State > TwainState.S4)
            {
                // transform it into a pointer for twain
                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    msgPtr = Config.MemoryManager.Allocate((uint)Marshal.SizeOf(msg));
                    IntPtr locked = Config.MemoryManager.Lock(msgPtr);
                    Marshal.StructureToPtr(msg, locked, false);

                    TW_EVENT evt = new TW_EVENT { pEvent = locked };
                    if (handled = DGControl.Event.ProcessEvent(ref evt) == ReturnCode.DSEvent)
                    {
                        Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {nameof(HandleWindowsMessage)} with {evt.TWMessage}");
                        HandleSourceMsg((Message)evt.TWMessage);
                    }
                }
                finally
                {
                    if (msgPtr != IntPtr.Zero) { Config.MemoryManager.Free(msgPtr); }
                }
            }
            return handled;
        }

        private void HandleSourceMsg(Message msg)
        {
            Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {nameof(HandleSourceMsg)}({msg})");
            switch (msg)
            {
                case Message.DeviceEvent:
                    TW_DEVICEEVENT de = default;
                    var rc = DGControl.DeviceEvent.Get(ref de);
                    if (rc == ReturnCode.Success)
                    {
                        OnDeviceEventReceived(new DeviceEventArgs { Data = de });
                    }
                    break;
                case Message.XferReady:
                    if (State < TwainState.S6)
                    {
                        State = TwainState.S6;
                    }
                    DoTransferRoutine();
                    break;
                case Message.CloseDSReq:
                    if (_state > TwainState.S5)
                    {
                        // do it after end of current xfer routine.
                        _disableDSNow = true;
                    }
                    else
                    {
                        DGControl.UserInterface.DisableDS(ref _lastEnableUI, false);
                    }
                    break;
                case Message.CloseDSOK:
                    DGControl.UserInterface.DisableDS(ref _lastEnableUI, true);
                    break;
            }
        }

        private void DoTransferRoutine()
        {
            var xMech = GetTransferMechs();

            TW_PENDINGXFERS pending = default;
            var rc = DGControl.PendingXfers.Get(ref pending);
            if (rc == ReturnCode.Success)
            {
                do
                {
                    var readyArgs = new TransferReadyEventArgs(CurrentSource, pending.Count, pending.EndOfJob)
                    {
                        CancelAll = _disableDSNow
                    };
                    OnTransferReady(readyArgs);


                    #region actually handle xfer

                    if (readyArgs.CancelAll || _disableDSNow)
                    {
                        rc = DGControl.PendingXfers.Reset(ref pending);
                    }
                    else
                    {
                        if (!readyArgs.CancelCurrent)
                        {
                            if (xMech.ImageMech.HasValue)
                            {
                                switch (xMech.ImageMech.Value)
                                {
                                    case XferMech.Memory:
                                        rc = DoImageMemoryXfer();
                                        break;
                                    case XferMech.File:
                                        rc = DoImageFileXfer();
                                        break;
                                    case XferMech.MemFile:
                                        rc = DoImageMemoryFileXfer();
                                        break;
                                    case XferMech.Native:
                                    default: // always assume native
                                        rc = DoImageNativeXfer();
                                        break;
                                }
                            }
                            if (xMech.AudioMech.HasValue)
                            {
                                switch (xMech.AudioMech.Value)
                                {
                                    case XferMech.File:
                                        rc = DoAudioFileXfer();
                                        break;
                                    case XferMech.Native:
                                    default: // always assume native
                                        rc = DoAudioNativeXfer();
                                        break;
                                }
                            }
                        }

                        if (rc != ReturnCode.Success)// && StopOnTransferError)
                        {
                            // end xfer without setting rc to exit (good/bad?)
                            DGControl.PendingXfers.Reset(ref pending);
                        }
                        else
                        {
                            rc = DGControl.PendingXfers.EndXfer(ref pending);
                        }
                    }
                    #endregion

                } while (rc == ReturnCode.Success && pending.Count != 0 && !_disableDSNow);
            }
            else
            {
                HandleXferReturnCode(rc);
            }

            if (_disableDSNow)
            {
                DGControl.UserInterface.DisableDS(ref _lastEnableUI, false);
            }
        }

        private ReturnCode DoImageNativeXfer()
        {
            throw new NotImplementedException();
        }

        private ReturnCode DoImageMemoryFileXfer()
        {
            throw new NotImplementedException();
        }

        private ReturnCode DoImageFileXfer()
        {
            throw new NotImplementedException();
        }

        private ReturnCode DoImageMemoryXfer()
        {
            throw new NotImplementedException();
        }

        private ReturnCode DoAudioNativeXfer()
        {
            throw new NotImplementedException();
        }

        private ReturnCode DoAudioFileXfer()
        {
            throw new NotImplementedException();
        }

        private void HandleXferReturnCode(ReturnCode rc)
        {
            switch (rc)
            {
                case ReturnCode.Success:
                case ReturnCode.XferDone:
                case ReturnCode.Cancel:
                    // ok to keep going
                    break;
                default:
                    var status = CurrentSource.GetStatus();
                    OnTransferError(new TransferErrorEventArgs(rc, status));
                    break;
            }
        }

        TransferMechs GetTransferMechs()
        {
            TransferMechs retVal = default;
            bool xferImage = true; // default to always xfer image
            bool xferAudio = false;
            DataGroups xferGroup = DataGroups.None;
            XferMech imgXferMech = XferMech.Native;
            XferMech audXferMech = XferMech.Native;
            if (DGControl.XferGroup.Get(ref xferGroup) == ReturnCode.Success)
            {
                xferAudio = (xferGroup & DataGroups.Audio) == DataGroups.Audio;
                // some DS returns none but we will assume it's image
                xferImage = xferGroup == DataGroups.None || (xferGroup & DataGroups.Image) == DataGroups.Image;
            }
            // TODO: restore this
            if (xferImage)
            {
                //imgXferMech = CurrentSource.Capabilities.ICapXferMech.GetCurrent();
                retVal.ImageMech = imgXferMech;
            }
            if (xferAudio)
            {
                //audXferMech = CurrentSource.Capabilities.ACapXferMech.GetCurrent();
                retVal.AudioMech = audXferMech;
            }
            return retVal;
        }

        struct TransferMechs
        {
            public XferMech? ImageMech;
            public XferMech? AudioMech;
        }
    }
}
