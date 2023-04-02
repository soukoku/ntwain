using NTwain.Data;
using System;
using System.Runtime.InteropServices;

namespace NTwain.DSM
{
  /// <summary>
  /// Low-level pinvoke methods using /usr/local/lib64/libtwaindsm.so.
  /// </summary>
  public static class Linux64DSM
  {
    const string DsmName = "/usr/local/lib64/libtwaindsm.so";

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref IntPtr hwnd
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, IntPtr zero
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, IntPtr zero
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref IntPtr mem
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_IDENTITY_LEGACY twidentity
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUS twstatus
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUS twstatus
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUSUTF8 twstatusutf8
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_ENTRYPOINT twentrypoint
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_DEVICEEVENT twdeviceevent
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CUSTOMDSDATA twcustomedsdata
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CALLBACK twcallback
    );
    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CALLBACK2 twcallback
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref DG xfergroup
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_USERINTERFACE userinterface
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_EVENT evt
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_PENDINGXFERS pendingxfers
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_SETUPMEMXFER memxfer
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_SETUPFILEXFER filexfer
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_PASSTHRU passthru
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_FILESYSTEM filesystem
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CAPABILITY cap
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_AUDIOINFO auioinfo
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_IMAGEMEMXFER memxfer
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CIECOLOR ciecolor
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_EXTIMAGEINFO imginfo
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_FILTER filter
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_GRAYRESPONSE resp
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_MEMORY mem
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_IMAGEINFO info
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_IMAGELAYOUT layout
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_JPEGCOMPRESSION compression
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_PALETTE8 palette
    );

    [DllImport(DsmName, CharSet = CharSet.Ansi)]
    public static extern ushort DSM_Entry
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_RGBRESPONSE resp
    );
  }
}
