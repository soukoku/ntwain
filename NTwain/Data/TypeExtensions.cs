using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace NTwain.Data
{
    /// <summary>
    /// Contains extensions for reading/writing TWAIN types from/to pointer.
    /// </summary>
    public static class TypeExtensions
    {
        static readonly IDictionary<ItemType, int> __sizes = new Dictionary<ItemType, int>
        {
            {ItemType.Int8, 1},
            {ItemType.UInt8, 1},
            {ItemType.Int16, 2},
            {ItemType.UInt16, 2},
            {ItemType.Bool, 2},
            {ItemType.Int32, 4},
            {ItemType.UInt32, 4},
            {ItemType.Fix32, 4},
            {ItemType.Frame, 16},
            {ItemType.String32, TwainConst.String32},
            {ItemType.String64, TwainConst.String64},
            {ItemType.String128, TwainConst.String128},
            {ItemType.String255, TwainConst.String255},
            // TODO: find out if it should be fixed 4 bytes or intptr size
            {ItemType.Handle, IntPtr.Size},
        };

        /// <summary>
        /// Gets the byte size of the item type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetItemTypeSize(this ItemType type)
        {
            if (__sizes.ContainsKey(type))
            {
                return __sizes[type];
            }
            return 0;
        }


        /// <summary>
        /// Reads a TWAIN value.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="type">The TWAIN type.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public static object ReadValue(this IntPtr baseAddress, ref int offset, ItemType type)
        {
            object val = null;
            switch (type)
            {
                case ItemType.Int8:
                    val = (sbyte)Marshal.ReadByte(baseAddress, offset);
                    break;
                case ItemType.UInt8:
                    val = Marshal.ReadByte(baseAddress, offset);
                    break;
                case ItemType.Bool:
                case ItemType.UInt16:
                    val = (ushort)Marshal.ReadInt16(baseAddress, offset);
                    break;
                case ItemType.Int16:
                    val = Marshal.ReadInt16(baseAddress, offset);
                    break;
                case ItemType.UInt32:
                    val = (uint)Marshal.ReadInt32(baseAddress, offset);
                    break;
                case ItemType.Int32:
                    val = Marshal.ReadInt32(baseAddress, offset);
                    break;
                case ItemType.Fix32:
                    TWFix32 f32 = new TWFix32();
                    f32.Whole = Marshal.ReadInt16(baseAddress, offset);
                    f32.Fraction = (ushort)Marshal.ReadInt16(baseAddress, offset + 2);
                    val = f32;
                    break;
                case ItemType.Frame:
                    TWFrame frame = new TWFrame();
                    frame.Left = (TWFix32)ReadValue(baseAddress, ref offset, ItemType.Fix32);
                    frame.Top = (TWFix32)ReadValue(baseAddress, ref offset, ItemType.Fix32);
                    frame.Right = (TWFix32)ReadValue(baseAddress, ref offset, ItemType.Fix32);
                    frame.Bottom = (TWFix32)ReadValue(baseAddress, ref offset, ItemType.Fix32);
                    return frame; // no need to update offset again after reading fix32
                case ItemType.String128:
                case ItemType.String255:
                case ItemType.String32:
                case ItemType.String64:
                    val = Marshal.PtrToStringAnsi(new IntPtr(baseAddress.ToInt64() + offset));
                    break;
                case ItemType.Handle:
                    val = Marshal.ReadIntPtr(baseAddress, offset);
                    break;
            }
            offset += GetItemTypeSize(type);
            return val;
        }



        #region writes

        /// <summary>
        /// Writes a TWAIN value.
        /// </summary>
        /// <param name="baseAddr">The base addr.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="type">The TWAIN type.</param>
        /// <param name="value">The value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public static void WriteValue(this IntPtr baseAddr, ref int offset, ItemType type, object value)
        {
            switch (type)
            {
                case ItemType.Int8:
                case ItemType.UInt8:
                    Marshal.WriteByte(baseAddr, offset, Convert.ToByte(value, CultureInfo.InvariantCulture));// (byte)value);
                    break;
                case ItemType.Bool:
                case ItemType.Int16:
                case ItemType.UInt16:
                    Marshal.WriteInt16(baseAddr, offset, Convert.ToInt16(value, CultureInfo.InvariantCulture));//(short)value);
                    break;
                case ItemType.UInt32:
                case ItemType.Int32:
                    Marshal.WriteInt32(baseAddr, offset, Convert.ToInt32(value, CultureInfo.InvariantCulture));//(int)value);
                    break;
                case ItemType.Fix32:
                    TWFix32 f32 = (TWFix32)value;
                    Marshal.WriteInt16(baseAddr, offset, f32.Whole);
                    if (f32.Fraction > Int16.MaxValue)
                    {
                        Marshal.WriteInt16(baseAddr, offset + 2, (Int16)(f32.Fraction - 32768));
                    }
                    else
                    {
                        Marshal.WriteInt16(baseAddr, offset + 2, (Int16)f32.Fraction);
                    }
                    break;
                case ItemType.Frame:
                    TWFrame frame = (TWFrame)value;
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Left);
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Top);
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Right);
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Bottom);
                    return; // no need to update offset for this
                //case ItemType.String1024:
                //    WriteString(baseAddr, offset, value as string, 1024);
                //    break;
                case ItemType.String128:
                    WriteString(baseAddr, offset, (string)value, 128);
                    break;
                case ItemType.String255:
                    WriteString(baseAddr, offset, (string)value, 255);
                    break;
                case ItemType.String32:
                    WriteString(baseAddr, offset, (string)value, 32);
                    break;
                case ItemType.String64:
                    WriteString(baseAddr, offset, (string)value, 64);
                    break;
                //case ItemType.Unicode512:
                //    WriteUString(baseAddr, offset, value as string, 512);
                //    break;
            }
            offset += TypeExtensions.GetItemTypeSize(type);
        }
        /// <summary>
        /// Writes string value. THIS MAY BE WRONG.
        /// </summary>
        /// <param name="baseAddr"></param>
        /// <param name="offset"></param>
        /// <param name="item"></param>
        /// <param name="maxLength"></param>
        static void WriteString(IntPtr baseAddr, int offset, string item, int maxLength)
        {
            if (string.IsNullOrEmpty(item))
            {
                // write zero
                Marshal.WriteByte(baseAddr, offset, 0);
            }
            else
            {
                for (int i = 0; i < maxLength; i++)
                {
                    if (i == item.Length)
                    {
                        // string end reached, so write \0 and quit
                        Marshal.WriteByte(baseAddr, offset, 0);
                        return;
                    }
                    else
                    {
                        Marshal.WriteByte(baseAddr, offset, (byte)item[i]);
                        offset++;
                    }
                }
                // when ended normally also write \0
                Marshal.WriteByte(baseAddr, offset, 0);
            }
        }
        ///// <summary>
        ///// Writes unicode string value.
        ///// </summary>
        ///// <param name="baseAddr"></param>
        ///// <param name="offset"></param>
        ///// <param name="item"></param>
        ///// <param name="maxLength"></param>
        //[EnvironmentPermissionAttribute(SecurityAction.LinkDemand)]
        //private void WriteUString(IntPtr baseAddr, int offset, string item, int maxLength)
        //{
        //    if (string.IsNullOrEmpty(item))
        //    {
        //        // write zero
        //        Marshal.WriteInt16(baseAddr, offset, (char)0);
        //    }
        //    else
        //    {
        //        // use 2 bytes per char
        //        for (int i = 0; i < maxLength; i++)
        //        {
        //            if (i == item.Length)
        //            {
        //                // string end reached, so write \0 and quit
        //                Marshal.WriteInt16(baseAddr, offset, (char)0);
        //                return;
        //            }
        //            else
        //            {
        //                Marshal.WriteInt16(baseAddr, offset, item[i]);
        //                offset += 2;
        //            }
        //        }
        //        // when ended normally also write \0
        //        Marshal.WriteByte(baseAddr, offset, 0);
        //    }
        //}

        #endregion

    }
}
