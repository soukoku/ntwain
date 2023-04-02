using NTwain.DSM;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.USERINTERFACE"/>.
  /// </summary>
  public class UserInterface : TripletBase
  {
    public UserInterface(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Disables source to bring state down to <see cref="STATE.S4"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS DisableDS(ref TW_USERINTERFACE data)
    {
      var rc = DoIt(MSG.DISABLEDS, ref data);
      if (rc == STS.SUCCESS)
      {
        Session.State = STATE.S4;
      }
      return rc;
    }

    /// <summary>
    /// Enables source to bring state up to <see cref="STATE.S5"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS EnableDS(ref TW_USERINTERFACE data)
    {
      var rc = DoIt(MSG.ENABLEDS, ref data);
      if (rc == STS.SUCCESS || (data.ShowUI == 0 && rc == STS.CHECKSTATUS))
      {
        Session.State = STATE.S5;
      }
      return rc;
    }

    /// <summary>
    /// Enables source to bring state up to <see cref="STATE.S5"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS EnableDSUIOnly(ref TW_USERINTERFACE data)
    {
      var rc = DoIt(MSG.ENABLEDSUIONLY, ref data);
      if (rc == STS.SUCCESS)
      {
        Session.State = STATE.S5;
      }
      return rc;
    }

    STS DoIt(MSG msg, ref TW_USERINTERFACE data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        var ds = Session.CurrentSource;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX ds = Session.CurrentSource;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
      }
      return rc;
    }
  }
}
