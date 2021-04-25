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
    /// Contains extension methods for reading pointers into various things.
    /// </summary>
    public static class ValueReader
    {
        /// <summary>
        /// Read capability's container content as ToString()'d one value.
        /// </summary>
        /// <param name="twain">Low-level twain object.</param>
        /// <param name="cap">Cap to read from.</param>
        /// <returns></returns>
        public static string CapabilityOneValueToString(this TWAIN twain, TW_CAPABILITY cap)
        {
            if (cap.hContainer == IntPtr.Zero) return null;

            var lockedPtr = twain.DsmMemLock(cap.hContainer);

            try
            {
                TWTY itemType;
                // Mac has a level of indirection and a different structure (ick)...
                if (PlatformTools.GetPlatform() == Platform.MACOSX)
                {
                    // Crack the container...
                    var onevalue = lockedPtr.MarshalTo<TW_ONEVALUE_MACOSX>();
                    itemType = (TWTY)onevalue.ItemType;
                    lockedPtr += Marshal.SizeOf(onevalue);
                }
                else
                {
                    // Crack the container...
                    var onevalue = lockedPtr.MarshalTo<TW_ONEVALUE>();
                    itemType = onevalue.ItemType;
                    lockedPtr += Marshal.SizeOf(onevalue);
                }

                return lockedPtr.ContainerToString(itemType);
            }
            finally
            {
                // All done...
                twain.DsmMemUnlock(cap.hContainer);
            }
        }

        /// <summary>
        /// Read the container pointer content as a string. Numeric values are ToString()'d.
        /// </summary>
        /// <param name="intptr">A locked pointer to the container data. If data is array this is the 0th item.</param>
        /// <param name="type">The twain type.</param>
        /// <param name="itemIndex">Index of the item if pointer is array.</param>
        /// <returns></returns>
        static string ContainerToString(this IntPtr intptr, TWTY type, int itemIndex = 0)
        {
            switch (type)
            {
                default:
                    throw new NotSupportedException($"Unknown item type {type} to read as string.");
                case TWTY.INT8:
                    intptr += 1 * itemIndex;
                    return intptr.MarshalToString<sbyte>();
                case TWTY.INT16:
                    intptr += 2 * itemIndex;
                    return intptr.MarshalToString<short>();
                case TWTY.INT32:
                    intptr += 4 * itemIndex;
                    return intptr.MarshalToString<int>();
                case TWTY.UINT8:
                    intptr += 1 * itemIndex;
                    return intptr.MarshalToString<byte>();
                case TWTY.BOOL:
                case TWTY.UINT16:
                    intptr += 2 * itemIndex;
                    return intptr.MarshalToString<ushort>();
                case TWTY.UINT32:
                    intptr += 4 * itemIndex;
                    return intptr.MarshalToString<uint>();
                case TWTY.FIX32:
                    intptr += 4 * itemIndex;
                    return intptr.MarshalToString<TW_FIX32>();
                case TWTY.FRAME:
                    intptr += 16 * itemIndex;
                    return intptr.MarshalToString<TW_FRAME>();
                case TWTY.STR32:
                    intptr += TW_STR32.Size * itemIndex;
                    return intptr.MarshalToString<TW_STR32>();
                case TWTY.STR64:
                    intptr += TW_STR64.Size * itemIndex;
                    return intptr.MarshalToString<TW_STR64>();
                case TWTY.STR128:
                    intptr += TW_STR128.Size * itemIndex;
                    return intptr.MarshalToString<TW_STR128>();
                case TWTY.STR255:
                    intptr += TW_STR255.Size * itemIndex;
                    return intptr.MarshalToString<TW_STR255>();
                case TWTY.HANDLE:
                    intptr += IntPtr.Size * itemIndex;
                    return Marshal.ReadIntPtr(intptr).ToString();
            }
        }

        static string MarshalToString<T>(this IntPtr ptr) => Marshal.PtrToStructure(ptr, typeof(T)).ToString();
        static T MarshalTo<T>(this IntPtr ptr) => (T)Marshal.PtrToStructure(ptr, typeof(T));
    }
}
