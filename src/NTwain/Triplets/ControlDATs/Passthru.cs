using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PASSTHRU"/>.
/// </summary>
public class Passthru
{
    public TWRC PassThrough(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_PASSTHRU data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
            }
        }
        return rc;
    }
}
