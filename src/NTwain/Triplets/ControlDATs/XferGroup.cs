using NTwain.DSM;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.XFERGROUP"/>.
  /// </summary>
  public class XferGroup : TripletBase
  {
    public XferGroup(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Gets the transfer group used.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS Get(out DG data)
    {
      data = default;
      return DoIt(MSG.GET, ref data);
    }

    /// <summary>
    /// Sets the transfer group to be used.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS Set(DG data)
    {
      return DoIt(MSG.SET, ref data);
    }

    STS DoIt(MSG msg, ref DG data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        var ds = Session.CurrentSource;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.XFERGROUP, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.XFERGROUP, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX ds = Session.CurrentSource;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.XFERGROUP, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.XFERGROUP, msg, ref data);
        }
      }
      return rc;
    }
  }
}
