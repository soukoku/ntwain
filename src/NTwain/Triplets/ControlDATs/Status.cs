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
    /// <summary>
    /// Gets the current status for the DSM.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public STS GetForDSM(ref TW_IDENTITY_LEGACY app, out TW_STATUS status)
    {
      status = default;
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      return rc;
    }

    /// <summary>
    /// Gets the status for the current source.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public STS GetForDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out TW_STATUS status)
    {
      status = default;
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX osxds = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref osxds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref osxds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      return rc;
    }
  }
}
