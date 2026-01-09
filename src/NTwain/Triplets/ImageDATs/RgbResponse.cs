using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs;

/// <summary>
/// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.RGBRESPONSE"/>.
/// </summary>
public class RgbResponse
{
    public TWRC Set(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_RGBRESPONSE data)
      => DoIt(app, ds, MSG.SET, ref data);
    public TWRC Reset(TWIdentityWrapper app, TWIdentityWrapper ds, out TW_RGBRESPONSE data)
    {
        data = default;
        return DoIt(app, ds, MSG.RESET, ref data);
    }

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, ref TW_RGBRESPONSE data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.RGBRESPONSE, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.RGBRESPONSE, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.RGBRESPONSE, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.RGBRESPONSE, msg, ref data);
            }
        }
        return rc;
    }
}
