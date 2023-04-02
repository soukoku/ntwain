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

        // get default source
        if (Session.DGControl.Identity.GetDefault(out TW_IDENTITY_LEGACY ds) == STS.SUCCESS)
        {
          Session.DefaultSource = ds;
        }

        // determine memory mgmt routines used
        if ((((DG)Session.AppIdentity.SupportedGroups) & DG.DSM2) == DG.DSM2)
        {
          if (Session.DGControl.EntryPoint.Get(out TW_ENTRYPOINT_DELEGATES entry) == STS.SUCCESS)
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
        var app = Session.AppIdentity;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        if (rc == STS.SUCCESS) Session.AppIdentity = app;
      }
      //else if (TwainPlatform.IsLinux)
      //{
      //  var app = Session._appIdentity;
      //  rc = (STS)NativeMethods.LinuxDsmEntryParent(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
      //  if (rc == STS.SUCCESS) Session._appIdentity = app;
      //}
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
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
          Session.AppIdentity = app;
        }
      }
      return rc;
    }
  }
}
