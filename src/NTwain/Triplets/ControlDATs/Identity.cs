using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.IDENTITY"/>.
/// </summary>
public class Identity
{
    public TWRC OpenDS(TWIdentityWrapper app, TWIdentityWrapper ds)
      => DoIt(app, MSG.OPENDS, ds);

    public TWRC CloseDS(TWIdentityWrapper app, TWIdentityWrapper ds)
      => DoIt(app, MSG.CLOSEDS, ds);

    /// <summary>
    /// Opens the TWAIN data source selector dialog
    /// to choose the default data source.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC UserSelect(TWIdentityWrapper app, out TWIdentityWrapper ds)
    {
        ds = new();
        return DoIt(app, MSG.USERSELECT, ds);
    }

    public TWRC GetDefault(TWIdentityWrapper app, out TWIdentityWrapper ds)
    {
        ds = new();
        return DoIt(app, MSG.GETDEFAULT, ds);
    }

    /// <summary>
    /// Sets the default data source.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC Set(TWIdentityWrapper app, TWIdentityWrapper ds)
      => DoIt(app, MSG.SET, ds);

    /// <summary>
    /// Gets the first available data source in an enumerating fashion 
    /// (use <see cref="GetNext"/> for subsequent ones).
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC GetFirst(TWIdentityWrapper app, out TWIdentityWrapper ds)
    {
        ds = new();
        return DoIt(app, MSG.GETFIRST, ds);
    }

    /// <summary>
    /// Gets the next available data source in an enumerating fashion (after using <see cref="GetFirst"/>).
    /// Ends when return values is <see cref="TWRC.ENDOFLIST"/>.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="ds"></param>
    /// <returns></returns>
    public TWRC GetNext(TWIdentityWrapper app, out TWIdentityWrapper ds)
    {
        ds = new();
        return DoIt(app, MSG.GETNEXT, ds);
    }


    static TWRC DoIt(TWIdentityWrapper app, MSG msg, TWIdentityWrapper ds)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds.TW_IDENTITY_LEGACY);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds.TW_IDENTITY_LEGACY);
            }
            ds.SetIdentity(ds.TW_IDENTITY_LEGACY);
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds.TW_IDENTITY_MACOSX);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, IntPtr.Zero, DG.CONTROL, DAT.IDENTITY, msg, ref ds.TW_IDENTITY_MACOSX);
            }
            ds.SetIdentity(ds.TW_IDENTITY_MACOSX);
        }
        return rc;
    }
}
