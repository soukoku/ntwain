using NTwain.DSM;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.DEVICEEVENT"/>.
  /// </summary>
  public class DeviceEvent
  {
    /// <summary>
    /// Gets the device event detail.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_DEVICEEVENT data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.GET, ref data);
    }

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_DEVICEEVENT data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
      }
      return rc;
    }
  }
}
