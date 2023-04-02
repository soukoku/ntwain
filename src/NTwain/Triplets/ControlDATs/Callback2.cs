using NTwain.DSM;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.CALLBACK2"/>.
  /// </summary>
  public class Callback2 : TripletBase
  {
    public Callback2(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Registers the callback function.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS RegisterCallback(ref TW_CALLBACK2 data)
    {
      return DoIt(MSG.REGISTER_CALLBACK, ref data);
    }

    STS DoIt(MSG msg, ref TW_CALLBACK2 data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        var ds = Session.CurrentSource;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CALLBACK2, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CALLBACK2, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX ds = Session.CurrentSource;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CALLBACK2, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CALLBACK2, msg, ref data);
        }
      }
      return rc;
    }
  }
}
