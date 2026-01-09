using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.STATUS"/>.
/// </summary>
public class Status
{
    public TWRC GetForDSM(TWIdentityWrapper app, out TW_STATUS status)
    {
        status = default;
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
        }
        return rc;
    }

    public TWRC GetForDS(TWIdentityWrapper app, TWIdentityWrapper ds, out TW_STATUS status)
    {
        status = default;
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
            }
        }
        return rc;
    }
}
