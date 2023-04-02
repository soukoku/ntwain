using NTwain.DSM;
using System;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.STATUSUTF8"/>.
  /// </summary>
  public class StatusUtf8
  {
    /// <summary>
    /// Gets the extended text info for a previously received <see cref="TW_STATUS"/>.
    /// If this is called you should try to extract the string value from it once
    /// with <see cref="TW_STATUSUTF8.ReadAndFree"/> or call <see cref="TW_STATUSUTF8.Free"/>
    /// so there's no memory leak.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="extendedStatus"></param>
    /// <returns></returns>
    public STS Get(ref TW_IDENTITY_LEGACY app, TW_STATUS status, out TW_STATUSUTF8 extendedStatus)
    {
      extendedStatus = new TW_STATUSUTF8 { Status = status };
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.STATUSUTF8, MSG.GET, ref extendedStatus);
        }
      }
      return rc;
    }

  }
}
