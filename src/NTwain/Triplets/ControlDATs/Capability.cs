using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.CAPABILITY"/>.
  /// </summary>
  public class Capability
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GET, ref data);
    public TWRC GetCurrent(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETCURRENT, ref data);
    public TWRC GetDefault(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETDEFAULT, ref data);
    public TWRC GetHelp(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETHELP, ref data);
    public TWRC GetLabel(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETLABEL, ref data);
    public TWRC GetLabelEnum(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.GETLABELENUM, ref data);
    public TWRC QuerySupport(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.QUERYSUPPORT, ref data);
    public TWRC Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.RESET, ref data);
    public TWRC ResetAll(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.RESETALL, ref data);
    public TWRC Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.SET, ref data);
    public TWRC SetConstraint(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, MSG.SETCONSTRAINT, ref data);

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_CAPABILITY data)
    {
      var rc = TWRC.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.CAPABILITY, msg, ref data);
        }
      }
      return rc;
    }
  }
}
