using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.EXTIMAGEINFO"/>.
  /// </summary>
  public class ExtImageInfo
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_EXTIMAGEINFO data)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.EXTIMAGEINFO, MSG.GET, ref data);
        }
      }
      return rc;
    }
  }
}
