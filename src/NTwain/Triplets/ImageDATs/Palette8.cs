using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.PALETTE8"/>.
  /// </summary>
  public class Palette8
  {
    public STS Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_PALETTE8 data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.GET, ref data);
    }
    public STS GetDefault(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_PALETTE8 data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.GETDEFAULT, ref data);
    }
    public STS Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_PALETTE8 data)
      => DoIt(ref app, ref ds, MSG.SET, ref data);
    public STS Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_PALETTE8 data)
    {
      data = default;
      return DoIt(ref app, ref ds, MSG.RESET, ref data);
    }

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_PALETTE8 data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.PALETTE8, msg, ref data);
        }
      }
      return rc;
    }
  }
}
