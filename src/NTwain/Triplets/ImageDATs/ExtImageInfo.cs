using NTwain.Data;
using NTwain.Data.Kds;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs;

/// <summary>
/// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.EXTIMAGEINFO"/>.
/// </summary>
public class ExtImageInfo
{
    /// <summary>
    /// For use with all scanners in state 7.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_EXTIMAGEINFO data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
            }
        }
        return rc;
    }

    /// <summary>
    /// For use with KODAK scanners in state 6.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public TWRC GetSpecial(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_EXTIMAGEINFO data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.EXTIMAGEINFO, (MSG)KDS_MSG.GETSPECIAL, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.EXTIMAGEINFO, (MSG)KDS_MSG.GETSPECIAL, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.EXTIMAGEINFO, (MSG)KDS_MSG.GETSPECIAL, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.EXTIMAGEINFO, (MSG)KDS_MSG.GETSPECIAL, ref data);
            }
        }
        return rc;
    }


}
