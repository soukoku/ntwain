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
    /// Loads and opens the specified data source.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS OpenDS(TW_IDENTITY_LEGACY ds) // not a ref on purpose
    {
      STS rc;
      if ((rc = DoIt(MSG.OPENDS, ref ds)) == STS.SUCCESS)
      {
        Session.CurrentSource = ds;
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
    /// Closes the currently open data source.
    /// </summary>
    /// <returns></returns>
    public STS CloseDS()
    {
      STS rc;
      var ds = Session.CurrentSource;
      if ((rc = DoIt(MSG.CLOSEDS, ref ds)) == STS.SUCCESS)
      {
        Session.CurrentSource = default;
        Session.State = STATE.S3;
      }
      return rc;
    }

    /// <summary>
    /// Opens the TWAIN data source selector dialog
    /// to choose the default data source.
    /// </summary>
    /// <returns></returns>
    public STS UserSelect()
    {
      STS rc;
      var ds = Session.DefaultSource;
      if ((rc = DoIt(MSG.USERSELECT, ref ds)) == STS.SUCCESS)
      {
        Session.DefaultSource = ds;
      }
      return rc;
    }

    /// <summary>
    /// Gets the default data source.
    /// </summary>
    /// <returns></returns>
    public STS GetDefault(out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(MSG.GETDEFAULT, ref ds);
    }

    /// <summary>
    /// Sets the default data source.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS Set(TW_IDENTITY_LEGACY ds)
    {
      STS rc;
      if ((rc = DoIt(MSG.SET, ref ds)) == STS.SUCCESS)
      {
        Session.DefaultSource = ds;
      }
      return rc;
    }

    /// <summary>
    /// Gets the first available data source in an enumerating fashion 
    /// (use <see cref="GetNext"/> for subsequent ones).
    /// </summary>
    /// <returns></returns>
    public STS GetFirst(out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(MSG.GETFIRST, ref ds);
    }

    /// <summary>
    /// Gets the next available data source in an enumerating fashion (after using <see cref="GetFirst"/>).
    /// Ends when return values is <see cref="STS.ENDOFLIST"/>.
    /// </summary>
    /// <returns></returns>
    public STS GetNext(out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(MSG.GETNEXT, ref ds);
    }


    STS DoIt(MSG msg, ref TW_IDENTITY_LEGACY ds)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        var app = Session.AppIdentity;
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
        TW_IDENTITY_MACOSX app = Session.AppIdentity;
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
