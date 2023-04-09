using NTwain.Data;
using NTwain.DSM;
using System;

namespace NTwain.Triplets.ImageDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.IMAGE"/> and <see cref="DAT.IMAGENATIVEXFER"/>.
  /// </summary>
  public class ImageNativeXfer
  {
    public TWRC Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, out IntPtr data)
      => DoIt(ref app, ref ds, MSG.GET, out data);

    static TWRC DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, out IntPtr data)
    {
      var rc = TWRC.FAILURE;
      data = IntPtr.Zero;
      if (TWPlatform.IsWindows)
      {
        if (TWPlatform.Is32bit && TWPlatform.PreferLegacyDSM)
        {
          rc = WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
        }
        else
        {
          rc = WinNewDSM.DSM_Entry(ref app, ref ds, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
        }
      }
      else if (TWPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TWPlatform.PreferLegacyDSM)
        {
          rc = OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
        }
        else
        {
          rc = OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.IMAGE, DAT.IMAGENATIVEXFER, msg, ref data);
        }
      }
      return rc;
    }
  }
}
