using NTwain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWAINWorkingGroup
{
    // contains my additions that makes twain types easier to work with.

    /// <summary>
    /// TWAIN's boolean values.
    /// </summary>
    public enum BoolType : ushort
    {
        /// <summary>
        /// The false value (0).
        /// </summary>
        False = 0,
        /// <summary>
        /// The true value (1).
        /// </summary>
        True = 1
    }

    /// <summary>
    /// A more dotnet-friendly representation of <see cref="TW_ENUMERATION"/>.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Enumeration<TValue> where TValue : struct
    {
        public int CurrentIndex;

        public int DefaultIndex;

        public TValue[] Items;
    }

    /// <summary>
    /// A more dotnet-friendly representation of <see cref="TW_RANGE"/>.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public partial class Range<TValue> : IEnumerable<TValue> where TValue : struct
    {
        public TValue MinValue;
        public TValue MaxValue;
        public TValue StepSize;
        public TValue DefaultValue;
        public TValue CurrentValue;

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            if (!(MinValue is IConvertible))
                throw new NotSupportedException($"The value type {typeof(TValue).Name} is not supported for range enumeration.");

            return new DynamicEnumerator(MinValue, MaxValue, StepSize);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TValue>)this).GetEnumerator();
        }

        // dynamic is a cheap hack to sidestep the compiler restrictions if I know TValue is numeric
        class DynamicEnumerator : IEnumerator<TValue>
        {
            private readonly TValue _min;
            private readonly TValue _max;
            private readonly TValue _step;
            private TValue _cur;
            bool started = false;

            public DynamicEnumerator(TValue min, TValue max, TValue step)
            {
                _min = min;
                _max = max;
                _step = step;
                _cur = min;
            }

            public TValue Current => _cur;

            object IEnumerator.Current => this.Current;

            public void Dispose() { }

            public bool MoveNext()
            {
                if (!started)
                {
                    started = true;
                    return true;
                }

                var next = _cur + (dynamic)_step;
                if (next == _cur || next < _min || next > _max) return false;

                _cur = next;
                return true;
            }

            public void Reset()
            {
                _cur = _min;
                started = false;
            }
        }
    }

    partial struct TW_FIX32 : IEquatable<TW_FIX32>, IConvertible
    {
        // the conversion logic is found in the spec.

        float ToFloat()
        {
            return Whole + Frac / 65536f;
        }
        double ToDouble()
        {
            return Whole + Frac / 65536.0;
        }
        public TW_FIX32(double value)
        {
            Whole = (short)value;
            Frac = (ushort)((value - Whole) * 65536.0);
        }
        public TW_FIX32(float value)
        {
            //int temp = (int)(value * 65536.0 + 0.5);
            //Whole = (short)(temp >> 16);
            //Fraction = (ushort)(temp & 0x0000ffff);

            // different version from twain faq
            bool sign = value < 0;
            int temp = (int)(value * 65536.0 + (sign ? (-0.5) : 0.5));
            Whole = (short)(temp >> 16);
            Frac = (ushort)(temp & 0x0000ffff);
        }

        public override string ToString()
        {
            return ToFloat().ToString();
        }

        public bool Equals(TW_FIX32 other)
        {
            return Whole == other.Whole && Frac == other.Frac;
        }
        public override bool Equals(object obj)
        {
            if (obj is TW_FIX32 other)
            {
                return Equals(other);
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Whole ^ Frac;
        }


        #region IConvertable

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Single;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return this != 0;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte((float)this);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar((float)this);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime((float)this);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal((float)this);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble((float)this);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16((float)this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32((float)this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64((float)this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte((float)this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle((float)this);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType((float)this, conversionType, CultureInfo.InvariantCulture);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16((float)this);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32((float)this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64((float)this);
        }

        #endregion

        public static implicit operator float(TW_FIX32 value) => value.ToFloat();
        public static implicit operator TW_FIX32(float value) => new TW_FIX32(value);

        public static implicit operator double(TW_FIX32 value) => value.ToDouble();
        public static implicit operator TW_FIX32(double value) => new TW_FIX32((float)value);

        public static bool operator ==(TW_FIX32 value1, TW_FIX32 value2) => value1.Equals(value2);
        public static bool operator !=(TW_FIX32 value1, TW_FIX32 value2) => !value1.Equals(value2);
    }

    partial struct TW_FRAME : IEquatable<TW_FRAME>
    {
        /// <summary>
        /// Creates <see cref="TW_FRAME"/> from a string representation of it.
        /// </summary>
        /// <param name="value"></param>
        public TW_FRAME(string value) : this()
        {
            var parts = value.Split(',');
            if (parts.Length == 4)
            {
                Left = float.Parse(parts[0]);
                Top = float.Parse(parts[1]);
                Right = float.Parse(parts[2]);
                Bottom = float.Parse(parts[3]);
            }
            else
            {
                throw new ArgumentException($"Cannot create frame from \"{value}\".");
            }
        }

        /// <summary>
        /// String representation of Left,Top,Right,Bottom.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{Left},{Top},{Right},{Bottom}";
        }

        public bool Equals(TW_FRAME other)
        {
            return Left == other.Left && Top == other.Top &&
                Right == other.Right && Bottom == other.Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is TW_FRAME other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() ^ Top.GetHashCode() ^
                Right.GetHashCode() ^ Bottom.GetHashCode();
        }


        public static bool operator ==(TW_FRAME value1, TW_FRAME value2)
        {
            return value1.Equals(value2);
        }
        public static bool operator !=(TW_FRAME value1, TW_FRAME value2)
        {
            return !value1.Equals(value2);
        }
    }

    partial struct TW_STR32
    {
        public const int Size = 34;

        public TW_STR32(string value) : this()
        {
            Set(value);
        }

        public override string ToString()
        {
            return Get();
        }

        public static implicit operator string(TW_STR32 value) => value.ToString();
        public static explicit operator TW_STR32(string value) => new TW_STR32(value);

    }

    partial struct TW_STR64
    {
        public const int Size = 66;

        public TW_STR64(string value) : this()
        {
            Set(value);
        }

        public override string ToString()
        {
            return Get();
        }

        public static implicit operator string(TW_STR64 value) => value.ToString();
        public static explicit operator TW_STR64(string value) => new TW_STR64(value);
    }

    partial struct TW_STR128
    {
        public const int Size = 130;

        public TW_STR128(string value) : this()
        {
            Set(value);
        }

        public override string ToString()
        {
            return Get();
        }

        public static implicit operator string(TW_STR128 value) => value.ToString();
        public static explicit operator TW_STR128(string value) => new TW_STR128(value);
    }

    partial struct TW_STR255
    {
        public const int Size = 256;

        public TW_STR255(string value) : this()
        {
            Set(value);
        }

        public override string ToString()
        {
            return Get();
        }

        public static implicit operator string(TW_STR255 value) => value.ToString();
        public static explicit operator TW_STR255(string value) => new TW_STR255(value);
    }

    partial struct TW_IDENTITY
    {
        public override string ToString()
        {
            return $"{Manufacturer} - {ProductName} {Version} (TWAIN {ProtocolMajor}.{ProtocolMinor})";
        }
    }

    partial struct TW_VERSION
    {
        public override string ToString()
        {
            return $"{MajorNum}.{MinorNum}";
        }
    }

    partial struct TW_DEVICEEVENT
    {
        public TWDE Event { get { return (TWDE)_event; } }
        public TWFL FlashUsed2 { get { return (TWFL)_flashUsed2; } }
    }
}
