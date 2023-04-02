using TWAINWorkingGroup;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.CUSTOMDSDATA"/>.
  /// </summary>
  public class DATCustomDsData : TripletBase
  {
    public DATCustomDsData(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Loads and opens the specified data source.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS Get(ref TW_CUSTOMDSDATA data)
    {
      return DoIt(MSG.GET, ref data);
    }

    /// <summary>
    /// Sets the customs data.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS Set(ref TW_CUSTOMDSDATA data)
    {
      return DoIt(MSG.SET, ref data);
    }

    STS DoIt(MSG msg, ref TW_CUSTOMDSDATA data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        var ds = Session.CurrentSource;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryCustomdsdata(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryCustomdsdata(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX ds = Session.CurrentSource;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryCustomdsdata(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryCustomdsdata(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
      }
      return rc;
    }
  }
}
