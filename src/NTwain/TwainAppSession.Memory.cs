﻿using NTwain.Data;
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
      else if (TwainPlatform.IsWindows)
      {
        return NativeMemoryMethods.WinGlobalAlloc(NativeMemoryMethods.AllocFlag.GHND, (UIntPtr)size);
      }
      else if (TwainPlatform.IsLinux)
      {
        return Marshal.AllocHGlobal((int)size);
      }
      else if (TwainPlatform.IsMacOSX)
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
      else if (TwainPlatform.IsWindows)
      {
        NativeMemoryMethods.WinGlobalFree(handle);
      }
      else if (TwainPlatform.IsLinux)
      {
        Marshal.FreeHGlobal(handle);
      }
      else if (TwainPlatform.IsMacOSX)
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
      else if (TwainPlatform.IsWindows)
      {
        return NativeMemoryMethods.WinGlobalLock(handle);
      }
      else if (TwainPlatform.IsLinux)
      {
        return handle;
      }
      else if (TwainPlatform.IsMacOSX)
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
      else if (TwainPlatform.IsWindows)
      {
        NativeMemoryMethods.WinGlobalUnlock(handle);
      }
      else if (TwainPlatform.IsLinux)
      {
      }
      else if (TwainPlatform.IsMacOSX)
      {
      }
      else
      {
        throw new PlatformNotSupportedException();
      }
    }
  }
}
