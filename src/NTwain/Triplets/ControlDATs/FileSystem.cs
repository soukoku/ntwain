using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.FILESYSTEM"/>.
  /// </summary>
  public class FileSystem
  {
    public STS AutomaticCaptureDirectory(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.AUTOMATICCAPTUREDIRECTORY, ref data);
    public STS ChangeDirectory(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.CHANGEDIRECTORY, ref data);
    public STS Copy(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.COPY, ref data);
    public STS CreateDirectory(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.CREATEDIRECTORY, ref data);
    public STS Delete(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.DELETE, ref data);
    public STS FormatMedia(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.FORMATMEDIA, ref data);
    public STS GetClose(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETCLOSE, ref data);
    public STS GetFirstFile(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETFIRSTFILE, ref data);
    public STS GetInfo(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETINFO, ref data);
    public STS GetNextFile(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.GETNEXTFILE, ref data);
    public STS Rename(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_FILESYSTEM data)
      => DoIt(ref app, ref ds, MSG.RENAME, ref data);

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_FILESYSTEM data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.FILESYSTEM, msg, ref data);
        }
      }
      return rc;
    }
  }
}
