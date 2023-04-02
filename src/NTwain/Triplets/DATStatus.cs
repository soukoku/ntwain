using System;
using TWAINWorkingGroup;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.STATUS"/>.
  /// </summary>
  public class DATStatus : TripletBase
  {
    public DATStatus(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Gets the current status for the DSM.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public STS GetForDSM(out TW_STATUS status)
    {
      status = default;
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryStatusState3(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatusState3(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryStatusState3(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryStatusState3(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      return rc;
    }

    /// <summary>
    /// Gets the status for the current source.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public STS GetForDS(out TW_STATUS status)
    {
      status = default;
      var ds = Session.CurrentSource;
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryStatus(ref app, ref ds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatus(ref app, ref ds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX osxds = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryStatus(ref app, ref osxds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryStatus(ref app, ref osxds, DG.CONTROL, DAT.STATUS, MSG.GET, ref status);
        }
      }
      return rc;
    }
  }
}
