using System;
using System.Collections.Generic;
using System.IO;
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
        public static TValue ReadOneValue<TValue>(TWAIN twain, TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
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
                if (freeMemory) twain.DsmMemFree(ref cap.hContainer);
            }
        }
        public static IList<TValue> ReadEnumeration<TValue>(TWAIN twain, TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
        {
            throw new NotImplementedException();
        }
        public static IList<TValue> ReadArray<TValue>(TWAIN twain, TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
        {
            var list = new List<TValue>();

            if (cap.hContainer == IntPtr.Zero) return list;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                TWTY itemType;
                uint count;

                // Mac has a level of indirection and a different structure (ick)...
                if (PlatformTools.GetPlatform() == Platform.MACOSX)
                {
                    // Crack the container...
                    TW_ARRAY_MACOSX twarraymacosx = default(TW_ARRAY_MACOSX);
                    twarraymacosx = MarshalTo<TW_ARRAY_MACOSX>(lockedPtr);
                    itemType = (TWTY)twarraymacosx.ItemType;
                    count = twarraymacosx.NumItems;
                    lockedPtr += Marshal.SizeOf(twarraymacosx);
                }
                else
                {
                    // Crack the container...
                    TW_ARRAY twarray = default(TW_ARRAY);
                    twarray = MarshalTo<TW_ARRAY>(lockedPtr);
                    itemType = twarray.ItemType;
                    count = twarray.NumItems;
                    lockedPtr += Marshal.SizeOf(twarray);
                }

                for (var i = 0; i < count; i++)
                {
                    list.Add(ReadContainerData<TValue>(lockedPtr, itemType, i));
                }
            }
            finally
            {
                twain.DsmMemUnlock(cap.hContainer);
                if (freeMemory) twain.DsmMemFree(ref cap.hContainer);
            }
            return list;
        }
        public static (TValue defaultVal, TValue currentVal, IEnumerable<TValue> values)
            ReadRange<TValue>(TWAIN twain, TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read the one value of a cap as string. Only STR* and HANDLE types are supported.
        /// </summary>
        /// <param name="twain"></param>
        /// <param name="cap"></param>
        /// <param name="freeMemory"></param>
        /// <returns></returns>
        public static string ReadOneString(TWAIN twain, TW_CAPABILITY cap, bool freeMemory = true)
        {
            if (cap.hContainer == IntPtr.Zero) return null;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                if (cap.ConType == TWON.ONEVALUE)
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

                    switch (itemType)
                    {
                        case TWTY.STR32:
                            return MarshalTo<TW_STR32>(lockedPtr).ToString();
                        case TWTY.STR64:
                            return MarshalTo<TW_STR64>(lockedPtr).ToString();
                        case TWTY.STR128:
                            return MarshalTo<TW_STR128>(lockedPtr).ToString();
                        case TWTY.STR255:
                            return MarshalTo<TW_STR255>(lockedPtr).ToString();
                        case TWTY.HANDLE:
                            // null-terminated and encoded string.
                            // good chance this ain't right.
                            using (var stream = new MemoryStream())
                            {
                                byte read = Marshal.ReadByte(lockedPtr);
                                while (read != 0)
                                {
                                    stream.WriteByte(read);
                                    read = Marshal.ReadByte(lockedPtr);
                                    lockedPtr += 1;
                                }
                                // which one?
                                return Encoding.Unicode.GetString(Encoding.Convert(Language.GetEncoding(), Encoding.Unicode, stream.ToArray()));
                                //return Language.GetEncoding().GetString(stream.ToArray());
                            }
                    }
                }
            }
            finally
            {
                twain.DsmMemUnlock(cap.hContainer);
                if (freeMemory) twain.DsmMemFree(ref cap.hContainer);
            }
            return null;
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
