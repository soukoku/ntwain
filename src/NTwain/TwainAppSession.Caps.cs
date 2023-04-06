using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
  // this file contains capability mgmt methods

  partial class TwainAppSession
  {
    /// <summary>
    /// Gets all the supported caps for the current source.
    /// </summary>
    /// <returns></returns>
    public IList<CAP> GetAllCaps()
    {
      // just as a sample of how to read cap values

      if (GetCapValues(CAP.CAP_SUPPORTEDCAPS, out TW_CAPABILITY value).RC == TWRC.SUCCESS)
      {
        return value.ReadArray<CAP>(this);
      }
      return Array.Empty<CAP>();
    }

    /// <summary>
    /// Gets a CAP's actual supported operations. 
    /// This is not supported by all sources.
    /// </summary>
    /// <param name="cap"></param>
    /// <returns></returns>
    public TWQC QueryCapSupport(CAP cap)
    {
      var value = new TW_CAPABILITY(cap) { ConType = TWON.ONEVALUE };
      if (DGControl.Capability.QuerySupport(ref _appIdentity, ref _currentDS, ref value) == TWRC.SUCCESS)
      {
        return value.ReadOneValue<TWQC>(this);
      }
      return TWQC.Unknown;
    }

    /// <summary>
    /// Gets a CAP's current value.
    /// Caller will need to free the memory.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapCurrent(CAP cap, out TW_CAPABILITY value)
    {
      value = new TW_CAPABILITY(cap);
      return WrapInSTS(DGControl.Capability.GetCurrent(ref _appIdentity, ref _currentDS, ref value));
    }

    /// <summary>
    /// Gets a CAP's default value.
    /// Caller will need to free the memory.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapDefault(CAP cap, out TW_CAPABILITY value)
    {
      value = new TW_CAPABILITY(cap);
      return WrapInSTS(DGControl.Capability.GetDefault(ref _appIdentity, ref _currentDS, ref value));
    }

    /// <summary>
    /// Gets a CAP's supported values.
    /// Caller will need to free the memory.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapValues(CAP cap, out TW_CAPABILITY value)
    {
      value = new TW_CAPABILITY(cap);
      return WrapInSTS(DGControl.Capability.Get(ref _appIdentity, ref _currentDS, ref value));
    }

    /// <summary>
    /// Gets a CAP's help text (description).
    /// This is not implemented.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="help"></param>
    /// <returns></returns>
    public STS GetCapHelp(CAP cap, out string? help)
    {
      help = null;
      var value = new TW_CAPABILITY(cap);
      var rc = DGControl.Capability.GetHelp(ref _appIdentity, ref _currentDS, ref value);
      if (rc == TWRC.SUCCESS)
      {
        // how to determine the length of this thing???
        var data = value.ReadOneValue<IntPtr>(this, false);
      }
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Gets a CAP's text name label.
    /// This is not implemented.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    public STS GetCapLabel(CAP cap, out string? label)
    {
      label = null;
      var value = new TW_CAPABILITY(cap) { ConType = TWON.ONEVALUE };
      var rc = DGControl.Capability.GetLabel(ref _appIdentity, ref _currentDS, ref value);
      if (rc == TWRC.SUCCESS)
      {
        // how to determine the length of this thing???
        var data = value.ReadOneValue<IntPtr>(this, false);
      }
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Gets a CAP's value label texts.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="labels"></param>
    /// <returns></returns>
    public STS GetCapLabelEnum(CAP cap, out IList<string>? labels)
    {
      labels = null;
      var value = new TW_CAPABILITY(cap);
      var rc = DGControl.Capability.GetLabelEnum(ref _appIdentity, ref _currentDS, ref value);
      if (rc == TWRC.SUCCESS)
      {
        // spec says they're utf8
        labels = value.ReadArray<TW_STR255>(this, false).Select(t => t.Get(Encoding.UTF8)).ToList();
      }
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Sets a CAP's current value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetCap(ref TW_CAPABILITY value)
    {
      var rc = DGControl.Capability.Set(ref _appIdentity, ref _currentDS, ref value);
      value.Free(this);

      if (value.Cap == CAP.CAP_LANGUAGE && rc == TWRC.SUCCESS)
      {
        RefreshCapLanguage();
      }

      return WrapInSTS(rc);
    }

    /// <summary>
    /// Sets a CAP's constraint values.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetConstraint(ref TW_CAPABILITY value)
    {
      var rc = DGControl.Capability.SetConstraint(ref _appIdentity, ref _currentDS, ref value);
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Resets a CAP's current value to power-on default.
    /// Caller will need to free the memory.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS ResetCap(CAP cap, out TW_CAPABILITY value)
    {
      value = new TW_CAPABILITY(cap);
      var rc = DGControl.Capability.Reset(ref _appIdentity, ref _currentDS, ref value);

      if (value.Cap == CAP.CAP_LANGUAGE && rc == TWRC.SUCCESS)
      {
        RefreshCapLanguage();
      }

      return WrapInSTS(rc);
    }

    /// <summary>
    /// Resets all CAP values and constraint to power-on defaults.
    /// </summary>
    /// <returns></returns>
    public STS ResetAllCaps()
    {
      var value = new TW_CAPABILITY(CAP.CAP_SUPPORTEDCAPS);
      var rc = DGControl.Capability.ResetAll(ref _appIdentity, ref _currentDS, ref value);

      if (rc == TWRC.SUCCESS)
      {
        RefreshCapLanguage();
      }

      return WrapInSTS(rc);
    }

    private void RefreshCapLanguage()
    {
      var rc2 = GetCapCurrent(CAP.CAP_LANGUAGE, out TW_CAPABILITY curCap);
      if (rc2.RC == TWRC.SUCCESS)
      {
        var lang = curCap.ReadOneValue<TWLG>(this);
        Language.Set(lang);
      }
    }
  }
}
