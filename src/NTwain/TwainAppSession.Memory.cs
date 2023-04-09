using NTwain.Data;
using NTwain.Native;
using System;
using System.Runtime.InteropServices;

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
        return NativeMemoryMethods.WinGlobalAlloc(NativeMemoryMethods.AllocFlag.GHND, (UIntPtr)size);
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
        NativeMemoryMethods.WinGlobalFree(handle);
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
        return NativeMemoryMethods.WinGlobalLock(handle);
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
        NativeMemoryMethods.WinGlobalUnlock(handle);
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
