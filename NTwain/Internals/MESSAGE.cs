using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Internals
{


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
}
