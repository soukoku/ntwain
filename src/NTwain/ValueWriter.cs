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
    /// Contains methods for writing vairous things to pointers.
    /// </summary>
    public static class ValueWriter
    {

        public static void WriteOneValue<TValue>(TWAIN twain, ref TW_CAPABILITY twCap, TValue value) where TValue : struct
        {
            IntPtr lockedPtr = IntPtr.Zero;
            try
            {
                TWTY itemType = GetItemType<TValue>();
                if (PlatformTools.GetPlatform() == Platform.MACOSX)
                {
                    twCap.hContainer = twain.DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE_MACOSX)) + Marshal.SizeOf(default(TW_STR255))));
                    lockedPtr = twain.DsmMemLock(twCap.hContainer);

                    TW_ONEVALUE_MACOSX twonevaluemacosx = default(TW_ONEVALUE_MACOSX);
                    twonevaluemacosx.ItemType = (uint)itemType;
                    Marshal.StructureToPtr(twonevaluemacosx, lockedPtr, true);

                    lockedPtr += Marshal.SizeOf(twonevaluemacosx);
                }
                else
                {
                    twCap.hContainer = twain.DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE)) + Marshal.SizeOf(default(TW_STR255))));
                    lockedPtr = twain.DsmMemLock(twCap.hContainer);

                    TW_ONEVALUE twonevalue = default(TW_ONEVALUE);
                    twonevalue.ItemType = itemType;
                    Marshal.StructureToPtr(twonevalue, lockedPtr, true);

                    lockedPtr += Marshal.SizeOf(twonevalue);
                }

                WriteContainerData(lockedPtr, itemType, value, 0);
            }
            finally
            {
                if (lockedPtr != IntPtr.Zero) twain.DsmMemUnlock(twCap.hContainer);
            }
        }

        public static void WriteArray<TValue>(TWAIN twain, ref TW_CAPABILITY twCap, TValue[] values) where TValue : struct
        {
            throw new NotImplementedException();
        }
        public static void WriteRange<TValue>(TWAIN twain, ref TW_CAPABILITY twCap, Range<TValue> value) where TValue : struct
        {
            throw new NotImplementedException();
        }

        public static void WriteEnum<TValue>(TWAIN twain, ref TW_CAPABILITY twCap, Enumeration<TValue> value) where TValue : struct
        {
            throw new NotImplementedException();
        }


        static TWTY GetItemType<TValue>() where TValue : struct
        {
            var type = typeof(TValue);
            if (type == typeof(BoolType)) return TWTY.BOOL;
            if (type == typeof(TW_FIX32)) return TWTY.FIX32;
            if (type == typeof(TW_STR32)) return TWTY.STR32;
            if (type == typeof(TW_STR64)) return TWTY.STR64;
            if (type == typeof(TW_STR128)) return TWTY.STR128;
            if (type == typeof(TW_STR255)) return TWTY.STR255;
            if (type == typeof(TW_FRAME)) return TWTY.FRAME;

            if (type.IsEnum)
            {
                type = type.GetEnumUnderlyingType();
            }

            if (type == typeof(ushort)) return TWTY.UINT16;
            if (type == typeof(short)) return TWTY.INT16;
            if (type == typeof(uint)) return TWTY.UINT32;
            if (type == typeof(int)) return TWTY.INT32;
            if (type == typeof(byte)) return TWTY.UINT8;
            if (type == typeof(sbyte)) return TWTY.INT8;

            throw new NotSupportedException($"{type.Name} is not supported for writing.");
        }

        /// <summary>
        /// Writes single piece of value to the container pointer.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="intptr">A locked pointer to the container's data pointer. If data is array this is the 0th item.</param>
        /// <param name="type">The twain type.</param>
        /// <param name="value"></param>
        /// <param name="itemIndex">Index of the item if pointer is array.</param>
        static void WriteContainerData<TValue>(IntPtr intptr, TWTY type, TValue value, int itemIndex) where TValue : struct
        {
            switch (type)
            {
                default:
                    throw new NotSupportedException($"Unsupported item type {type} for writing.");
                // TODO: for small types needs to fill whole int32 before writing?
                case TWTY.INT8:
                    intptr += 1 * itemIndex;
                    //int intval = Convert.ToSByte(value);
                    //Marshal.StructureToPtr(intval, intptr, false);
                    Marshal.StructureToPtr(Convert.ToSByte(value), intptr, false);
                    break;
                case TWTY.UINT8:
                    intptr += 1 * itemIndex;
                    //uint uintval = Convert.ToByte(value);
                    //Marshal.StructureToPtr(uintval, intptr, false);
                    Marshal.StructureToPtr(Convert.ToByte(value), intptr, false);
                    break;
                case TWTY.INT16:
                    intptr += 2 * itemIndex;
                    //intval = Convert.ToInt16(value);
                    //Marshal.StructureToPtr(intval, intptr, false);
                    Marshal.StructureToPtr(Convert.ToInt16(value), intptr, false);
                    break;
                case TWTY.BOOL:
                case TWTY.UINT16:
                    intptr += 2 * itemIndex;
                    //uintval = Convert.ToUInt16(value);
                    //Marshal.StructureToPtr(uintval, intptr, false);
                    Marshal.StructureToPtr(Convert.ToUInt16(value), intptr, false);
                    break;
                case TWTY.INT32:
                case TWTY.UINT32:
                    intptr += 4 * itemIndex;
                    Marshal.StructureToPtr(value, intptr, false);
                    break;
                case TWTY.FIX32:
                    intptr += 4 * itemIndex;
                    Marshal.StructureToPtr(value, intptr, false);
                    break;
                case TWTY.FRAME:
                    intptr += 16 * itemIndex;
                    Marshal.StructureToPtr(value, intptr, false);
                    break;
                case TWTY.STR32:
                    intptr += TW_STR32.Size * itemIndex;
                    Marshal.StructureToPtr(value, intptr, false);
                    break;
                case TWTY.STR64:
                    intptr += TW_STR64.Size * itemIndex;
                    Marshal.StructureToPtr(value, intptr, false);
                    break;
                case TWTY.STR128:
                    intptr += TW_STR128.Size * itemIndex;
                    Marshal.StructureToPtr(value, intptr, false);
                    break;
                case TWTY.STR255:
                    intptr += TW_STR255.Size * itemIndex;
                    Marshal.StructureToPtr(value, intptr, false);
                    break;
            }
        }
    }
}
