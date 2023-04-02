using NTwain.DSM;
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
    /// Loads the custom DS data.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS Get(out TW_CUSTOMDSDATA data)
    {
      data = default;
      return DoIt(MSG.GET, ref data);
    }

    /// <summary>
    /// Sets the custom DS data.
    /// </summary>
    /// <param name="data"></param>
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
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX ds = Session.CurrentSource;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CUSTOMDSDATA, msg, ref data);
        }
      }
      return rc;
    }
  }
}
