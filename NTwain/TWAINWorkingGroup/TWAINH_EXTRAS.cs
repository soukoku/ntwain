using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWAINWorkingGroup
{
    // contains my additions

    partial struct TW_FIX32 : IEquatable<TW_FIX32>
    {
        // the conversion logic is found in the spec.

        float ToFloat()
        {
            return (float)Whole + Frac / 65536f;
        }
        TW_FIX32(float value)
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

        public static implicit operator float(TW_FIX32 value) => value.ToFloat();
        public static implicit operator TW_FIX32(float value) => new TW_FIX32(value);

        public static implicit operator double(TW_FIX32 value) => value.ToFloat();
        public static implicit operator TW_FIX32(double value) => new TW_FIX32((float)value);

        public static bool operator ==(TW_FIX32 value1, TW_FIX32 value2) => value1.Equals(value2);
        public static bool operator !=(TW_FIX32 value1, TW_FIX32 value2) => !value1.Equals(value2);
    }

    partial struct TW_FRAME : IEquatable<TW_FRAME>
    {
        public override string ToString()
        {
            return $"L={Left}, T={Top}, R={Right}, B={Bottom}";
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
