using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs;

/// <summary>
/// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.FILESYSTEM"/>.
/// </summary>
public class FileSystem
{
    public TWRC AutomaticCaptureDirectory(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.AUTOMATICCAPTUREDIRECTORY, ref data);
    public TWRC ChangeDirectory(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.CHANGEDIRECTORY, ref data);
    public TWRC Copy(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.COPY, ref data);
    public TWRC CreateDirectory(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.CREATEDIRECTORY, ref data);
    public TWRC Delete(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.DELETE, ref data);
    public TWRC FormatMedia(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.FORMATMEDIA, ref data);
    public TWRC GetClose(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.GETCLOSE, ref data);
    public TWRC GetFirstFile(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.GETFIRSTFILE, ref data);
    public TWRC GetInfo(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.GETINFO, ref data);
    public TWRC GetNextFile(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.GETNEXTFILE, ref data);
    public TWRC Rename(TWIdentityWrapper app, TWIdentityWrapper ds, ref TW_FILESYSTEM data)
      => DoIt(app, ds, MSG.RENAME, ref data);

    static TWRC DoIt(TWIdentityWrapper app, TWIdentityWrapper ds, MSG msg, ref TW_FILESYSTEM data)
    {
        var rc = TWRC.FAILURE;
        if (TWPlatform.IsWindows)
        {
            if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
            {
                rc = WinLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
            }
            else
            {
                rc = WinNewDSM.DSM_Entry(ref app.TW_IDENTITY_LEGACY, ref ds.TW_IDENTITY_LEGACY, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
            }
        }
        else if (TWPlatform.IsMacOSX)
        {
            if (TWPlatform.PreferLegacyDSM)
            {
                rc = OSXLegacyDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
            }
            else
            {
                rc = OSXNewDSM.DSM_Entry(ref app.TW_IDENTITY_MACOSX, ref ds.TW_IDENTITY_MACOSX, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
            }
        }
        return rc;
    }
}
