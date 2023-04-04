using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.CAPABILITY"/>.
  /// </summary>
  public class Capability
  {
    public STS Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GET, ref data);
    public STS GetCurrent(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETCURRENT, ref data);
    public STS GetDefault(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETDEFAULT, ref data);
    public STS GetHelp(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETHELP, ref data);
    public STS GetLabel(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETLABEL, ref data);
    public STS GetLabelEnum(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETLABELENUM, ref data);
    public STS QuerySupport(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.QUERYSUPPORT, ref data);
    public STS Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.RESET, ref data);
    public STS ResetAll(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.RESETALL, ref data);
    public STS Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.SET, ref data);
    public STS SetConstraint(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.SETCONSTRAINT, ref data);

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_CAPABILITY data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
      }
      return rc;
    }
  }
}
