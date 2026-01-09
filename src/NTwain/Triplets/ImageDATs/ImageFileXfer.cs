using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ImageDATs;

/// <summary>
/// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.IMAGEFILEXFER"/>.
/// </summary>
public class ImageFileXfer
{
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds)
      => DoIt(app, ds, MSG.GET);

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
            }
        }
        return rc;
    }
}
