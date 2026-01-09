using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs;

/// <summary>
/// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.IMAGEMEMXFER"/>.
/// </summary>
public class ImageMemXfer
{
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_IMAGEMEMXFER data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.IMAGEMEMXFER, MSG.GET, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.IMAGEMEMXFER, MSG.GET, ref data);
            }
        }
        return rc;
    }

    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_IMAGEMEMXFER_MACOSX data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.IMAGEMEMXFER, MSG.GET, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.IMAGEMEMXFER, MSG.GET, ref data);
            }
        }
        return rc;
    }
}
