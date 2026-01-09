using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.USERINTERFACE"/>.
/// </summary>
public class UserInterface
{
    public TWRC DisableDS(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_USERINTERFACE data)
      => DoIt(app, ds, MSG.DISABLEDS, ref data);

    public TWRC EnableDS(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_USERINTERFACE data)
      => DoIt(app, ds, MSG.ENABLEDS, ref data);

    public TWRC EnableDSUIOnly(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_USERINTERFACE data)
      => DoIt(app, ds, MSG.ENABLEDSUIONLY, ref data);

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, ref TW_USERINTERFACE data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
            }
        }
        return rc;
    }
}
