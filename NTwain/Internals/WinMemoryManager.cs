using NTwain.Data;
using NTwain.Internals;
using System;
using System.ComponentModel;

namespace NTwain
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
    }
}
