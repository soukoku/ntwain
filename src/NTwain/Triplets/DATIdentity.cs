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
    public STS OpenDS(TW_IDENTITY_LEGACY ds) // not a ref on purpose
    {
      STS rc;
      if ((rc = DoIt(MSG.OPENDS, ref ds, true)) == STS.SUCCESS)
      {
        Session._currentDS = ds;
        Session.State = STATE.S4;
        //// determine memory mgmt routines used
        //if ((((DG)Session._appIdentity.SupportedGroups) & DG.DSM2) == DG.DSM2)
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

    /// <summary>
    /// Closes the currently open DS.
    /// </summary>
    /// <returns></returns>
    public STS CloseDS()
    {
     STS rc;
     var ds = Session._currentDS;
     if ((rc = DoIt(MSG.CLOSEDS, ref ds, true)) == STS.SUCCESS)
     {
       Session._currentDS = default;
       Session.State = STATE.S3;
     }
     return rc;
    }

    /// <summary>
    /// Opens the TWAIN source selector dialog
    /// to choose the default source.
    /// </summary>
    /// <returns></returns>
    public STS UserSelect()
    {
      STS rc;
      var ds = Session._defaultDS;
      if ((rc = DoIt(MSG.USERSELECT, ref ds, true)) == STS.SUCCESS)
      {
        Session._defaultDS = ds;
      }
    }


    STS DoIt(MSG msg, ref TW_IDENTITY_LEGACY ds)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session._appIdentity;
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.WindowsTwain32DsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
        else
        {
          rc = (STS)NativeMethods.WindowsTwaindsmDsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app = Session._appIdentity;
        TW_IDENTITY_MACOSX osxds = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)NativeMethods.MacosxTwainDsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        else
        {
          rc = (STS)NativeMethods.MacosxTwaindsmDsmEntryIdentity(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        ds = osxds;
      }
      return rc;
    }
  }
}
