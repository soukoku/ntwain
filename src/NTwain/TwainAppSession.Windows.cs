using NTwain.Data;
using NTwain.Native;
using NTwain.Triplets;
using System;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using MSG = NTwain.Data.MSG;

namespace NTwain
{

    // contains parts for winform/wpf message loop integration

    partial class TwainAppSession : IWin32MessageFilter
    {

        bool IWin32MessageFilter.PreFilterMessage(ref Win32Message m)
        {
            return WndProc(m.HWnd, (int)m.Msg, (nint)m.WParam, m.LParam);
        }

        /// <summary>
        /// Method to check window message and handle it if it's for the TWAIN source.'
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public bool WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            // this handles the message from a typical WndProc message loop and checks if it's for the TWAIN source.
            bool handled = false;
            if (_state >= STATE.S5)
            {
                Windows.Win32.UI.WindowsAndMessaging.MSG winMsg = new()
                {
                    hwnd = (HWND)hWnd,
                    message = (uint)msg,
                    wParam = TWPlatform.Is32bit ? new UIntPtr((uint)wParam.ToInt32()) : new UIntPtr((ulong)wParam.ToInt64()),
                    lParam = lParam
                };
                // no need to do another lock call when using marshal alloc
                if (_procEvent.pEvent == IntPtr.Zero)
                    _procEvent.pEvent = Marshal.AllocHGlobal(Marshal.SizeOf(winMsg));
                Marshal.StructureToPtr(winMsg, _procEvent.pEvent, true);

                if (!_closeDsRequested)
                {
                    var rc = DGControl.Event.ProcessEvent(ref _appIdentity, ref _currentDS, ref _procEvent);
                    handled = rc == TWRC.DSEVENT;
                    if (_procEvent.TWMessage != 0 && (handled || rc == TWRC.NOTDSEVENT))
                    {
                        //Logger.LogTrace("[thread {ThreadId}] CheckIfTwainMessage at state {State} with MSG={Msg}.",
                        //    Environment.CurrentManagedThreadId, State, _procEvent.TWMessage);
                        HandleSourceMsg((MSG)_procEvent.TWMessage);
                    }
                }
            }
            return handled;
        }
    }
}