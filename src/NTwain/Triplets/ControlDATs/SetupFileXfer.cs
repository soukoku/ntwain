using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.SETUPFILEXFER"/>.
  /// </summary>
  public class SetupFileXfer
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_SETUPFILEXFER data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.GET, ref data);
    }
    public TWRC Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_SETUPFILEXFER data)
      => DoIt(ref app, ref ds, MSG.SET, ref data);
    public TWRC GetDefault(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_SETUPFILEXFER data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.GETDEFAULT, ref data);
    }
    public TWRC Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_SETUPFILEXFER data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.RESET, ref data);
    }


    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_SETUPFILEXFER data)
    {
      var rc = TWRC.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.SETUPFILEXFER, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.SETUPFILEXFER, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.SETUPFILEXFER, msg, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.SETUPFILEXFER, msg, ref data);
        }
      }
      return rc;
    }
  }
}
