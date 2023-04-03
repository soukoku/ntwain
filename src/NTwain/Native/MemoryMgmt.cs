using System;
using System.Runtime.InteropServices;

namespace NTwain.Native
{
  static class NativeMemoryMethods
  {
    [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalAlloc")]
    public static extern IntPtr WinGlobalAlloc(AllocFlag uFlags, UIntPtr dwBytes);

    [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalFree")]
    public static extern IntPtr WinGlobalFree(IntPtr hMem);

    [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalLock")]
    public static extern IntPtr WinGlobalLock(IntPtr handle);

    [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalUnlock")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool WinGlobalUnlock(IntPtr handle);

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
  }
}
