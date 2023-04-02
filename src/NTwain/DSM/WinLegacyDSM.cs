using System;
using System.Runtime.InteropServices;
using TWAINWorkingGroup;

namespace NTwain.DSM
{
  static class WinLegacyDSM
  {
    const string DsmName = "twain_32.dll";

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        IntPtr dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref IntPtr hwnd
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        IntPtr dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref TW_IDENTITY_LEGACY twidentity
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        ref TW_IDENTITY_LEGACY dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref TW_STATUS twstatus
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        IntPtr dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref TW_STATUS twstatus
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        IntPtr dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref TW_STATUSUTF8 twstatusutf8
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        IntPtr dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref TW_ENTRYPOINT twentrypoint
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        ref TW_IDENTITY_LEGACY dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref TW_DEVICEEVENT twdeviceevent
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern UInt16 DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin,
        ref TW_IDENTITY_LEGACY dest,
        DG dg,
        DAT dat,
        MSG msg,
        ref TW_CUSTOMDSDATA twcustomedsdata
    );
  }
}
