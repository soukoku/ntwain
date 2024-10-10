#if WINDOWS || NETFRAMEWORK
using NTwain.Data;
using NTwain.Native;
using NTwain.Triplets;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;
using MSG = NTwain.Data.MSG;

namespace NTwain
{

    // contains parts for winform/wpf message loop integration

    partial class TwainAppSession : IMessageFilter
    {
        HwndSource? _wpfhook;

        /// <summary>
        /// Registers this session for use in a Winform UI thread.
        /// </summary>
        public void AddWinformFilter()
        {
            Application.AddMessageFilter(this);
        }
        /// <summary>
        /// Unregisters this session if previously registered with <see cref="AddWinformFilter"/>.
        /// </summary>
        public void RemoveWinformFilter()
        {
            Application.RemoveMessageFilter(this);
        }

        /// <summary>
        /// Registers this session for use in a WPF UI thread.
        /// This requires the hwnd used in <see cref="OpenDSM"/>
        /// be a valid WPF window handle.
        /// </summary>
        public void AddWpfHook()
        {
            if (_wpfhook == null)
            {
                _wpfhook = HwndSource.FromHwnd(_hwnd);
                _wpfhook.AddHook(WpfHook);
            }
        }
        /// <summary>
        /// Unregisters this session if previously registered with <see cref="AddWpfHook"/>.
        /// </summary>
        public void RemoveWpfHook()
        {
            if (_wpfhook != null)
            {
                _wpfhook.RemoveHook(WpfHook);
                _wpfhook = null;
            }
        }

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            return WndProc(m.HWnd, m.Msg, m.WParam, m.LParam);
        }

        IntPtr WpfHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = WndProc(hwnd, msg, wParam, lParam);
            return IntPtr.Zero;
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
                WIN_MESSAGE winMsg = new()
                {
                    hwnd = hWnd,
                    message = (uint)msg,
                    wParam = wParam,
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
                        //Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] CheckIfTwainMessage at state {State} with MSG={_procEvent.TWMessage}.");
                        HandleSourceMsg((MSG)_procEvent.TWMessage);
                    }
                }
            }
            return handled;
        }
    }
}
#endif