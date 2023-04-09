﻿using NTwain.Data;
using NTwain.DSM;
using System;
using System.Runtime.InteropServices;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.ENTRYPOINT"/>.
  /// </summary>
  public class EntryPoint
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, out TW_ENTRYPOINT_DELEGATES entry)
    {
      entry = default;
      TW_ENTRYPOINT rawentry = default;
      var rc = DoIt(ref app, MSG.GET, ref rawentry);
      if (rc == TWRC.SUCCESS)
      {
        entry.Size = rawentry.Size;
        entry.DSM_Entry = rawentry.DSM_Entry;
        if (rawentry.DSM_MemAllocate != IntPtr.Zero)
        {
          entry.DSM_MemAllocate = (DSM_MEMALLOC)Marshal.GetDelegateForFunctionPointer(rawentry.DSM_MemAllocate, typeof(DSM_MEMALLOC));
        }
        if (rawentry.DSM_MemFree != IntPtr.Zero)
        {
          entry.DSM_MemFree = (DSM_MEMFREE)Marshal.GetDelegateForFunctionPointer(rawentry.DSM_MemFree, typeof(DSM_MEMFREE));
        }
        if (rawentry.DSM_MemLock != IntPtr.Zero)
        {
          entry.DSM_MemLock = (DSM_MEMLOCK)Marshal.GetDelegateForFunctionPointer(rawentry.DSM_MemLock, typeof(DSM_MEMLOCK));
        }
        if (rawentry.DSM_MemUnlock != IntPtr.Zero)
        {
          entry.DSM_MemUnlock = (DSM_MEMUNLOCK)Marshal.GetDelegateForFunctionPointer(rawentry.DSM_MemUnlock, typeof(DSM_MEMUNLOCK));
        }
      }
      return rc;
    }

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, MSG msg, ref TW_ENTRYPOINT entry)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
      }
      //else if (TwainPlatform.IsLinux)
      //{
      //  var app = Session._appIdentity;
      //}
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
      }
      return rc;
    }
  }
}
