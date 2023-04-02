using NTwain.DSM;
using System;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.IDENTITY"/>.
  /// </summary>
  public class Identity
  {
    /// <summary>
    /// Loads and opens the specified data source.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS OpenDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, MSG.OPENDS, ref ds);

    /// <summary>
    /// Closes the currently open data source.
    /// </summary>
    /// <returns></returns>
    public STS CloseDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, MSG.CLOSEDS, ref ds);

    /// <summary>
    /// Opens the TWAIN data source selector dialog
    /// to choose the default data source.
    /// </summary>
    /// <returns></returns>
    public STS UserSelect(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.USERSELECT, ref ds);
    }

    /// <summary>
    /// Gets the default data source.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS GetDefault(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.GETDEFAULT, ref ds);
    }

    /// <summary>
    /// Sets the default data source.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS Set(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds)
      => DoIt(ref app, MSG.SET, ref ds);

    /// <summary>
    /// Gets the first available data source in an enumerating fashion 
    /// (use <see cref="GetNext"/> for subsequent ones).
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS GetFirst(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.GETFIRST, ref ds);
    }

    /// <summary>
    /// Gets the next available data source in an enumerating fashion (after using <see cref="GetFirst"/>).
    /// Ends when return values is <see cref="STS.ENDOFLIST"/>.
    /// </summary>
    /// <param name="ds"></param>
    /// <returns></returns>
    public STS GetNext(ref TW_IDENTITY_LEGACY app, out TW_IDENTITY_LEGACY ds)
    {
      ds = default;
      return DoIt(ref app, MSG.GETNEXT, ref ds);
    }


    static STS DoIt(ref TW_IDENTITY_LEGACY app, MSG msg, ref TW_IDENTITY_LEGACY ds)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX osxds = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref osxds);
        }
        ds = osxds;
      }
      return rc;
    }
  }
}
