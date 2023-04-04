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
      while (rc == TWRC.SUCCESS)
      {
        yield return ds;
        rc = DGControl.Identity.GetNext(ref _appIdentity, out ds);
      }
    }

    /// <summary>
    /// Shows the TWAIN source selection UI for setting the default source.
    /// </summary>
    public STS ShowUserSelect()
    {
      var rc = DGControl.Identity.UserSelect(ref _appIdentity, out TW_IDENTITY_LEGACY ds);
      if (rc == TWRC.SUCCESS)
      {
        _defaultDS = ds;
        DefaultSourceChanged?.Invoke(this, ds);
      }
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Loads and opens the specified data source.
    /// </summary>
    /// <param name="source"></param>
    public STS OpenSource(TW_IDENTITY_LEGACY source)
    {
      var rc = DGControl.Identity.OpenDS(ref _appIdentity, ref source);
      if (rc == TWRC.SUCCESS)
      {
        RegisterCallback();
        CurrentSource = source;
        State = STATE.S4;
      }
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Closes the currently open data source.
    /// </summary>
    public STS CloseSource()
    {
      var rc = DGControl.Identity.CloseDS(ref _appIdentity, ref _currentDS);
      if (rc == TWRC.SUCCESS)
      {
        State = STATE.S3;
        CurrentSource = default;
      }
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Sets the default data source.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public STS SetDefaultSource(TW_IDENTITY_LEGACY source)
    {
      // this doesn't work on windows legacy twain_32.dll

      var rc = DGControl.Identity.Set(ref _appIdentity, ref source);
      if (rc == TWRC.SUCCESS)
      {
        _defaultDS = source;
        DefaultSourceChanged?.Invoke(this, source);
      }
      return WrapInSTS(rc);
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
      if (State > STATE.S4)
      {
        // already enabled :(
        // TODO: should bring it down?
      }

      _userInterface = new TW_USERINTERFACE
      {
        ShowUI = (ushort)((showUI || uiOnly) ? 1 : 0),
        hParent = _hwnd,
      };
      var rc = uiOnly ?
        DGControl.UserInterface.EnableDSUIOnly(ref _appIdentity, ref _currentDS, ref _userInterface) :
        DGControl.UserInterface.EnableDS(ref _appIdentity, ref _currentDS, ref _userInterface);
      if (rc == TWRC.SUCCESS || (!uiOnly && !showUI && rc == TWRC.CHECKSTATUS))
      {
        State = STATE.S5;
      }
      else
      {
        _userInterface = default;
      }
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Disables the currently enabled source.
    /// </summary>
    /// <returns></returns>
    public STS DisableSource()
    {
      var rc = DGControl.UserInterface.DisableDS(ref _appIdentity, ref _currentDS, ref _userInterface);
      if (rc == TWRC.SUCCESS)
      {
        State = STATE.S4;
      }
      return WrapInSTS(rc);
    }
  }
}
