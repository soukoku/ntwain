using NTwain.Data;
using NTwain.Triplets;
using System.Collections.Generic;
using System.Linq;

namespace NTwain
{
  // this file contains capability mgmt methods

  partial class TwainSession
  {
    ///// <summary>
    ///// Gets all the supported caps for the current source.
    ///// </summary>
    ///// <returns></returns>
    //public IEnumerable<CAP> GetAllCaps()
    //{
    //  // just as a sample of how to read cap values

    //  if (GetCapValues(CAP.CAP_SUPPORTEDCAPS, out TW_CAPABILITY value) == TWRC.SUCCESS)
    //  {
    //    value.Read(this);
    //  }
    //  return Enumerable.Empty<CAP>();
    //}

    /// <summary>
    /// Gets a CAP's actual supported operations. 
    /// This is not supported by all sources.
    /// </summary>
    /// <param name="cap"></param>
    /// <returns></returns>
    public TWQC QueryCapSupport(CAP cap)
    {
      var value = new TW_CAPABILITY(cap);
      if (DGControl.Capability.QuerySupport(ref _appIdentity, ref _currentDS, ref value) == TWRC.SUCCESS)
      {
        value.Read(this);
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
      return WrapInSTS(DGControl.Capability.Get(ref _appIdentity, ref _currentDS, ref value));
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
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapHelp(CAP cap, out string? help)
    {
      help = null;
      var value = new TW_CAPABILITY(cap);
      var rc = DGControl.Capability.GetHelp(ref _appIdentity, ref _currentDS, ref value);
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Gets a CAP's text name label.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapLabel(CAP cap, out string? label)
    {
      label = null;
      var value = new TW_CAPABILITY(cap);
      var rc = DGControl.Capability.GetLabel(ref _appIdentity, ref _currentDS, ref value);
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Gets a CAP's value label texts.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapLabelEnum(CAP cap, out string[]? labels)
    {
      labels = null;
      var value = new TW_CAPABILITY(cap);
      var rc = DGControl.Capability.GetLabelEnum(ref _appIdentity, ref _currentDS, ref value);
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Sets a CAP's current value.
    /// </summary>
    /// <param name="cap"></param>
    /// <returns></returns>
    public STS SetCap(ref TW_CAPABILITY value)
    {
      var rc = DGControl.Capability.Set(ref _appIdentity, ref _currentDS, ref value);
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Sets a CAP's constraint values.
    /// </summary>
    /// <param name="cap"></param>
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
      return WrapInSTS(DGControl.Capability.Reset(ref _appIdentity, ref _currentDS, ref value));
    }

    /// <summary>
    /// Resets all CAP values and constraint to power-on defaults.
    /// </summary>
    /// <returns></returns>
    public STS ResetAllCaps()
    {
      var value = new TW_CAPABILITY(CAP.CAP_SUPPORTEDCAPS);
      return WrapInSTS(DGControl.Capability.ResetAll(ref _appIdentity, ref _currentDS, ref value));
    }
  }
}
