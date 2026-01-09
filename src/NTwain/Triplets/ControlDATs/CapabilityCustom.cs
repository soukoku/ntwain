using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and custom capability DAT for certain devices.
/// </summary>
public class CapabilityCustom
{
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.GET, ref data);
    public TWRC GetCurrent(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.GETCURRENT, ref data);
    public TWRC GetDefault(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.GETDEFAULT, ref data);
    public TWRC GetHelp(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.GETHELP, ref data);
    public TWRC GetLabel(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.GETLABEL, ref data);
    public TWRC GetLabelEnum(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.GETLABELENUM, ref data);
    public TWRC QuerySupport(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.QUERYSUPPORT, ref data);
    public TWRC Reset(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.RESET, ref data);
    public TWRC ResetAll(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.RESETALL, ref data);
    public TWRC Set(TWIdentityWrapper app, TWIdentityWrapper ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(app, ds, customDAT, MSG.SET, ref data);

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, ushort dat, MSG msg, ref TW_CAPABILITY data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, (DAT)dat, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, (DAT)dat, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, (DAT)dat, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, (DAT)dat, msg, ref data);
            }
        }
        return rc;
    }
}
