using NTwain.DSM;
using System;
using System.Runtime.InteropServices;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.ENTRYPOINT"/>.
  /// </summary>
  public class EntryPoint
  {
    /// <summary>
    /// Loads and opens the DSM.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    public STS Get(ref TW_IDENTITY_LEGACY app, out TW_ENTRYPOINT_DELEGATES entry)
    {
      entry = default;
      TW_ENTRYPOINT rawentry = default;
      var rc = DoIt(ref app, MSG.GET, ref rawentry);
      if (rc == STS.SUCCESS)
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

    static STS DoIt(ref TW_IDENTITY_LEGACY app, MSG msg, ref TW_ENTRYPOINT entry)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
      }
      //else if (TwainPlatform.IsLinux)
      //{
      //  var app = Session._appIdentity;
      //}
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.ENTRYPOINT, msg, ref entry);
        }
      }
      return rc;
    }
  }
}
