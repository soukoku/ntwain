using NTwain.Triplets;
using System.Collections.Generic;
using TWAINWorkingGroup;

namespace NTwain
{
  // this file contains data source utilities

  partial class TwainSession
  {
    /// <summary>
    /// Gets all available sources.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TW_IDENTITY_LEGACY> GetSources()
    {
      var rc = DGControl.Identity.GetFirst(out TW_IDENTITY_LEGACY ds);
      while (rc == STS.SUCCESS)
      {
        yield return ds;
        rc = DGControl.Identity.GetNext(out ds);
      }
    }

    /// <summary>
    /// Enables the currently open source.
    /// </summary>
    /// <param name="showUI">Whether to show driver interface.</param>
    /// <param name="uiOnly">If true try to display only driver dialog (no capture). 
    /// Otherwise capturing will begin after this.</param>
    /// <returns></returns>
    public STS EnableSource(bool showUI, bool uiOnly)
    {
      var ui = new TW_USERINTERFACE
      {
        ShowUI = (ushort)((showUI || uiOnly) ? 1 : 0),
        hParent = _hwnd,
      };
      var rc = uiOnly ? DGControl.UserInterface.EnableDSUIOnly(ref ui) : DGControl.UserInterface.EnableDS(ref ui);
      if (rc == STS.SUCCESS || (!uiOnly && !showUI && rc == STS.CHECKSTATUS))
      {
        // keep it around for disable use
        _userInterface = ui;
      };
      return rc;
    }

    /// <summary>
    /// Disables the currently enabled source.
    /// </summary>
    /// <returns></returns>
    public STS DisableSource()
    {
      return DGControl.UserInterface.DisableDS(ref _userInterface);
    }
  }
}
