using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and custom capability DAT for certain devices.
  /// </summary>
  public class CapabilityCustom
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GET, ref data);
    public TWRC GetCurrent(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETCURRENT, ref data);
    public TWRC GetDefault(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETDEFAULT, ref data);
    public TWRC GetHelp(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETHELP, ref data);
    public TWRC GetLabel(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETLABEL, ref data);
    public TWRC GetLabelEnum(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.GETLABELENUM, ref data);
    public TWRC QuerySupport(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.QUERYSUPPORT, ref data);
    public TWRC Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.RESET, ref data);
    public TWRC ResetAll(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.RESETALL, ref data);
    public TWRC Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort customDAT, ref TW_CAPABILITY data)
      => DoIt(ref app, ref ds, customDAT, MSG.SET, ref data);

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ushort dat, MSG msg, ref TW_CAPABILITY data)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, (DAT)dat, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, (DAT)dat, msg, ref data);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, (DAT)dat, msg, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, (DAT)dat, msg, ref data);
        }
      }
      return rc;
    }
  }
}
