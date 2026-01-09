using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.CAPABILITY"/>.
/// </summary>
public class Capability
{
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.GET, ref data);
    public TWRC GetCurrent(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.GETCURRENT, ref data);
    public TWRC GetDefault(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.GETDEFAULT, ref data);
    public TWRC GetHelp(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.GETHELP, ref data);
    public TWRC GetLabel(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.GETLABEL, ref data);
    public TWRC GetLabelEnum(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.GETLABELENUM, ref data);
    public TWRC QuerySupport(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.QUERYSUPPORT, ref data);
    public TWRC Reset(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.RESET, ref data);
    public TWRC ResetAll(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.RESETALL, ref data);
    public TWRC Set(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.SET, ref data);
    public TWRC SetConstraint(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_CAPABILITY data)
      => DoIt(app, ds, MSG.SETCONSTRAINT, ref data);

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, ref TW_CAPABILITY data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
            }
        }
        return rc;
    }
}
