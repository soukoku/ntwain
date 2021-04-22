using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWAINWorkingGroup
{
    // contains my additions

    partial struct TW_STR32
    {
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
}
