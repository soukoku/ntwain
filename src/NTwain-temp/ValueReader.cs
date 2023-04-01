using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;
using static TWAINWorkingGroup.TWAIN;

namespace NTwain
{
    /// <summary>
    /// Contains methods for reading pointers into various things.
    /// </summary>
    public static class ValueReader
    {
        /// <summary>
        /// Reads pointer as UTF8 string.
        /// </summary>
        /// <param name="intPtr">Pointer to string.</param>
        /// <param name="length">Number of bytes to read.</param>
        /// <returns></returns>
        public static unsafe string PtrToStringUTF8(IntPtr intPtr, int length)
        {
            if (intPtr == IntPtr.Zero) throw new ArgumentNullException(nameof(intPtr));
            if (length == 0) throw new ArgumentOutOfRangeException(nameof(length), length, "Length must be greater than 0.");

            //// safe method with 2 copies
            //var bytes = new byte[length];
            //Marshal.Copy(intPtr, bytes, 0, length);
            //return Encoding.UTF8.GetString(bytes);

            // unsafe method with 1 copy (does it work?)
            sbyte* bytes = (sbyte*)intPtr;
            var str = new string(bytes, 0, length, Encoding.UTF8);
            return str;
        }



        // most of these are modified from the original TWAIN.CapabilityToCsv()

