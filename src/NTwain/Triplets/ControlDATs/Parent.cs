using NTwain.Data;
using NTwain.DSM;
using System;

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
    public TWRC OpenDSM(ref TW_IDENTITY_LEGACY app, IntPtr hwnd) 
      => DoIt(ref app, MSG.OPENDSM, hwnd);

    /// <summary>
    /// Closes the DSM.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <returns></returns>
    public TWRC CloseDSM(ref TW_IDENTITY_LEGACY app, IntPtr hwnd)
      => DoIt(ref app, MSG.CLOSEDSM, hwnd);

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, MSG msg, IntPtr hwnd)
    {
      var rc = TWRC.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
      }
      //else if (TwainPlatform.IsLinux)
      //{
      //  var app = Session._appIdentity;
      //  rc = NativeMethods.LinuxDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
      //  if (rc == TWRC.SUCCESS) Session._appIdentity = app;
      //}
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, hwnd);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, hwnd);
        }
      }
      return rc;
    }
  }
}
