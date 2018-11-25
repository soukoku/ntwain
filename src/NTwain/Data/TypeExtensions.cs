using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Data
{
    /// <summary>
    /// Contains extension methods for reading/writing primitive
    /// TWAIN data types.
    /// </summary>
    static class TypeExtensions
    {
        static readonly Dictionary<ItemType, int> _sizes = new Dictionary<ItemType, int>
        {
            { ItemType.Int8, 1 },
            { ItemType.UInt8, 1 },
            { ItemType.Int16, 2 },
            { ItemType.UInt16, 2 },
            { ItemType.Int32, 4 },
            { ItemType.UInt32, 4 },
            { ItemType.Bool, 2 },
            { ItemType.Fix32, Marshal.SizeOf(typeof(TW_FIX32)) },
            { ItemType.Frame, Marshal.SizeOf(typeof(TW_FRAME)) },
            { ItemType.String128, TwainConst.String128 },
            { ItemType.String255, TwainConst.String255 },
            { ItemType.String32, TwainConst.String32 },
            { ItemType.String64, TwainConst.String64 },
            // TODO: find out if it should be fixed 4 bytes or intptr size
            { ItemType.Handle, IntPtr.Size },
        };

        public static int GetSize(this ItemType type)
        {
            if(_sizes.TryGetValue(type, out int size)) return size;

            throw new NotSupportedException($"Unsupported item type {type}.");
        }


        #region writes

        /// <summary>
        /// Writes a TWAIN value.
        /// </summary>
        /// <param name="baseAddr">The base addr.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="type">The TWAIN type.</param>
        /// <param name="value">The value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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
                    TW_FIX32 f32 = (TW_FIX32)value;
                    WriteFix32(baseAddr, ref offset, f32);
                    return; // no need to update offset for this
                case ItemType.Frame:
                    TW_FRAME frame = (TW_FRAME)value;
                    WriteFix32(baseAddr, ref offset, frame._left);
                    WriteFix32(baseAddr, ref offset, frame._top);
                    WriteFix32(baseAddr, ref offset, frame._right);
                    WriteFix32(baseAddr, ref offset, frame._bottom);
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
            offset +=  type.GetSize();
        }

        private static void WriteFix32(IntPtr baseAddr, ref int offset, TW_FIX32 f32)
        {
            Marshal.WriteInt16(baseAddr, offset, f32.Whole);
            if (f32.Fraction > Int16.MaxValue)
            {
                Marshal.WriteInt16(baseAddr, offset + 2, (Int16)(f32.Fraction - 32768));
            }
            else
            {
                Marshal.WriteInt16(baseAddr, offset + 2, (Int16)f32.Fraction);
            }
            offset += _sizes[ItemType.Fix32];
        }

        /// <summary>
        /// Writes string value. THIS MAY BE WRONG.
        /// </summary>
        /// <param name="baseAddr"></param>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        static void WriteString(IntPtr baseAddr, int offset, string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                // write zero
                Marshal.WriteByte(baseAddr, offset, 0);
            }
            else
            {
                for (int i = 0; i < maxLength; i++)
                {
                    if (i == value.Length)
                    {
                        // string end reached, so write \0 and quit
                        Marshal.WriteByte(baseAddr, offset, 0);
                        return;
                    }
                    else
                    {
                        Marshal.WriteByte(baseAddr, offset, (byte)value[i]);
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
