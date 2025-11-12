using NTwain.Data;
using System;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;

namespace NTwain
{
    // this file contains memory methods

    partial class TwainAppSession : IMemoryManager
    {
        TW_ENTRYPOINT_DELEGATES _entryPoint;

        public IntPtr Alloc(uint size)
        {
            if (_entryPoint.DSM_MemAllocate != null)
            {
                return _entryPoint.DSM_MemAllocate(size);
            }
            else if (TWPlatform.IsWindows)
            {
                return PInvoke.GlobalAlloc(GLOBAL_ALLOC_FLAGS.GHND, size);
                //return WinNativeMethods.GlobalAlloc(WinNativeMethods.AllocFlag.GHND, (UIntPtr)size);
            }
            else if (TWPlatform.IsLinux)
            {
                return Marshal.AllocHGlobal((int)size);
            }
            else if (TWPlatform.IsMacOSX)
            {
                return Marshal.AllocHGlobal((int)size);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public void Free(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;

            if (_entryPoint.DSM_MemFree != null)
            {
                _entryPoint.DSM_MemFree(handle);
            }
            else if (TWPlatform.IsWindows)
            {
                PInvoke.GlobalFree((HGLOBAL)handle);
                //WinNativeMethods.GlobalFree(handle);
            }
            else if (TWPlatform.IsLinux)
            {
                Marshal.FreeHGlobal(handle);
            }
            else if (TWPlatform.IsMacOSX)
            {
                Marshal.FreeHGlobal(handle);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public IntPtr Lock(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return IntPtr.Zero;

            if (_entryPoint.DSM_MemLock != null)
            {
                return _entryPoint.DSM_MemLock(handle);
            }
            else if (TWPlatform.IsWindows)
            {
                unsafe
                {
                    return (IntPtr)PInvoke.GlobalLock((HGLOBAL)handle);
                }
                //return WinNativeMethods.GlobalLock(handle);
            }
            else if (TWPlatform.IsLinux)
            {
                return handle;
            }
            else if (TWPlatform.IsMacOSX)
            {
                return handle;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        public void Unlock(IntPtr handle)
        {
            if (handle == IntPtr.Zero) return;

            if (_entryPoint.DSM_MemUnlock != null)
            {
                _entryPoint.DSM_MemUnlock(handle);
            }
            else if (TWPlatform.IsWindows)
            {
                PInvoke.GlobalUnlock((HGLOBAL)handle);
                //WinNativeMethods.GlobalUnlock(handle);
            }
            else if (TWPlatform.IsLinux)
            {
            }
            else if (TWPlatform.IsMacOSX)
            {
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}
