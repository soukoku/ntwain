using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Contains methods for reading pointers into various things.
    /// </summary>
    public static class ValueReader
    {
        public static TValue ReadOneValue<TValue>(TWAIN twain, TW_CAPABILITY cap) where TValue : struct
        {
            if (cap.hContainer == IntPtr.Zero) return default(TValue);

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                TWTY itemType;
                // Mac has a level of indirection and a different structure (ick)...
                if (PlatformTools.GetPlatform() == Platform.MACOSX)
                {
                    // Crack the container...
                    var onevalue = MarshalTo<TW_ONEVALUE_MACOSX>(lockedPtr);
                    itemType = (TWTY)onevalue.ItemType;
                    lockedPtr += Marshal.SizeOf(onevalue);
                }
                else
                {
                    // Crack the container...
                    var onevalue = MarshalTo<TW_ONEVALUE>(lockedPtr);
                    itemType = onevalue.ItemType;
                    lockedPtr += Marshal.SizeOf(onevalue);
                }

                return ReadContainerData<TValue>(lockedPtr, itemType);
            }
            finally
            {
                twain.DsmMemUnlock(cap.hContainer);
            }
        }
        public static IList<TValue> ReadEnumeration<TValue>(TWAIN twain, TW_CAPABILITY twCap) where TValue : struct
        {
            throw new NotImplementedException();
        }
        public static IList<TValue> ReadArray<TValue>(TWAIN twain, TW_CAPABILITY twCap) where TValue : struct
        {
            throw new NotImplementedException();
        }
        public static (TValue defaultVal, TValue currentVal, IEnumerable<TValue> values) ReadRange<TValue>(TWAIN twain, TW_CAPABILITY twCap) where TValue : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read the container pointer content.
        /// </summary>
        /// <param name="intptr">A locked pointer to the container's data pointer. If data is array this is the 0th item.</param>
        /// <param name="type">The twain type.</param>
        /// <param name="itemIndex">Index of the item if pointer is array.</param>
        /// <returns></returns>
        static TValue ReadContainerData<TValue>(IntPtr intptr, TWTY type, int itemIndex = 0) where TValue : struct
        {
            var isEnum = typeof(TValue).IsEnum;

            switch (type)
            {
                default:
                    throw new NotSupportedException($"Unsupported item type {type} for reading.");
                case TWTY.INT8:
                    intptr += 1 * itemIndex;
                    if (isEnum)
                    {
                        return NumericToEnum<sbyte, TValue>(MarshalTo<sbyte>(intptr));
                    }
                    return MarshalTo<TValue>(intptr);
                case TWTY.UINT8:
                    intptr += 1 * itemIndex;
                    if (isEnum)
                    {
                        return NumericToEnum<byte, TValue>(MarshalTo<byte>(intptr));
                    }
                    return MarshalTo<TValue>(intptr);
                case TWTY.INT16:
                    intptr += 2 * itemIndex;
                    if (isEnum)
                    {
                        return NumericToEnum<short, TValue>(MarshalTo<short>(intptr));
                    }
                    return MarshalTo<TValue>(intptr);
                case TWTY.BOOL:
                case TWTY.UINT16:
                    intptr += 2 * itemIndex;
                    if (isEnum)
                    {
                        return NumericToEnum<ushort, TValue>(MarshalTo<ushort>(intptr));
                    }
                    return MarshalTo<TValue>(intptr);
                case TWTY.INT32:
                    intptr += 4 * itemIndex;
                    if (isEnum)
                    {
                        return NumericToEnum<int, TValue>(MarshalTo<int>(intptr));
                    }
                    return MarshalTo<TValue>(intptr);
                case TWTY.UINT32:
                    intptr += 4 * itemIndex;
                    if (isEnum)
                    {
                        return NumericToEnum<uint, TValue>(MarshalTo<uint>(intptr));
                    }
                    return MarshalTo<TValue>(intptr);
                case TWTY.FIX32:
                    intptr += 4 * itemIndex;
                    return MarshalTo<TValue>(intptr);
                case TWTY.FRAME:
                    intptr += 16 * itemIndex;
                    return MarshalTo<TValue>(intptr);
                case TWTY.STR32:
                    intptr += TW_STR32.Size * itemIndex;
                    return MarshalTo<TValue>(intptr);
                case TWTY.STR64:
                    intptr += TW_STR64.Size * itemIndex;
                    return MarshalTo<TValue>(intptr);
                case TWTY.STR128:
                    intptr += TW_STR128.Size * itemIndex;
                    return MarshalTo<TValue>(intptr);
                case TWTY.STR255:
                    intptr += TW_STR255.Size * itemIndex;
                    return MarshalTo<TValue>(intptr);
            }
        }

        static TEnum NumericToEnum<TNumber, TEnum>(TNumber num) where TEnum : struct
        {
            // some caps returns a data type that's not the underlying datatype for the enum 
            // so best way is to ToString() it and parse it as the enum type.
            var str = num.ToString();

            if (Enum.TryParse(str, out TEnum parsed))
            {
                return parsed;
            }
            return default(TEnum);
        }

        static T MarshalTo<T>(IntPtr ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T));
    }
}
