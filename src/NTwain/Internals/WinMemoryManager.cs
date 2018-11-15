using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace NTwain.Internals
{
    /// <summary>
    /// Provides methods for managing memory on data exchanged with twain sources using old win32 methods.
    /// </summary>
    class WinMemoryManager : IMemoryManager
    {
        public IntPtr Allocate(uint size)
        {
            IntPtr retVal = UnsafeNativeMethods.WinGlobalAlloc(0x0040, new UIntPtr(size));

            if (retVal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return retVal;
        }

        public void Free(IntPtr handle)
        {
            UnsafeNativeMethods.WinGlobalFree(handle);
        }

        public IntPtr Lock(IntPtr handle)
        {
            return UnsafeNativeMethods.WinGlobalLock(handle);
        }

        public void Unlock(IntPtr handle)
        {
            UnsafeNativeMethods.WinGlobalUnlock(handle);
        }


        [SuppressUnmanagedCodeSecurity]
        static class UnsafeNativeMethods
        {
            [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalAlloc")]
            public static extern IntPtr WinGlobalAlloc(uint uFlags, UIntPtr dwBytes);

            [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalFree")]
            public static extern IntPtr WinGlobalFree(IntPtr hMem);

            [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalLock")]
            public static extern IntPtr WinGlobalLock(IntPtr handle);

            [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalUnlock")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool WinGlobalUnlock(IntPtr handle);
        }
    }
}
