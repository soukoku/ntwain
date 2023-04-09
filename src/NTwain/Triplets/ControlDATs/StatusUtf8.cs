using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.STATUSUTF8"/>.
  /// </summary>
  public class StatusUtf8
  {
    /// <summary>
    /// Gets the extended text info for a previously received <see cref="TW_STATUS"/>.
    /// If this is called you should try to extract the string value from it
    /// with <see cref="TW_STATUSUTF8.Read(IMemoryManager, bool)"/> or call <see cref="TW_STATUSUTF8.Free"/>
    /// so there's no memory leak.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="status"></param>
    /// <param name="extendedStatus"></param>
    /// <returns></returns>
    public TWRC Get(ref TW_IDENTITY_LEGACY app, TW_STATUS status, out TW_STATUSUTF8 extendedStatus)
    {
      extendedStatus = new TW_STATUSUTF8 { Status = status };
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
      }
      return rc;
    }

  }
}
