using NTwain.Data;
using NTwain.Internals;
using System;
using System.ComponentModel;

namespace NTwain
{
    /// <summary>
    /// Provides methods for managing memory on data exchanged with twain sources using old win32 methods.
    /// This should only be used after the DSM has been opened.
    /// </summary>
    class WinMemoryManager : IMemoryManager
    {
        public IntPtr Allocate(uint size)
        {
            IntPtr retVal = NativeMethods.WinGlobalAlloc(0x0040, new UIntPtr(size));

            if (retVal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            return retVal;
        }

        public void Free(IntPtr handle)
        {
            NativeMethods.WinGlobalFree(handle);
        }

        public IntPtr Lock(IntPtr handle)
        {
            return NativeMethods.WinGlobalLock(handle);
        }

        public void Unlock(IntPtr handle)
        {
            NativeMethods.WinGlobalUnlock(handle);
        }
    }
}
