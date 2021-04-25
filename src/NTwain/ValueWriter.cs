using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Contains methods for writing vairous things to pointers.
    /// </summary>
    public static class ValueWriter
    {

        public static void WriteOneValue<TValue>(TWAIN twain, TW_CAPABILITY twCap, TValue value) where TValue : struct
        {
            throw new NotImplementedException();
        }
        public static void WriteArray<TValue>(TWAIN twain, TW_CAPABILITY twCap, TValue[] values) where TValue : struct
        {
            throw new NotImplementedException();
        }
        public static void WriteRange<TValue>(TWAIN twain, TW_CAPABILITY twCap, Range<TValue> value) where TValue : struct
        {
            throw new NotImplementedException();
        }

        public static void WriteEnum<TValue>(TWAIN twain, TW_CAPABILITY twCap, Enumeration<TValue> value) where TValue : struct
        {
            throw new NotImplementedException();
        }

    }
}
