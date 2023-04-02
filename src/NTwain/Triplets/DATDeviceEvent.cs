using TWAINWorkingGroup;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.DEVICEEVENT"/>.
  /// </summary>
  public class DATDeviceEvent : TripletBase
  {
    public DATDeviceEvent(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Gets the device event detail.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS Get(out TW_DEVICEEVENT data)
    {
      data = default;
      return DoIt(MSG.GET, ref data);
    }

    STS DoIt(MSG msg, ref TW_DEVICEEVENT data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        var ds = Session.CurrentSource;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryDeviceevent(ref app, ref ds, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryDeviceevent(ref app, ref ds, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX ds = Session.CurrentSource;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryDeviceevent(ref app, ref ds, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryDeviceevent(ref app, ref ds, DG.CONTROL, DAT.DEVICEEVENT, msg, ref data);
        }
      }
      return rc;
    }
  }
}
