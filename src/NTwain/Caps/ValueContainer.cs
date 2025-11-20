using NTwain.Data;
using System.Collections.Generic;
using System.Linq;

namespace NTwain.Caps;

public record ValueContainer<TValue> where TValue : struct
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
            TWON.ONEVALUE => OneValue.HasValue ? ToEnumerable(OneValue.Value) : [],
            TWON.ARRAY => ArrayValue ?? [],
            TWON.ENUMERATION => EnumValue?.Items ?? [],
            TWON.RANGE => RangeValue != null ? GenerateRangeValues(RangeValue) : [],
            _ => [],
        };
    }

    private IEnumerable<TValue> ToEnumerable(TValue value)
    {
        yield return value;
    }

    private IEnumerable<TValue> GenerateRangeValues(RangeValue<TValue> range)
    {
        var de = new DynamicEnumerator<TValue>(range.Min, range.Max, range.Step);
        while (de.MoveNext())
        {
            yield return de.Current;
        }
    }
}

public record EnumValue<TValue> where TValue : struct
{
    public TValue[] Items { get; set; } = [];

    public int CurrentIndex { get; set; }

    public int DefaultIndex { get; set; }
}

public record RangeValue<TValue> where TValue : struct
{
    public TValue Min { get; set; }

    public TValue Max { get; set; }

    public TValue Step { get; set; }

    public TValue DefaultValue;

    public TValue CurrentValue;
}
