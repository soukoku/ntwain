using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.IDENTITY"/>.
  /// </summary>
  public class Identity
  {
    public TWRC OpenDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, MSG.OPENDS, ref ds);

    public TWRC CloseDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, MSG.CLOSEDS, ref ds);

    /// <summary>
    /// Opens the TWAIN data source selector dialog
    /// to choose the default data source.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC UserSelect(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.USERSELECT, ref ds);
    }

    public TWRC GetDefault(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.GETDEFAULT, ref ds);
    }

    /// <summary>
    /// Sets the default data source.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, MSG.SET, ref ds);

    /// <summary>
    /// Gets the first available data source in an enumerating fashion 
    /// (use <see cref="GetNext"/> for subsequent ones).
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC GetFirst(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.GETFIRST, ref ds);
    }

    /// <summary>
    /// Gets the next available data source in an enumerating fashion (after using <see cref="GetFirst"/>).
    /// Ends when return values is <see cref="TWRC.ENDOFLIST"/>.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC GetNext(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.GETNEXT, ref ds);
    }


    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, MSG msg, ref TW_IDENTITY_LEGACY ds)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX osxds = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        ds = osxds;
      }
      return rc;
    }
  }
}
