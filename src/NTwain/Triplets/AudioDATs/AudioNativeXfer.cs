using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.AudioDATs;

/// <summary>
/// Contains calls used with <see cref="DG.AUDIO"/> and <see cref="DAT.AUDIONATIVEXFER"/>.
/// </summary>
public class AudioNativeXfer
{
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, out IntPtr data)
      => DoIt(app, ds, MSG.GET, out data);

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, out IntPtr data)
    {
        data = default;
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.AUDIO, DAT.AUDIONATIVEXFER, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.AUDIO, DAT.AUDIONATIVEXFER, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.AUDIO, DAT.AUDIONATIVEXFER, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.AUDIO, DAT.AUDIONATIVEXFER, msg, ref data);
            }
        }
        return rc;
    }
}
