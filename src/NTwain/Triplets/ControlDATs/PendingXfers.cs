﻿using NTwain.Data;
using NTwain.DSM;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.PENDINGXFERS"/>.
  /// </summary>
  public class PendingXfers
  {
    public STS EndXfer(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_PENDINGXFERS data)
      => DoIt(ref app, ref ds, MSG.ENDXFER, ref data);
    public STS Get(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_PENDINGXFERS data)
      => DoIt(ref app, ref ds, MSG.GET, ref data);
    public STS Reset(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_PENDINGXFERS data)
      => DoIt(ref app, ref ds, MSG.RESET, ref data);
    public STS StopFeeder(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_PENDINGXFERS data)
      => DoIt(ref app, ref ds, MSG.STOPFEEDER, ref data);


    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_PENDINGXFERS data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.PENDINGXFERS, msg, ref data);
        }
      }
      return rc;
    }
  }
}
