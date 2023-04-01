using System;
using TWAINWorkingGroup;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PARENT"/>.
  /// </summary>
  public class DATParent : TripletBase
  {
    public DATParent(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Loads and opens the DSM.
    /// </summary>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <returns></returns>
    public STS OpenDSM(ref IntPtr hwnd)
    {
      STS rc;
      if ((rc = DoIt(MSG.OPENDSM, ref hwnd)) == STS.SUCCESS)
      {
        Session._hwnd = hwnd;
        Session.State = STATE.S3;
        // determine memory mgmt routines used
        if ((((DG)Session._appIdentityLegacy.SupportedGroups) & DG.DSM2) == DG.DSM2)
        {
          TW_ENTRYPOINT_DELEGATES entry = default;
          if (Session.DGControl.EntryPoint.Get(ref entry) == STS.SUCCESS)
          {
            Session._entryPoint = entry;
          }
        }
      }
      return rc;
    }

    /// <summary>
    /// Closes the DSM.
    /// </summary>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <returns></returns>
    public STS CloseDSM(ref IntPtr hwnd)
    {
      STS rc;
      if ((rc = DoIt(MSG.CLOSEDSM, ref hwnd)) == STS.SUCCESS)
      {
        Session._hwnd = IntPtr.Zero;
        Session._entryPoint = default;
        Session.State = STATE.S2;
      }
      return rc;
    }


    STS DoIt(MSG msg, ref IntPtr hwnd)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session._appIdentityLegacy;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        if (rc == STS.SUCCESS) Session._appIdentityLegacy = app;
      }
      //else if (TwainPlatform.IsLinux)
      //{
      //  var app = Session._appIdentityLegacy;
      //  rc = (STS)NativeMethods.LinuxDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
      //  if (rc == STS.SUCCESS) Session._appIdentityLegacy = app;
      //}
      else if (TwainPlatform.IsMacOSX)
      {
        var app = Session._appIdentityOSX;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        if (rc == STS.SUCCESS)
        {
          Session._appIdentityOSX = app;
          Session._appIdentityLegacy = app;
        }
      }
      return rc;
    }
  }
}
