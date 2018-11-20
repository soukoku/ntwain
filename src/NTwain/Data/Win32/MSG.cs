using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Data.Win32
{
    /// <summary>
    /// The MSG structure in Windows for TWAIN use.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct MSG
    {
        public IntPtr hwnd;
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        int time;
        int x;
        int y;
    }
}
