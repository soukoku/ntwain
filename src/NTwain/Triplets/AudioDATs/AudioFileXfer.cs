using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.AudioDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.AUDIO"/> and <see cref="DAT.AUDIOFILEXFER"/>.
  /// </summary>
  public class AudioFileXfer
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, ref ds, MSG.GET);

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.AUDIO, DAT.AUDIOFILEXFER, msg, IntPtr.Zero);
        }
      }
      return rc;
    }
  }
}
