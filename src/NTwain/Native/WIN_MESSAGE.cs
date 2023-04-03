using System;
using System.Runtime.InteropServices;

namespace NTwain.Native
{
  /// <summary>
  /// The MSG structure in Windows for TWAIN use.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  struct WIN_MESSAGE
  {
    public IntPtr hwnd;
    public uint message;
    public IntPtr wParam;
    public IntPtr lParam;
    uint _time;
    int _x;
    int _y;
    uint lprivate;
  }
}
