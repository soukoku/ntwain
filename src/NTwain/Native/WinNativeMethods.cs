using System;
using System.Runtime.InteropServices;

namespace NTwain.Native
{
  /// <summary>
  /// Native methods for windows.
  /// </summary>
  static partial class WinNativeMethods
  {
#if NET7_0_OR_GREATER
    [LibraryImport("kernel32", SetLastError = true)]
    public static partial IntPtr GlobalAlloc(AllocFlag uFlags, UIntPtr dwBytes);

    [LibraryImport("kernel32", SetLastError = true)]
    public static partial IntPtr GlobalFree(IntPtr hMem);

    [LibraryImport("kernel32", SetLastError = true)]
    public static partial IntPtr GlobalLock(IntPtr handle);

    [LibraryImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GlobalUnlock(IntPtr handle);
#else
    [DllImport("kernel32", SetLastError = true)]
    public static extern IntPtr GlobalAlloc(AllocFlag uFlags, UIntPtr dwBytes);

    [DllImport("kernel32", SetLastError = true)]
    public static extern IntPtr GlobalFree(IntPtr hMem);

    [DllImport("kernel32", SetLastError = true)]
    public static extern IntPtr GlobalLock(IntPtr handle);

    [DllImport("kernel32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GlobalUnlock(IntPtr handle);
#endif

    [Flags]
    public enum AllocFlag : uint
    {
      /// <summary>
      /// Allocates fixed memory. The return value is a pointer.
      /// </summary>
      GMEM_FIXED = 0,
      /// <summary>
      /// Allocates movable memory. Memory blocks are never moved in physical memory, but they can be moved within the default heap.
      /// The return value is a handle to the memory object. To translate the handle into a pointer, use the GlobalLock function.
      /// </summary>
      GMEM_MOVEABLE = 2,
      /// <summary>
      /// Initializes memory contents to zero.
      /// </summary>
      GMEM_ZEROINIT = 0x40,
      GPTR = GMEM_FIXED | GMEM_ZEROINIT,
      GHND = GMEM_MOVEABLE | GMEM_ZEROINIT
    }

//    [Flags]
//    public enum PEEK_MESSAGE_REMOVE_TYPE : uint
//    {
//      PM_NOREMOVE = 0x00000000,
//      PM_REMOVE = 0x00000001,
//      PM_NOYIELD = 0x00000002,
//      PM_QS_INPUT = 0x04070000,
//      PM_QS_POSTMESSAGE = 0x00980000,
//      PM_QS_PAINT = 0x00200000,
//      PM_QS_SENDMESSAGE = 0x00400000,
//    }

//#if NET7_0_OR_GREATER
//    [LibraryImport("USER32.dll")]
//    public static partial int PeekMessageW(ref WIN_MESSAGE lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, PEEK_MESSAGE_REMOVE_TYPE wRemoveMsg);

//    [LibraryImport("USER32.dll", SetLastError = true)]
//    public static partial int GetMessageW(ref WIN_MESSAGE lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

//    [LibraryImport("USER32.dll")]
//    public static partial int TranslateMessage(ref WIN_MESSAGE lpMsg);

//    [LibraryImport("USER32.dll")]
//    public static partial nint DispatchMessageW(ref WIN_MESSAGE lpMsg);
//#else
//    [DllImport("USER32.dll")]
//    public static extern int PeekMessageW(ref WIN_MESSAGE lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, PEEK_MESSAGE_REMOVE_TYPE wRemoveMsg);

//    [DllImport("USER32.dll", SetLastError = true)]
//    public static extern int GetMessageW(ref WIN_MESSAGE lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

//    [DllImport("USER32.dll")]
//    public static extern int TranslateMessage(ref WIN_MESSAGE lpMsg);

//    [DllImport("USER32.dll")]
//    public static extern nint DispatchMessageW(ref WIN_MESSAGE lpMsg);
//#endif
  }
}
