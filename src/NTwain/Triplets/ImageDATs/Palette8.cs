using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.PALETTE8"/>.
  /// </summary>
  public class Palette8
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_PALETTE8 data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.GET, ref data);
    }
    public TWRC GetDefault(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_PALETTE8 data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.GETDEFAULT, ref data);
    }
    public TWRC Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_PALETTE8 data)
      => DoIt(ref app, ref ds, MSG.SET, ref data);
    public TWRC Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_PALETTE8 data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.RESET, ref data);
    }

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_PALETTE8 data)
    {
      var rc = TWRC.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
      }
      return rc;
    }
  }
}
