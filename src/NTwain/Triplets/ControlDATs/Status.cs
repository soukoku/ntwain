using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.STATUS"/>.
  /// </summary>
  public class Status
  {
    public TWRC GetForDSM(ref TW_IDENTITY_LEGACY app, out TW_STATUS status)
    {
      status = default;
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      return rc;
    }

    public TWRC GetForDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_STATUS status)
    {
      status = default;
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX osxds = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref osxds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref osxds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      return rc;
    }
  }
}
