using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs;

/// <summary>
/// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.PALETTE8"/>.
/// </summary>
public class Palette8
{
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, out TW_PALETTE8 data)
    {
        data = default;
        return DoIt(app, ds, MSG.GET, ref data);
    }
    public TWRC GetDefault(TWIdentityWrapper app, TWIdentityWrapper ds, out TW_PALETTE8 data)
    {
        data = default;
        return DoIt(app, ds, MSG.GETDEFAULT, ref data);
    }
    public TWRC Set(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_PALETTE8 data)
      => DoIt(app, ds, MSG.SET, ref data);
    public TWRC Reset(TWIdentityWrapper app, TWIdentityWrapper ds, out TW_PALETTE8 data)
    {
        data = default;
        return DoIt(app, ds, MSG.RESET, ref data);
    }

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, ref TW_PALETTE8 data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.PALETTE8, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.PALETTE8, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.PALETTE8, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.PALETTE8, msg, ref data);
            }
        }
        return rc;
    }
}
