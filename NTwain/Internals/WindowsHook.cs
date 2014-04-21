using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace NTwain.Internals
{

    /// <summary>
    /// Abstracts out wnd proc hook on Windows from MessageLoop class.
    /// This allows things to not depend on PresentationCore.dll at runtime on mono.
    /// Not that everything works yet in mono but it's something.
    /// </summary>
    class WindowsHook : IDisposable
    {
        IDisposable _win;
        WndProcHook _hook;

        public WindowsHook(WndProcHook hook)
        {
            // hook into windows msg loop for old twain to post msgs.
            // the style values are purely guesses here with
            // CS_NOCLOSE, WS_DISABLED, and WS_EX_NOACTIVATE
            HwndSource win = null;
            try
            {
                win = new HwndSource(0x0200, 0x8000000, 0x8000000, 0, 0, "NTWAIN_LOOPER", IntPtr.Zero);
                Handle = win.Handle;
                win.AddHook(WndProc);
                _win = win;
                _hook = hook;
            }
            catch
            {
                if (win != null) { win.Dispose(); }
                throw;
            }
        }

        public delegate void WndProcHook(ref MESSAGE winMsg, ref bool handled);


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (_hook != null)
            {
                var winmsg = new MESSAGE(hwnd, msg, wParam, lParam);
                _hook(ref winmsg, ref handled);
            }
            return IntPtr.Zero;
        }

        public IntPtr Handle { get; private set; }


        /// <summary>
        /// The MSG structure in Windows for TWAIN use.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MESSAGE
        {
            public MESSAGE(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam)
            {
                _hwnd = hwnd;
                _message = (uint)message;
                _wParam = wParam;
                _lParam = lParam;
                _time = 0;
                _x = 0;
                _y = 0;
            }

            IntPtr _hwnd;
            uint _message;
            IntPtr _wParam;
            IntPtr _lParam;
            uint _time;
            int _x;
            int _y;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_win != null)
            {
                ((HwndSource)_win).RemoveHook(WndProc);
                _win.Dispose();
                _win = null;
            }
        }

        #endregion
    }
}
