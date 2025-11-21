using NTwain.Data;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NTwain.Caps;

public record ValueContainer<TValue>
{
    public TWON ContainerType { get; set; }

    public TValue? OneValue { get; set; }

    public IList<TValue>? ArrayValue { get; set; }

    public EnumValue<TValue>? EnumValue { get; set; }

    public RangeValue<TValue>? RangeValue { get; set; }

    public IEnumerable<TValue> GetValues()
    {
        return ContainerType switch
        {
            TWON.ONEVALUE => ToEnumerable(OneValue),
            TWON.ARRAY => ArrayValue ?? [],
            TWON.ENUMERATION => EnumValue?.Items ?? [],
            TWON.RANGE => RangeValue != null ? GenerateRangeValues(RangeValue) : [],
            _ => [],
        };
    }

    private IEnumerable<TValue> ToEnumerable(TValue? value)
    {
        if (value == null) yield break;
        yield return value;
    }

    private IEnumerable<TValue> GenerateRangeValues(RangeValue<TValue> range)
    {
        var dynamicType = typeof(DynamicEnumerator<>);
        var genericType = dynamicType.MakeGenericType(typeof(TValue));

        var de = Activator.CreateInstance(genericType, range.Min, range.Max, range.Step) as IEnumerator;
        if (de == null) yield break;
        while (de.MoveNext())
        {
            yield return (TValue)de.Current;
        }
    }
}

public record EnumValue<TValue>
{
    public TValue[] Items { get; set; } = [];

    public int CurrentIndex { get; set; }

    public int DefaultIndex { get; set; }
}

public record RangeValue<TValue>
{
    public TValue Min { get; set; }

    public TValue Max { get; set; }

    public TValue Step { get; set; }

    public TValue DefaultValue;

    public TValue CurrentValue;
}
