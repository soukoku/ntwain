using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.IMAGEFILEXFER"/>.
  /// </summary>
  public class ImageFileXfer
  {
    public STS Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, ref ds, MSG.GET);

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.IMAGEFILEXFER, msg, IntPtr.Zero);
        }
      }
      return rc;
    }
  }
}
