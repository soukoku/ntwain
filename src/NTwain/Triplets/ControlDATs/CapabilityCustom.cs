using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and custom capability DAT for certain devices.
  /// </summary>
  public class CapabilityCustom
  {
    public STS Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GET, ref data);
    public STS GetCurrent(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETCURRENT, ref data);
    public STS GetDefault(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETDEFAULT, ref data);
    public STS GetHelp(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETHELP, ref data);
    public STS GetLabel(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETLABEL, ref data);
    public STS GetLabelEnum(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETLABELENUM, ref data);
    public STS QuerySupport(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.QUERYSUPPORT, ref data);
    public STS Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.RESET, ref data);
    public STS ResetAll(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.RESETALL, ref data);
    public STS Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.SET, ref data);

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort dat, MSG msg, ref TW_CAPABILITY data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, (DAT)dat, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, (DAT)dat, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, (DAT)dat, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, (DAT)dat, msg, ref data);
        }
      }
      return rc;
    }
  }
}
