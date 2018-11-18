using NTwain.Data;
using NTwain.Internals;
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

        /// <summary>
        /// If on Windows pass all messages from WndProc here to handle it
        /// for TWAIN-specific things.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// true if handled by TWAIN.
        /// </returns>
        public bool HandleWndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            var handled = false;
            if (State > TwainState.S4)
            {
                var winMsg = new MSG(hwnd, msg, wParam, lParam);

                // transform it into a pointer for twain
                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    msgPtr = Config.MemoryManager.Allocate((uint)Marshal.SizeOf(winMsg));
                    IntPtr locked = Config.MemoryManager.Lock(msgPtr);
                    Marshal.StructureToPtr(winMsg, locked, false);

                    TW_EVENT evt = new TW_EVENT { pEvent = locked };
                    if (handled = DGControl.Event.ProcessEvent(ref evt) == ReturnCode.DSEvent)
                    {
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
                    DGControl.UserInterface.DisableDS(ref _lastEnableUI, false);
                    break;
                case Message.CloseDSOK:
                    DGControl.UserInterface.DisableDS(ref _lastEnableUI, true);
                    break;
            }
        }

        private void DoTransferRoutine()
        {
            throw new NotImplementedException();
        }
    }
}
