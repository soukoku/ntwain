using NTwain.Data;
using NTwain.DSM;
using System.Runtime.InteropServices;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.METRICS"/>.
  /// </summary>
  public class Metrics
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_METRICS data)
    {
      data = default;
      data.SizeOf = (uint)Marshal.SizeOf<TW_METRICS>();

      var rc = TWRC.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.METRICS, MSG.GET, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.METRICS, MSG.GET, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.METRICS, MSG.GET, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.METRICS, MSG.GET, ref data);
        }
      }
      return rc;
    }
  }
}
