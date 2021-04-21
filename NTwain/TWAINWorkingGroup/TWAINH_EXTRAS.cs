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
        public override string ToString()
        {
            return Get();
        }
    }

    partial struct TW_STR64
    {
        public override string ToString()
        {
            return Get();
        }
    }

    partial struct TW_STR128
    {
        public override string ToString()
        {
            return Get();
        }
    }

    partial struct TW_STR255
    {
        public override string ToString()
        {
            return Get();
        }
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
