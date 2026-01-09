using NTwain.Caps;
using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain;

// this file contains capability mgmt methods

// TODO: need to marshal to pump thread?

partial class TwainAppSession
{
    private BuiltInCaps? _builtinCaps;

    /// <summary>
    /// Access the built-in TWAIN caps as properties.
    /// </summary>
    public BuiltInCaps Caps
    {
        get { return _builtinCaps ??= new BuiltInCaps(this); }
    }

    private KdsCaps? _kdsCaps;

    /// <summary>
    /// Access the Kodak custom caps as properties.
    /// </summary>
    public KdsCaps KdsCaps
    {
        get { return _kdsCaps ??= new KdsCaps(this); }
    }

    /// <summary>
    /// Gets a CAP's actual supported operations. 
    /// This is not supported by all sources.
    /// </summary>
    /// <param name="cap"></param>
    /// <returns></returns>
    public TWQC QueryCapSupport(CAP cap)
    {
        if (_currentDs == null) return TWQC.Unknown;

        var value = new TW_CAPABILITY(cap) { ConType = TWON.ONEVALUE };
        if (DGControl.Capability.QuerySupport(_appIdentity, _currentDs, ref value) == TWRC.SUCCESS)
        {
            return value.ReadOneValue<TWQC>(MemoryManager);
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
        if (_currentDs == null) return STS.SequenceError();

        return WrapInSTS(DGControl.Capability.GetCurrent(_appIdentity, _currentDs, ref value));
    }

    /// <summary>
    /// Gets a CAP's current value. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapCurrent<TValue>(CAP cap, out List<TValue> value) where TValue : struct
    {
        value = [];
        var sts = GetCapCurrent(cap, out TW_CAPABILITY twcap);
        if (sts.RC == TWRC.SUCCESS)
        {
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    value.Add(twcap.ReadOneValue<TValue>(MemoryManager));
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumeration<TValue>(MemoryManager);
                    if (twenum.Items != null && twenum.CurrentIndex < twenum.Items.Length)
                    {
                        value.Add(twenum.Items[twenum.CurrentIndex]);
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRange<TValue>(MemoryManager);
                    if (range != null) value.Add(range.CurrentValue);
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArray<TValue>(MemoryManager);
                    if (twarr != null && twarr.Count > 0) value.AddRange(twarr);
                    break;
                default:
                    twcap.Free(MemoryManager); break;
            }
        }
        return sts;
    }

    /// <summary>
    /// Gets a CAP's current value as boxed values. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapCurrentBoxed(CAP cap, out List<object> value)
    {
        value = [];
        var sts = GetCapCurrent(cap, out TW_CAPABILITY twcap);
        if (sts.RC == TWRC.SUCCESS)
        {
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    var read = twcap.ReadOneValueBoxed(MemoryManager);
                    if (read != null) value.Add(read);
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumerationBoxed(MemoryManager);
                    if (twenum.Items != null && twenum.CurrentIndex < twenum.Items.Length)
                    {
                        value.Add(twenum.Items[twenum.CurrentIndex]);
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRangeBoxed(MemoryManager);
                    if (range != null) value.Add(range.CurrentValue);
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArrayBoxed(MemoryManager);
                    if (twarr != null && twarr.Count > 0) value.AddRange(twarr);
                    break;
                default:
                    twcap.Free(MemoryManager); break;
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
        if (_currentDs == null) return STS.SequenceError();

        return WrapInSTS(DGControl.Capability.GetDefault(_appIdentity, _currentDs, ref value));
    }


    /// <summary>
    /// Gets a CAP's default value. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapDefault<TValue>(CAP cap, out List<TValue> value) where TValue : struct
    {
        value = [];
        var sts = GetCapDefault(cap, out TW_CAPABILITY twcap);
        if (sts.RC == TWRC.SUCCESS)
        {
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    value.Add(twcap.ReadOneValue<TValue>(MemoryManager));
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumeration<TValue>(MemoryManager);
                    if (twenum.Items != null && twenum.DefaultIndex < twenum.Items.Length)
                    {
                        value.Add(twenum.Items[twenum.DefaultIndex]);
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRange<TValue>(MemoryManager);
                    if (range != null) value.Add(range.DefaultValue);
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArray<TValue>(MemoryManager);
                    if (twarr != null && twarr.Count > 0) value.AddRange(twarr);
                    break;
                default:
                    twcap.Free(MemoryManager); break;
            }
        }
        return sts;
    }

    /// <summary>
    /// Gets a CAP's default value. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapDefaultBoxed(CAP cap, out List<object> value)
    {
        value = [];
        var sts = GetCapDefault(cap, out TW_CAPABILITY twcap);
        if (sts.RC == TWRC.SUCCESS)
        {
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    var read = twcap.ReadOneValueBoxed(MemoryManager);
                    if (read != null) value.Add(read);
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumerationBoxed(MemoryManager);
                    if (twenum.Items != null && twenum.DefaultIndex < twenum.Items.Length)
                    {
                        value.Add(twenum.Items[twenum.DefaultIndex]);
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRangeBoxed(MemoryManager);
                    if (range != null) value.Add(range.DefaultValue);
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArrayBoxed(MemoryManager);
                    if (twarr != null && twarr.Count > 0) value.AddRange(twarr);
                    break;
                default:
                    twcap.Free(MemoryManager); break;
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
        if (_currentDs == null) return STS.SequenceError();

        return WrapInSTS(DGControl.Capability.Get(_appIdentity, _currentDs, ref value));
    }


    /// <summary>
    /// Gets a CAP's supported values. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapValues<TValue>(CAP cap, out ValueContainer<TValue> value) where TValue : struct
    {
        value = new ValueContainer<TValue> { ContainerType = TWON.DONTCARE };
        var sts = GetCapValues(cap, out TW_CAPABILITY twcap);
        if (sts.RC == TWRC.SUCCESS)
        {
            value.ContainerType = twcap.ConType;
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    value.OneValue = twcap.ReadOneValue<TValue>(MemoryManager);
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumeration<TValue>(MemoryManager);
                    if (twenum.Items != null)
                    {
                        value.EnumValue = twenum;
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRange<TValue>(MemoryManager);
                    value.RangeValue = range;
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArray<TValue>(MemoryManager);
                    if (twarr != null)
                    {
                        value.ArrayValue = twarr;
                    }
                    break;
                default:
                    twcap.Free(MemoryManager); break;
            }
        }
        return sts;
    }


    /// <summary>
    /// Gets a CAP's supported values. This is a simplified version that doesn't require
    /// manual reading, but may or may not work.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS GetCapValuesBoxed(CAP cap, out ValueContainer<object> value)
    {
        value = new ValueContainer<object> { ContainerType = TWON.DONTCARE };
        var sts = GetCapValues(cap, out TW_CAPABILITY twcap);
        if (sts.RC == TWRC.SUCCESS)
        {
            value.ContainerType = twcap.ConType;
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    value.OneValue = twcap.ReadOneValueBoxed(MemoryManager);
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumerationBoxed(MemoryManager);
                    if (twenum.Items != null)
                    {
                        value.EnumValue = twenum;
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRangeBoxed(MemoryManager);
                    value.RangeValue = range;
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArrayBoxed(MemoryManager);
                    if (twarr != null)
                    {
                        value.ArrayValue = twarr;
                    }
                    break;
                default:
                    twcap.Free(MemoryManager); break;
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
        if (_currentDs == null) return STS.SequenceError();

        var value = new TW_CAPABILITY(cap);
        var rc = DGControl.Capability.GetHelp(_appIdentity, _currentDs, ref value);
        if (rc == TWRC.SUCCESS)
        {
            help = value.ReadString(MemoryManager, false);
        }
        value.Free(MemoryManager);
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
        if (_currentDs == null) return STS.SequenceError();

        var value = new TW_CAPABILITY(cap) { ConType = TWON.ONEVALUE };
        var rc = DGControl.Capability.GetLabel(_appIdentity, _currentDs, ref value);
        if (rc == TWRC.SUCCESS)
        {
            label = value.ReadString(MemoryManager, false);
        }
        value.Free(MemoryManager);
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
        if (_currentDs == null) return STS.SequenceError();

        var value = new TW_CAPABILITY(cap);
        var rc = DGControl.Capability.GetLabelEnum(_appIdentity, _currentDs, ref value);
        if (rc == TWRC.SUCCESS)
        {
            // spec says they're utf8
            labels = value.ReadArray<TW_STR255>(MemoryManager, false).Select(t => t.Get(Encoding.UTF8)).ToList();
        }
        value.Free(MemoryManager);
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
        if (_currentDs == null) return STS.SequenceError();

        var rc = DGControl.Capability.Set(_appIdentity, _currentDs, ref value);
        value.Free(MemoryManager);

        if (value.Cap == CAP.CAP_LANGUAGE && rc == TWRC.SUCCESS)
        {
            RefreshCapLanguage();
        }

        return WrapInSTS(rc);
    }

    /// <summary>
    /// A simpler cap value setter for one-value type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetCap<TValue>(CAP cap, TValue value) where TValue : struct
    {
        var twcap = ValueWriter.CreateOneValueCap(cap, MemoryManager, value);
        return SetCap(ref twcap);
    }

    /// <summary>
    /// A cap value setter for enumeration container type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetCap<TValue>(CAP cap, Enumeration<TValue> value) where TValue : struct
    {
        var twcap = ValueWriter.CreateEnumCap(cap, MemoryManager, value);
        return SetCap(ref twcap);
    }

    /// <summary>
    /// A cap value setter for range container type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetCap<TValue>(CAP cap, Range<TValue> value) where TValue : struct
    {
        var twcap = ValueWriter.CreateRangeCap(cap, MemoryManager, value);
        return SetCap(ref twcap);
    }

    /// <summary>
    /// A cap value setter for array container type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetCap<TValue>(CAP cap, IList<TValue> value) where TValue : struct
    {
        var twcap = ValueWriter.CreateArrayCap(cap, MemoryManager, value);
        return SetCap(ref twcap);
    }

    /// <summary>
    /// A cap value setter for all kinds of container types.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public STS SetCap<TValue>(CAP cap, ValueContainer<TValue> value) where TValue : struct
    {
        switch (value.ContainerType)
        {
            case TWON.ONEVALUE:
                return SetCap(cap, value.OneValue);
            case TWON.ENUMERATION:
                if (value.EnumValue == null)
                    throw new ArgumentException("EnumValue cannot be null when ContainerType is ENUMERATION.", nameof(value));
                return SetCap(cap, value.EnumValue);
            case TWON.RANGE:
                if (value.RangeValue == null)
                    throw new ArgumentException("RangeValue cannot be null when ContainerType is RANGE.", nameof(value));
                return SetCap(cap, value.RangeValue);
            case TWON.ARRAY:
                if (value.ArrayValue == null)
                    throw new ArgumentException("ArrayValue cannot be null when ContainerType is ARRAY.", nameof(value));
                return SetCap(cap, value.ArrayValue);
            default:
                throw new ArgumentException("Unsupported ContainerType for setting CAP.", nameof(value));
        }
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
        if (_currentDs == null) return STS.SequenceError();

        var rc = DGControl.Capability.SetConstraint(_appIdentity, _currentDs, ref value);
        value.Free(MemoryManager);
        return WrapInSTS(rc);
    }


    /// <summary>
    /// A simpler cap constraint setter for one-value type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetConstraint<TValue>(CAP cap, TValue value) where TValue : struct
    {
        var twcap = ValueWriter.CreateOneValueCap(cap, MemoryManager, value);
        return SetConstraint(ref twcap);
    }

    /// <summary>
    /// A cap constraint setter for enumeration container type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetConstraint<TValue>(CAP cap, Enumeration<TValue> value) where TValue : struct
    {
        var twcap = ValueWriter.CreateEnumCap(cap, MemoryManager, value);
        return SetConstraint(ref twcap);
    }

    /// <summary>
    /// A cap constraint setter for range container type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetConstraint<TValue>(CAP cap, Range<TValue> value) where TValue : struct
    {
        var twcap = ValueWriter.CreateRangeCap(cap, MemoryManager, value);
        return SetConstraint(ref twcap);
    }

    /// <summary>
    /// A cap constraint setter for array container type.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetConstraint<TValue>(CAP cap, IList<TValue> value) where TValue : struct
    {
        var twcap = ValueWriter.CreateArrayCap(cap, MemoryManager, value);
        return SetConstraint(ref twcap);
    }

    /// <summary>
    /// A cap constraint setter for all kinds of container types.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public STS SetConstraint<TValue>(CAP cap, ValueContainer<TValue> value) where TValue : struct
    {
        switch (value.ContainerType)
        {
            case TWON.ONEVALUE:
                return SetConstraint(cap, value.OneValue);
            case TWON.ENUMERATION:
                if (value.EnumValue == null)
                    throw new ArgumentException("EnumValue cannot be null when ContainerType is ENUMERATION.", nameof(value));
                return SetConstraint(cap, value.EnumValue);
            case TWON.RANGE:
                if (value.RangeValue == null)
                    throw new ArgumentException("RangeValue cannot be null when ContainerType is RANGE.", nameof(value));
                return SetConstraint(cap, value.RangeValue);
            case TWON.ARRAY:
                if (value.ArrayValue == null)
                    throw new ArgumentException("ArrayValue cannot be null when ContainerType is ARRAY.", nameof(value));
                return SetConstraint(cap, value.ArrayValue);
            default:
                throw new ArgumentException("Unsupported ContainerType for setting CAP constraint.", nameof(value));
        }
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
        if (_currentDs == null) return STS.SequenceError();

        var rc = DGControl.Capability.Reset(_appIdentity, _currentDs, ref value);

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
    public STS ResetCap<TValue>(CAP cap, out List<TValue> value) where TValue : struct
    {
        value = [];
        var sts = ResetCap(cap, out TW_CAPABILITY twcap);

        if (sts.RC == TWRC.SUCCESS)
        {
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    value.Add(twcap.ReadOneValue<TValue>(MemoryManager));
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumeration<TValue>(MemoryManager);
                    if (twenum.Items != null && twenum.CurrentIndex < twenum.Items.Length)
                    {
                        value.Add(twenum.Items[twenum.CurrentIndex]);
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRange<TValue>(MemoryManager);
                    if (range != null) value.Add(range.CurrentValue);
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArray<TValue>(MemoryManager);
                    if (twarr != null && twarr.Count > 0) value.AddRange(twarr);
                    break;
                default:
                    twcap.Free(MemoryManager); break;
            }
        }
        return sts;
    }

    /// <summary>
    /// Resets a CAP's current value to power-on default.
    /// </summary>
    /// <param name="cap"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS ResetCapBoxed(CAP cap, out List<object> value)
    {
        value = [];
        var sts = ResetCap(cap, out TW_CAPABILITY twcap);

        if (sts.RC == TWRC.SUCCESS)
        {
            switch (twcap.ConType)
            {
                case TWON.ONEVALUE:
                    var read = twcap.ReadOneValueBoxed(MemoryManager);
                    if (read != null) value.Add(read);
                    break;
                case TWON.ENUMERATION:
                    var twenum = twcap.ReadEnumerationBoxed(MemoryManager);
                    if (twenum.Items != null && twenum.CurrentIndex < twenum.Items.Length)
                    {
                        value.Add(twenum.Items[twenum.CurrentIndex]);
                    }
                    break;
                case TWON.RANGE:
                    var range = twcap.ReadRangeBoxed(MemoryManager);
                    if (range != null) value.Add(range.CurrentValue);
                    break;
                case TWON.ARRAY:
                    var twarr = twcap.ReadArrayBoxed(MemoryManager);
                    if (twarr != null && twarr.Count > 0) value.AddRange(twarr);
                    break;
                default:
                    twcap.Free(MemoryManager); break;
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
        if (_currentDs == null) return STS.SequenceError();

        // no memory is allocated for this
        var value = new TW_CAPABILITY(CAP.CAP_SUPPORTEDCAPS);
        var rc = DGControl.Capability.ResetAll(_appIdentity, _currentDs, ref value);

        if (rc == TWRC.SUCCESS)
        {
            RefreshCapLanguage();
        }

        return WrapInSTS(rc);
    }

    private void RefreshCapLanguage()
    {
        var rc2 = GetCapCurrent(CAP.CAP_LANGUAGE, out List<TWLG> lang);
        if (rc2.RC == TWRC.SUCCESS && lang.Count > 0)
        {
            Language.Set(lang.First());
        }
    }
}
