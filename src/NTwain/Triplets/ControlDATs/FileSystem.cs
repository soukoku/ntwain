using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.FILESYSTEM"/>.
  /// </summary>
  public class FileSystem
  {
    public TWRC AutomaticCaptureDirectory(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.AUTOMATICCAPTUREDIRECTORY, ref data);
    public TWRC ChangeDirectory(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.CHANGEDIRECTORY, ref data);
    public TWRC Copy(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.COPY, ref data);
    public TWRC CreateDirectory(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.CREATEDIRECTORY, ref data);
    public TWRC Delete(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.DELETE, ref data);
    public TWRC FormatMedia(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.FORMATMEDIA, ref data);
    public TWRC GetClose(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETCLOSE, ref data);
    public TWRC GetFirstFile(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETFIRSTFILE, ref data);
    public TWRC GetInfo(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETINFO, ref data);
    public TWRC GetNextFile(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETNEXTFILE, ref data);
    public TWRC Rename(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.RENAME, ref data);

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_FILESYSTEM data)
    {
      var rc = TWRC.FAILURE;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
      }
      return rc;
    }
  }
}
