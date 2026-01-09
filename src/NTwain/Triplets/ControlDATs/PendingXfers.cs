using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PENDINGXFERS"/>.
/// </summary>
public class PendingXfers
{
    public TWRC EndXfer(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_PENDINGXFERS data)
      => DoIt(app, ds, MSG.ENDXFER, ref data);
    public TWRC Get(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_PENDINGXFERS data)
      => DoIt(app, ds, MSG.GET, ref data);
    public TWRC Reset(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_PENDINGXFERS data)
      => DoIt(app, ds, MSG.RESET, ref data);
    public TWRC StopFeeder(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_PENDINGXFERS data)
      => DoIt(app, ds, MSG.STOPFEEDER, ref data);


    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, ref TW_PENDINGXFERS data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
            }
        }
        return rc;
    }
}
