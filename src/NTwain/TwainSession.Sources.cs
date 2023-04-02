using NTwain.Data;
using NTwain.Triplets;
using System.Collections.Generic;

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
      var rc = DGControl.Identity.GetFirst(ref _appIdentity, out TW_IDENTITY_LEGACY ds);
      while (rc == STS.SUCCESS)
      {
        yield return ds;
        rc = DGControl.Identity.GetNext(ref _appIdentity, out ds);
      }
    }

    /// <summary>
    /// Shows the TWAIN source selection UI for setting the default source.
    /// </summary>
    public void ShowUserSelect()
    {
      if (DGControl.Identity.UserSelect(ref _appIdentity, out TW_IDENTITY_LEGACY ds) == STS.SUCCESS)
      {
        _defaultDS = ds;
        DefaultSourceChanged?.Invoke(this, ds);
      }
    }

    /// <summary>
    /// Loads and opens the specified data source.
    /// </summary>
    /// <param name="source"></param>
    public void OpenSource(TW_IDENTITY_LEGACY source)
    {
      if (DGControl.Identity.OpenDS(ref _appIdentity, ref source) == STS.SUCCESS)
      {
        RegisterCallback();
        CurrentSource = source;
        State = STATE.S4;
      }
    }

    /// <summary>
    /// Closes the currently open data source.
    /// </summary>
    public void CloseSource()
    {
      if (DGControl.Identity.CloseDS(ref _appIdentity, ref _currentDS) == STS.SUCCESS)
      {
        State = STATE.S3;
        CurrentSource = default;
      }
    }

    /// <summary>
    /// Sets the default data source.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public STS SetDefaultSource(TW_IDENTITY_LEGACY source)
    {
      // TODO: this doesn't work???

      var rc = DGControl.Identity.Set(ref _appIdentity, ref source);
      if (rc == STS.SUCCESS)
      {
        _defaultDS = source;
        DefaultSourceChanged?.Invoke(this, source);
      }
      return rc;
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
      var rc = uiOnly ?
        DGControl.UserInterface.EnableDSUIOnly(ref _appIdentity, ref _currentDS, ref ui) :
        DGControl.UserInterface.EnableDS(ref _appIdentity, ref _currentDS, ref ui);
      if (rc == STS.SUCCESS || (!uiOnly && !showUI && rc == STS.CHECKSTATUS))
      {
        // keep it around for disable use
        _userInterface = ui;
        State = STATE.S5;
      };
      return rc;
    }

    /// <summary>
    /// Disables the currently enabled source.
    /// </summary>
    /// <returns></returns>
    public STS DisableSource()
    {
      var rc = DGControl.UserInterface.DisableDS(ref _appIdentity, ref _currentDS, ref _userInterface);
      if (rc == STS.SUCCESS)
      {
        State = STATE.S4;
      }
      return rc;
    }
  }
}
