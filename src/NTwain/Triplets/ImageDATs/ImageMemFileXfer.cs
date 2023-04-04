using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.IMAGEMEMFILEXFER"/>.
  /// </summary>
  public class ImageMemFileXfer
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_IMAGEMEMXFER data)
      => DoIt(ref app, ref ds, MSG.GET, out data);

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, out TW_IMAGEMEMXFER data)
    {
      data = default;
      var rc = TWRC.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.IMAGEMEMFILEXFER, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.IMAGEMEMFILEXFER, msg, ref data);
        }
      }
      //else if (TwainPlatform.IsMacOSX)
      //{
      //  TW_IDENTITY_MACOSX app2 = app;
      //  TW_IDENTITY_MACOSX ds2 = ds;
      //  if (TwainPlatform.PreferLegacyDSM)
      //  {
      //    rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.IMAGEMEMXFER, msg, ref data);
      //  }
      //  else
      //  {
      //    rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.IMAGEMEMXFER, msg, ref data);
      //  }
      //}
      return rc;
    }
  }
}
