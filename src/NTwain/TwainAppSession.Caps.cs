using NTwain.Caps;
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
    private KnownCaps? _knownCaps;

    /// <summary>
    /// Access the known and pre-defined caps as properties.
    /// </summary>
    public KnownCaps Caps
    {
      get { return _knownCaps ??= new KnownCaps(this); }
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
    /// Gets a CAP's raw current value.
    /// Caller will need to manually read and free the memory.
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
    /// Gets a CAP's current value. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapCurrent<TValue>(CAP cap, out TValue value) where TValue : struct
    {
      value = default;
      var sts = GetCapCurrent(cap, out TW_CAPABILITY twcap);
      if (sts.RC == TWRC.SUCCESS)
      {
        switch (twcap.ConType)
        {
          case TWON.ONEVALUE:
            value = twcap.ReadOneValue<TValue>(this);
            break;
          case TWON.ENUMERATION:
            var twenum = twcap.ReadEnumeration<TValue>(this);
            if (twenum.Items != null && twenum.CurrentIndex < twenum.Items.Length)
            {
              value = twenum.Items[twenum.CurrentIndex];
            }
            break;
          case TWON.RANGE:
            value = twcap.ReadRange<TValue>(this).CurrentValue;
            break;
          case TWON.ARRAY:
            // no source should ever return an array but anyway
            var twarr = twcap.ReadArray<TValue>(this);
            if (twarr != null && twarr.Count > 0) value = twarr[0];
            break;
          default:
            twcap.Free(this); break;
        }
      }
      return sts;
    }

    /// <summary>
    /// Gets a CAP's raw default value.
    /// Caller will need to manually read and free the memory.
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
    /// Gets a CAP's default value. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapDefault<TValue>(CAP cap, out TValue value) where TValue : struct
    {
      value = default;
      var sts = GetCapDefault(cap, out TW_CAPABILITY twcap);
      if (sts.RC == TWRC.SUCCESS)
      {
        switch (twcap.ConType)
        {
          case TWON.ONEVALUE:
            value = twcap.ReadOneValue<TValue>(this);
            break;
          case TWON.ENUMERATION:
            var twenum = twcap.ReadEnumeration<TValue>(this);
            if (twenum.Items != null && twenum.DefaultIndex < twenum.Items.Length)
            {
              value = twenum.Items[twenum.DefaultIndex];
            }
            break;
          case TWON.RANGE:
            value = twcap.ReadRange<TValue>(this).DefaultValue;
            break;
          case TWON.ARRAY:
            // no source should ever return an array but anyway
            var twarr = twcap.ReadArray<TValue>(this);
            if (twarr != null && twarr.Count > 0) value = twarr[0];
            break;
          default:
            twcap.Free(this); break;
        }
      }
      return sts;
    }

    /// <summary>
    /// Gets a CAP's raw supported values.
    /// Caller will need to manually read and free the memory.
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
    /// Gets a CAP's supported values. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS GetCapValues<TValue>(CAP cap, out IList<TValue> values) where TValue : struct
    {
      values = new List<TValue>();
      var sts = GetCapValues(cap, out TW_CAPABILITY twcap);
      if (sts.RC == TWRC.SUCCESS)
      {
        switch (twcap.ConType)
        {
          case TWON.ONEVALUE:
            values.Add(twcap.ReadOneValue<TValue>(this));
            break;
          case TWON.ENUMERATION:
            var twenum = twcap.ReadEnumeration<TValue>(this);
            if (twenum.Items != null && twenum.Items.Length > 0)
              ((List<TValue>)values).AddRange(twenum.Items);
            break;
          case TWON.RANGE:
            // This can be slow
            var twrange = twcap.ReadRange<TValue>(this);
            ((List<TValue>)values).AddRange(twrange);
            break;
          case TWON.ARRAY:
            var twarr = twcap.ReadArray<TValue>(this);
            if (twarr != null && twarr.Count > 0)
              ((List<TValue>)values).AddRange(twarr);
            break;
          default:
            twcap.Free(this); break;
        }
      }
      return sts;
    }

    /// <summary>
    /// Gets a CAP's help text (description).
    /// This may not work due to unclear spec.
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
        help = value.ReadString(this, false);
      }
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Gets a CAP's text name label.
    /// This may not work due to unclear spec.
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
        label = value.ReadString(this, false);
      }
      value.Free(this);
      return WrapInSTS(rc);
    }

    /// <summary>
    /// Gets a CAP's enum/array value label texts.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="labels"></param>
    /// <returns></returns>
    public STS GetCapLabelEnum(CAP cap, out IList<string> labels)
    {
      labels = Array.Empty<string>();
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
    /// An easy way to create a value is to use the 
    /// <see cref="ValueWriter.CreateOneValueCap{TValue}(CAP, IMemoryManager, TValue)"/>
    /// extension method (or the other container variants).
    /// Memory of the value will be freed afterwards.
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
    /// A simpler cap value setter for common one-value scenarios
    /// that's easier to use. Not for other container type sets.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetCap<TValue>(CAP cap, TValue value) where TValue : struct
    {
      var twcap = ValueWriter.CreateOneValueCap(cap, this, value);
      return SetCap(ref twcap);
    }

    /// <summary>
    /// Sets a CAP's constraint values.
    /// An easy way to create a value is to use the 
    /// <see cref="ValueWriter.CreateOneValueCap{TValue}(CAP, IMemoryManager, TValue)"/>
    /// extension method (or the other container variants).
    /// Memory of the value will be freed afterwards.
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
    /// Caller will need to manually read and free the memory.
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
    /// Resets a CAP's current value to power-on default.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS ResetCap<TValue>(CAP cap, out TValue value) where TValue : struct
    {
      value = default;
      var sts = ResetCap(cap, out TW_CAPABILITY twcap);
      if (sts.RC == TWRC.SUCCESS)
      {
        switch (twcap.ConType)
        {
          case TWON.ONEVALUE:
            value = twcap.ReadOneValue<TValue>(this);
            break;
          case TWON.ENUMERATION:
            var twenum = twcap.ReadEnumeration<TValue>(this);
            if (twenum.Items != null && twenum.CurrentIndex < twenum.Items.Length)
            {
              value = twenum.Items[twenum.CurrentIndex];
            }
            break;
          case TWON.RANGE:
            value = twcap.ReadRange<TValue>(this).CurrentValue;
            break;
          case TWON.ARRAY:
            var twarr = twcap.ReadArray<TValue>(this);
            if (twarr != null && twarr.Count > 0) value = twarr[0];
            break;
          default:
            twcap.Free(this); break;
        }
      }
      return sts;
    }

    /// <summary>
    /// Resets all CAP values and constraint to power-on defaults.
    /// </summary>
    /// <returns></returns>
    public STS ResetAllCaps()
    {
      // no memory is allocated for this
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
      var rc2 = GetCapCurrent(CAP.CAP_LANGUAGE, out TWLG lang);
      if (rc2.RC == TWRC.SUCCESS)
      {
        Language.Set(lang);
      }
    }
  }
}
