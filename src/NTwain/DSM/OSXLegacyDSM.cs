using System;
using System.Runtime.InteropServices;
using TWAINWorkingGroup;

namespace NTwain.DSM
{
  static class OSXLegacyDSM
  {
    const string DsmName = "/System/Library/Frameworks/framework/TWAIN";

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref IntPtr hwnd
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_IDENTITY_MACOSX twidentity
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUS twstatus
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUS twstatus
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUSUTF8 twstatusutf8
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_ENTRYPOINT twentrypoint
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, ref TW_DEVICEEVENT twdeviceevent
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, ref TW_CUSTOMDSDATA twcustomedsdata
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, ref TW_CALLBACK twcallback
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, ref TW_CALLBACK2 twcallback
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    internal static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, ref DG xfergroup
    );
  }
}
