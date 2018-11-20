using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Data.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct WNDCLASSEX
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public ClassStyles style;
        public IntPtr lpfnWndProc; // not WndProc -- careful   
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;
    }

    delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

}
