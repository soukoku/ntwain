using NTwain.Data;
using System;
using System.Runtime.InteropServices;

namespace NTwain.DSM
{
  /// <summary>
  /// Low-level pinvoke methods using twain_32.dll. For 32bit only.
  /// </summary>
  public static partial class WinLegacyDSM
  {
    const string DsmName = "twain_32.dll";

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref IntPtr hwnd
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, IntPtr zero
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, IntPtr zero
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref IntPtr mem
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_IDENTITY_LEGACY twidentity
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUS twstatus
    );
#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUS twstatus
    );
#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_STATUSUTF8 twstatusutf8
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, IntPtr dest,
        DG dg, DAT dat, MSG msg, ref TW_ENTRYPOINT twentrypoint
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_DEVICEEVENT twdeviceevent
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CUSTOMDSDATA twcustomedsdata
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CALLBACK twcallback
    );
#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CALLBACK2 twcallback
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref DG xfergroup
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_USERINTERFACE userinterface
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_EVENT evt
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_TWAINDIRECT task
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_PENDINGXFERS pendingxfers
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_SETUPMEMXFER memxfer
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_SETUPFILEXFER filexfer
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_METRICS metrics
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_PASSTHRU passthru
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_FILESYSTEM filesystem
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CAPABILITY cap
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_AUDIOINFO auioinfo
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_IMAGEMEMXFER memxfer
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_CIECOLOR ciecolor
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_EXTIMAGEINFO imginfo
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_FILTER filter
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_GRAYRESPONSE resp
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_MEMORY mem
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_IMAGEINFO info
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_IMAGELAYOUT layout
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_JPEGCOMPRESSION compression
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_PALETTE8 palette
    );

#if NET7_0_OR_GREATER
    [LibraryImport(DsmName)] public static partial TWRC DSM_Entry
#else
    [DllImport(DsmName)] public static extern TWRC DSM_Entry
#endif
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, ref TW_RGBRESPONSE resp
    );
  }
}
