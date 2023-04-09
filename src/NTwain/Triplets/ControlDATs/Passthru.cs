using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PASSTHRU"/>.
  /// </summary>
  public class Passthru
  {
    public TWRC PassThrough(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_PASSTHRU data)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.PASSTHRU, MSG.PASSTHRU, ref data);
        }
      }
      return rc;
    }
  }
}
