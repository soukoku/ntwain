using NTwain.DSM;
using System;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PARENT"/>.
  /// </summary>
  public class Parent
  {
    /// <summary>
    /// Loads and opens the DSM.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <returns></returns>
    public STS OpenDSM(ref TW_IDENTITY_LEGACY app, IntPtr hwnd) 
      => DoIt(ref app, MSG.OPENDSM, hwnd);

    /// <summary>
    /// Closes the DSM.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <returns></returns>
    public STS CloseDSM(ref TW_IDENTITY_LEGACY app, IntPtr hwnd)
      => DoIt(ref app, MSG.CLOSEDSM, hwnd);

    static STS DoIt(ref TW_IDENTITY_LEGACY app, MSG msg, IntPtr hwnd)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
      }
      //else if (TwainPlatform.IsLinux)
      //{
      //  var app = Session._appIdentity;
      //  rc = (STS)NativeMethods.LinuxDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
      //  if (rc == STS.SUCCESS) Session._appIdentity = app;
      //}
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
      }
      return rc;
    }
  }
}
