using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PARENT"/>.
/// </summary>
public class Parent
{
    /// <summary>
    /// Loads and opens the DSM.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <returns></returns>
    public TWRC OpenDSM(TWIdentityWrapper app, IntPtr hwnd)
      => DoIt(app, MSG.OPENDSM, hwnd);

    /// <summary>
    /// Closes the DSM.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <returns></returns>
    public TWRC CloseDSM(TWIdentityWrapper app, IntPtr hwnd)
      => DoIt(app, MSG.CLOSEDSM, hwnd);

    static TWRC DoIt(TWIdentityWrapper app, MSG msg, IntPtr hwnd)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
            }
        }
        //else if (TwainPlatform.IsLinux)
        //{
        //  var app = Session._appIdentity;
        //  rc = NativeMethods.LinuxDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        //  if (rc == TWRC.SUCCESS) Session._appIdentity = app;
        //}
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, hwnd);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, hwnd);
            }
        }
        return rc;
    }
}