        public static TValue ReadOneValueContainer<TValue>(TWAIN twain, ref TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
        {
            if (cap.hContainer == IntPtr.Zero) return default;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                TWTY itemType;
                // Mac has a level of indirection and a different structure (ick)...
                if (TWAIN.GetPlatform() == Platform.MACOSX)
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

                return ReadContainerData<TValue>(lockedPtr, itemType, 0);
            }
            finally
            {
                if (lockedPtr != IntPtr.Zero) twain.DsmMemUnlock(cap.hContainer);
                if (freeMemory) twain.DsmMemFree(ref cap.hContainer);
            }
        }
        public static Enumeration<TValue> ReadEnumerationContainer<TValue>(TWAIN twain, ref TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
        {
            Enumeration<TValue> retVal = new Enumeration<TValue>();

            if (cap.hContainer == IntPtr.Zero) return retVal;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                TWTY itemType;
                int count = 0;

                // Mac has a level of indirection and a different structure (ick)...
                if (TWAIN.GetPlatform() == Platform.MACOSX)
                {
                    // Crack the container...
                    var twenumerationmacosx = MarshalTo<TW_ENUMERATION_MACOSX>(lockedPtr);
                    itemType = (TWTY)twenumerationmacosx.ItemType;
                    count = (int)twenumerationmacosx.NumItems;
                    retVal.DefaultIndex = (int)twenumerationmacosx.DefaultIndex;
                    retVal.CurrentIndex = (int)twenumerationmacosx.CurrentIndex;
                    lockedPtr += Marshal.SizeOf(twenumerationmacosx);
                }
                // Windows or the 2.4+ Linux DSM...
                else if (TWAIN.GetPlatform() == Platform.WINDOWS || ((twain.m_blFoundLatestDsm || twain.m_blFoundLatestDsm64) && (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm)))
                {
                    // Crack the container...
                    var twenumeration = MarshalTo<TW_ENUMERATION>(lockedPtr);
                    itemType = twenumeration.ItemType;
                    count = (int)twenumeration.NumItems;
                    retVal.DefaultIndex = (int)twenumeration.DefaultIndex;
                    retVal.CurrentIndex = (int)twenumeration.CurrentIndex;
                    lockedPtr += Marshal.SizeOf(twenumeration);
                }
                // The -2.3 Linux DSM...
                else if (twain.m_blFound020302Dsm64bit && (twain.m_linuxdsm == TWAIN.LinuxDsm.Is020302Dsm64bit))
                {
                    // Crack the container...
                    var twenumerationlinux64 = MarshalTo<TW_ENUMERATION_LINUX64>(lockedPtr);
                    itemType = twenumerationlinux64.ItemType;
                    count = (int)twenumerationlinux64.NumItems;
                    retVal.DefaultIndex = (int)twenumerationlinux64.DefaultIndex;
                    retVal.CurrentIndex = (int)twenumerationlinux64.CurrentIndex;
                    lockedPtr += Marshal.SizeOf(twenumerationlinux64);
                }
                // This shouldn't be possible, but what the hey...
                else
                {
                    Log.Error("This is serious, you win a cookie for getting here...");
                    return retVal;
                }

                retVal.Items = new TValue[count];

                for (var i = 0; i < count; i++)
                {
                    retVal.Items[i] = ReadContainerData<TValue>(lockedPtr, itemType, i);
                }
            }
            finally
            {
                if (lockedPtr != IntPtr.Zero) twain.DsmMemUnlock(cap.hContainer);
                if (freeMemory) twain.DsmMemFree(ref cap.hContainer);
            }
            return retVal;
        }
        public static IList<TValue> ReadArrayContainer<TValue>(TWAIN twain, ref TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
        {
            if (cap.hContainer == IntPtr.Zero) return EmptyArray<TValue>.Value;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                TWTY itemType;
                uint count;

                // Mac has a level of indirection and a different structure (ick)...
                if (TWAIN.GetPlatform() == Platform.MACOSX)
                {
                    // Crack the container...
                    var twarraymacosx = MarshalTo<TW_ARRAY_MACOSX>(lockedPtr);
                    itemType = (TWTY)twarraymacosx.ItemType;
                    count = twarraymacosx.NumItems;
                    lockedPtr += Marshal.SizeOf(twarraymacosx);
                }
                else
                {
                    // Crack the container...
                    var twarray = MarshalTo<TW_ARRAY>(lockedPtr);
                    itemType = twarray.ItemType;
                    count = twarray.NumItems;
                    lockedPtr += Marshal.SizeOf(twarray);
                }

                var arr = new TValue[count];
                for (var i = 0; i < count; i++)
                {
                    arr[i] = ReadContainerData<TValue>(lockedPtr, itemType, i);
                }
                return arr;
            }
            finally
            {
                if (lockedPtr != IntPtr.Zero) twain.DsmMemUnlock(cap.hContainer);
                if (freeMemory) twain.DsmMemFree(ref cap.hContainer);
            }
        }
        public static Range<TValue> ReadRangeContainer<TValue>(TWAIN twain, ref TW_CAPABILITY cap, bool freeMemory = true) where TValue : struct
        {
            var retVal = new Range<TValue>();

            if (cap.hContainer == IntPtr.Zero) return retVal;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                TW_RANGE twrange = default;
                TW_RANGE_FIX32 twrangefix32 = default;
                
                // Mac has a level of indirection and a different structure (ick)...
                if (TWAIN.GetPlatform() == Platform.MACOSX)
                {
                    var twrangemacosx = MarshalTo<TW_RANGE_MACOSX>(lockedPtr);
                    var twrangefix32macosx = MarshalTo<TW_RANGE_FIX32_MACOSX>(lockedPtr);
                    twrange.ItemType = (TWTY)twrangemacosx.ItemType;
                    twrange.MinValue = twrangemacosx.MinValue;
                    twrange.MaxValue = twrangemacosx.MaxValue;
                    twrange.StepSize = twrangemacosx.StepSize;
                    twrange.DefaultValue = twrangemacosx.DefaultValue;
                    twrange.CurrentValue = twrangemacosx.CurrentValue;
                    twrangefix32.ItemType = (TWTY)twrangefix32macosx.ItemType;
                    twrangefix32.MinValue = twrangefix32macosx.MinValue;
                    twrangefix32.MaxValue = twrangefix32macosx.MaxValue;
                    twrangefix32.StepSize = twrangefix32macosx.StepSize;
                    twrangefix32.DefaultValue = twrangefix32macosx.DefaultValue;
                    twrangefix32.CurrentValue = twrangefix32macosx.CurrentValue;
                }
                // Windows or the 2.4+ Linux DSM...
                else if (TWAIN.GetPlatform() == Platform.WINDOWS || (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm) ||
                    ((twain.m_blFoundLatestDsm || twain.m_blFoundLatestDsm64) && (twain.m_linuxdsm == TWAIN.LinuxDsm.IsLatestDsm)))
                {
                    twrange = MarshalTo<TW_RANGE>(lockedPtr);
                    twrangefix32 = MarshalTo<TW_RANGE_FIX32>(lockedPtr);
                }
                // The -2.3 Linux DSM...
                else
                {
                    var twrangelinux64 = MarshalTo<TW_RANGE_LINUX64>(lockedPtr);
                    var twrangefix32macosx = MarshalTo<TW_RANGE_FIX32_MACOSX>(lockedPtr);
                    twrange.ItemType = twrangelinux64.ItemType;
                    twrange.MinValue = (uint)twrangelinux64.MinValue;
                    twrange.MaxValue = (uint)twrangelinux64.MaxValue;
                    twrange.StepSize = (uint)twrangelinux64.StepSize;
                    twrange.DefaultValue = (uint)twrangelinux64.DefaultValue;
                    twrange.CurrentValue = (uint)twrangelinux64.CurrentValue;
                    twrangefix32.ItemType = (TWTY)twrangefix32macosx.ItemType;
                    twrangefix32.MinValue = twrangefix32macosx.MinValue;
                    twrangefix32.MaxValue = twrangefix32macosx.MaxValue;
                    twrangefix32.StepSize = twrangefix32macosx.StepSize;
                    twrangefix32.DefaultValue = twrangefix32macosx.DefaultValue;
                    twrangefix32.CurrentValue = twrangefix32macosx.CurrentValue;
                }

                switch (twrange.ItemType)
                {
                    // use dynamic since I know they fit the type.
                    case TWTY.FIX32:
                        retVal.MinValue = (dynamic)twrangefix32.MinValue;
                        retVal.MaxValue = (dynamic)twrangefix32.MaxValue;
                        retVal.StepSize = (dynamic)twrangefix32.StepSize;
                        retVal.CurrentValue = (dynamic)twrangefix32.CurrentValue;
                        retVal.DefaultValue = (dynamic)twrangefix32.DefaultValue;
                        break;
                    case TWTY.INT8:
                    case TWTY.UINT8:
                    case TWTY.INT16:
                    case TWTY.BOOL:
                    case TWTY.UINT16:
                    case TWTY.INT32:
                    case TWTY.UINT32:
                        retVal.MinValue = (dynamic)twrange.MinValue;
                        retVal.MaxValue = (dynamic)twrange.MaxValue;
                        retVal.StepSize = (dynamic)twrange.StepSize;
                        retVal.CurrentValue = (dynamic)twrange.CurrentValue;
                        retVal.DefaultValue = (dynamic)twrange.DefaultValue;
                        break;
                    default:
                        throw new NotSupportedException($"The value type {twrange.ItemType} is not supported as range.");

                }
                return retVal;
            }
            finally
            {
                if (lockedPtr != IntPtr.Zero) twain.DsmMemUnlock(cap.hContainer);
                if (freeMemory) twain.DsmMemFree(ref cap.hContainer);
            }
        }

        /// <summary>
        /// Read the one value of a cap as string. Only STR* and HANDLE types are supported.
        /// </summary>
        /// <param name="twain"></param>
        /// <param name="cap"></param>
        /// <param name="freeMemory"></param>
        /// <returns></returns>
        public static string ReadOneValueContainerString(TWAIN twain, TW_CAPABILITY cap, bool freeMemory = true)
        {
            if (cap.hContainer == IntPtr.Zero) return null;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                if (cap.ConType == TWON.ONEVALUE)
                {
                    TWTY itemType;
                    // Mac has a level of indirection and a different structure (ick)...
                    if (TWAIN.GetPlatform() == Platform.MACOSX)
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
                if (lockedPtr != IntPtr.Zero) twain.DsmMemUnlock(cap.hContainer);
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
        static TValue ReadContainerData<TValue>(IntPtr intptr, TWTY type, int itemIndex) where TValue : struct
        {
            var isEnum = typeof(TValue).IsEnum;

            switch (type)
            {
                default:
                    throw new NotSupportedException($"Unsupported item type {type} for reading.");
                // TODO: verify if needs to read int32 for small types
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
            return default;
        }

        static T MarshalTo<T>(IntPtr ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T));
    }
}
