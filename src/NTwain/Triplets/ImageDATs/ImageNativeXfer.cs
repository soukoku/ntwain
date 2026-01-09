using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ImageDATs;

/// <summary>
/// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.IMAGENATIVEXFER"/>.
/// </summary>
public class ImageNativeXfer
{
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, out IntPtr data)
      => DoIt(app, ds, MSG.GET, out data);

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, out IntPtr data)
    {
        var rc = TWRC.FAILURE;
        data = IntPtr.Zero;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
            }
        }
        return rc;
    }
}
