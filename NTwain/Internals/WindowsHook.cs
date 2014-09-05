using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace NTwain.Internals
{

    /// <summary>
    /// Abstracts out wnd proc hook on Windows from InternalMessageLoopHook class.
    /// This allows things to not depend on PresentationCore.dll at runtime on mono.
    /// </summary>
    class WindowsHook : IDisposable
    {
        IDisposable _win;
        IWinMessageFilter _filter;

        public WindowsHook(IWinMessageFilter filter)
        {
            _filter = filter;

            HwndSource win = null;
            try
            {
                // hook into windows msg loop for old twain to post msgs.
                // the style values are purely guesses here with
                // CS_NOCLOSE, WS_TILEDWINDOW, and WS_EX_APPWINDOW
                win = new HwndSource(0x0200, 0xCF0000, 0x40000, 0, 0, "NTWAIN_LOOPER", IntPtr.Zero);
                Handle = win.Handle;
                win.AddHook(WndProc);
                _win = win;
            }
            catch
            {
                if (win != null) { win.Dispose(); }
                throw;
            }
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (_filter != null)
            {
                handled = _filter.IsTwainMessage(hwnd, msg, wParam, lParam);
            }
            if (!handled)
            {
                Debug.WriteLine("Hwnd=" + hwnd);
                handled = true;
                // unnecessary to do default wndproc?
                return NativeMethods.DefWindowProc(hwnd, (uint)msg, wParam, lParam);
            }
            return IntPtr.Zero;
        }

        public IntPtr Handle { get; private set; }


        #region IDisposable Members

        public void Dispose()
        {
            if (_win != null)
            {
                ((HwndSource)_win).RemoveHook(WndProc);
                _win.Dispose();
                _win = null;
                Handle = IntPtr.Zero;
            }
        }

        #endregion
    }
}
