﻿using NTwain.DSM;
using TWAINWorkingGroup;

namespace NTwain.Triplets.ControlDATs
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/> and <see cref="DAT.USERINTERFACE"/>.
  /// </summary>
  public class UserInterface
  {
    /// <summary>
    /// Disables source to bring state down to <see cref="STATE.S4"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS DisableDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_USERINTERFACE data)
      => DoIt(ref app, ref ds, MSG.DISABLEDS, ref data);

    /// <summary>
    /// Enables source to bring state up to <see cref="STATE.S5"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS EnableDS(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_USERINTERFACE data)
      => DoIt(ref app, ref ds, MSG.ENABLEDS, ref data);

    /// <summary>
    /// Enables source to bring state up to <see cref="STATE.S5"/>.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public STS EnableDSUIOnly(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, ref TW_USERINTERFACE data)
      => DoIt(ref app, ref ds, MSG.ENABLEDSUIONLY, ref data);

    static STS DoIt(ref TW_IDENTITY_LEGACY app, ref TW_IDENTITY_LEGACY ds, MSG msg, ref TW_USERINTERFACE data)
    {
      var rc = STS.FAILURE;
      if (TwainPlatform.IsWindows)
      {
        if (TwainPlatform.Is32bit && TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)WinLegacyDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
        else
        {
          rc = (STS)WinNewDSM.DSM_Entry(ref app, ref ds, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
      }
      else if (TwainPlatform.IsMacOSX)
      {
        TW_IDENTITY_MACOSX app2 = app;
        TW_IDENTITY_MACOSX ds2 = ds;
        if (TwainPlatform.PreferLegacyDSM)
        {
          rc = (STS)OSXLegacyDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
        else
        {
          rc = (STS)OSXNewDSM.DSM_Entry(ref app2, ref ds2, DG.CONTROL, DAT.USERINTERFACE, msg, ref data);
        }
      }
      return rc;
    }
  }
}