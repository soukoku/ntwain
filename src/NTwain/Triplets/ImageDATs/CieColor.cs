using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.CIECOLOR"/>.
  /// </summary>
  public class CieColor
  {
    public STS Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_CIECOLOR data)
      => DoIt(ref app, ref ds, MSG.GET, out data);

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, out TW_CIECOLOR data)
    {
      var rc = STS.FAILURE;
      data = default;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.CIECOLOR, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.CIECOLOR, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.CIECOLOR, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.CIECOLOR, msg, ref data);
        }
      }
      return rc;
    }
  }
}
