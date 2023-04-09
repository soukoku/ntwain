using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.GRAYRESPONSE"/>.
  /// </summary>
  public class GrayResponse
  {
    public TWRC Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_GRAYRESPONSE data)
      => DoIt(ref app, ref ds, MSG.SET, ref data);
    public TWRC Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_GRAYRESPONSE data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.RESET, ref data);
    }

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_GRAYRESPONSE data)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.GRAYRESPONSE, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.GRAYRESPONSE, msg, ref data);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.GRAYRESPONSE, msg, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.GRAYRESPONSE, msg, ref data);
        }
      }
      return rc;
    }
  }
}
