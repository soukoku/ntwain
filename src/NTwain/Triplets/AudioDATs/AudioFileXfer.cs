using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.AudioDATs;

/// <summary>
/// Contains calls used with <see cref="DG.AUDIO"/> and <see cref="DAT.AUDIOFILEXFER"/>.
/// </summary>
public class AudioFileXfer
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
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
            }
        }
        return rc;
    }
}
