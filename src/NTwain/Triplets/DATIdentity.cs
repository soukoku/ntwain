using System;
using TWAINWorkingGroup;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.IDENTITY"/>.
  /// </summary>
  public class DATIdentity : TripletBase
  {
    public DATIdentity(TwainSession session) : base(session)
    {
    }

    /// <summary>
    /// Loads and opens the specified DS.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS OpenDS(ref TW_IDENTITY_LEGACY ds)
    {
      STS rc;
      if ((rc = DoIt(MSG.OPENDS, ref ds, true)) == STS.SUCCESS)
      {
        Session.State = STATE.S4;
        //// determine memory mgmt routines used
        //if ((((DG)Session._appIdentityLegacy.SupportedGroups) & DG.DSM2) == DG.DSM2)
        //{
        //  TW_ENTRYPOINT_DELEGATES entry = default;
        //  if (Session.DGControl.EntryPoint.Get(ref entry) == STS.SUCCESS)
        //  {
        //    Session._entryPoint = entry;
        //  }
        //}
      }
      return rc;
    }

    ///// <summary>
    ///// Closes the currently open DS.
    ///// </summary>
    ///// <returns></returns>
    //public STS CloseDS(ref IntPtr hwnd)
    //{
    //  STS rc;
    //  if ((rc = DoIt(MSG.CLOSEDSM, ref hwnd)) == STS.SUCCESS)
    //  {
    //    Session._hwnd = IntPtr.Zero;
    //    Session._entryPoint = default;
    //    Session.State = STATE.S2;
    //  }
    //  return rc;
    //}


    STS DoIt(MSG msg, ref TW_IDENTITY_LEGACY ds, bool updateSource)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session._appIdentityLegacy;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
        if (updateSource && rc == STS.SUCCESS)
        {
          Session._dsIdentityLegacy = ds;
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        var app = Session._appIdentityOSX;
        TW_IDENTITY_MACOSX osxds = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        if (updateSource && rc == STS.SUCCESS)
        {
          Session._dsIdentityOSX = osxds;
          Session._dsIdentityLegacy = osxds;
        }
      }
      return rc;
    }
  }
}
