using System;
using System.Text;
using TWAINWorkingGroup;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.STATUSUTF8"/>.
  /// </summary>
  public class DATStatusUtf8 : TripletBase
  {
    public DATStatusUtf8(TwainSession session) : base(session)
    {

    }

    /// <summary>
    /// Gets the extended text info for a previously received <see cref="TW_STATUS"/>.
    /// If this is called you should try to extract the string value from it once
    /// with <see cref="TW_STATUSUTF8.ReadAndFree"/> or call <see cref="TW_STATUSUTF8.Free"/>
    /// so there's no memory leak.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="extendedStatus"></param>
    /// <returns></returns>
    public STS Get(TW_STATUS status, out TW_STATUSUTF8 extendedStatus)
    {
      extendedStatus = new TW_STATUSUTF8 { Status = status };
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
        var ds = Session.CurrentSource;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryStatusutf8(ref app, ref ds, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatusutf8(ref app, ref ds, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
        TW_IDENTITY_MACOSX ds = Session.CurrentSource;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryStatusutf8(ref app, ref ds, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryStatusutf8(ref app, ref ds, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
      }
      return rc;
    }

  }
}
