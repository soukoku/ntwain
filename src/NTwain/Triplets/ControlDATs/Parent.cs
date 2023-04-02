using NTwain.DSM;
using System;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PARENT"/>.
  /// </summary>
  public class Parent : TripletBase
  {
    public Parent(TwainSession session) : base(session)
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

        // get default source
        if (Session.DGControl.Identity.GetDefault(out TW_IDENTITY_LEGACY ds) == STS.SUCCESS)
        {
          Session.DefaultSource = ds;
        }

        // determine memory mgmt routines used
        if (((DG)Session.AppIdentity.SupportedGroups & DG.DSM2) == DG.DSM2)
        {
          if (Session.DGControl.EntryPoint.Get(out TW_ENTRYPOINT_DELEGATES entry) == STS.SUCCESS)
          {
            Session._entryPoint = entry;
          }
        }
        Session.State = STATE.S3;
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
        Session.State = STATE.S2;
        Session._entryPoint = default;
        Session.DefaultSource = default;
        Session._hwnd = IntPtr.Zero;
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
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
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
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.PARENT, msg, ref hwnd);
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
