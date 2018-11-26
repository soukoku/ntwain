using NTwain.Resources;
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
        static readonly Dictionary<ItemType, int> _twainBytes = new Dictionary<ItemType, int>
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
            { ItemType.Handle, IntPtr.Size },
        };
        static readonly Dictionary<Type, ItemType> _netToTwainTypes = new Dictionary<Type, ItemType>
        {
            { typeof(sbyte), ItemType.Int8 },
            { typeof(byte), ItemType.UInt8 },
            { typeof(short), ItemType.Int16 },
            { typeof(ushort), ItemType.UInt16 },
            { typeof(int), ItemType.Int32 },
            { typeof(uint), ItemType.UInt32 },
            { typeof(TW_FIX32), ItemType.Fix32 },
            { typeof(TW_FRAME), ItemType.Frame },
            { typeof(IntPtr), ItemType.Handle },
            { typeof(UIntPtr), ItemType.Handle },
        };


        internal static int GetByteSize(this ItemType type)
        {
            if (_twainBytes.TryGetValue(type, out int size)) return size;

            throw new NotSupportedException(string.Format(MsgText.TypeNotSupported, type));
        }


        #region writes

        /// <summary>
        /// Writes a TWAIN value with type detection.
        /// </summary>
        /// <param name="baseAddr">The base addr.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="value">The value.</param>
        public static void WriteValue(this IntPtr baseAddr, ref int offset, object value)
        {
            var rawType = value.GetType();
            if (rawType.IsEnum)
            {
                // convert enum to numerical value
                rawType = Enum.GetUnderlyingType(rawType);
                value = Convert.ChangeType(value, rawType);
            }


            if (_netToTwainTypes.ContainsKey(rawType))
            {
                WriteValue(baseAddr, ref offset, value, _netToTwainTypes[rawType]);
            }
            else if (rawType == typeof(string))
            {
                var strVal = value.ToString();
                if (strVal.Length <= 32)
                {
                    WriteValue(baseAddr, ref offset, strVal, ItemType.String32);
                }
                else if (strVal.Length <= 64)
                {
                    WriteValue(baseAddr, ref offset, strVal, ItemType.String64);
                }
                else if (strVal.Length <= 128)
                {
                    WriteValue(baseAddr, ref offset, strVal, ItemType.String128);
                }
                else if (strVal.Length <= 255)
                {
                    WriteValue(baseAddr, ref offset, strVal, ItemType.String255);
                }
                else
                {
                    throw new NotSupportedException(string.Format(MsgText.MaxStringLengthExceeded, 255));
                }
            }
            else
            {
                throw new NotSupportedException(string.Format(MsgText.TypeNotSupported, rawType));
            }
        }


        /// <summary>
        /// Writes a TWAIN value as specified type.
        /// </summary>
        /// <param name="baseAddr">The base addr.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="value">The value.</param>
        /// <param name="type">The TWAIN type.</param>
        public static void WriteValue(this IntPtr baseAddr, ref int offset, object value, ItemType type)
        {
            switch (type)
            {
                case ItemType.Int8:
                case ItemType.UInt8:
                    Marshal.WriteByte(baseAddr, offset, Convert.ToByte(value, CultureInfo.InvariantCulture));
                    break;
                case ItemType.Bool:
                case ItemType.Int16:
                case ItemType.UInt16:
                    Marshal.WriteInt16(baseAddr, offset, Convert.ToInt16(value, CultureInfo.InvariantCulture));
                    break;
                case ItemType.UInt32:
                case ItemType.Int32:
                    Marshal.WriteInt32(baseAddr, offset, Convert.ToInt32(value, CultureInfo.InvariantCulture));
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
                case ItemType.Handle:
                    Marshal.WriteIntPtr(baseAddr, offset, (IntPtr)value);
                    break;
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
            offset += type.GetByteSize();
        }

        static void WriteFix32(IntPtr baseAddr, ref int offset, TW_FIX32 value)
        {
            Marshal.WriteInt16(baseAddr, offset, value.Whole);
            offset += _twainBytes[ItemType.Int16];
            if (value.Fraction > Int16.MaxValue)
            {
                Marshal.WriteInt16(baseAddr, offset, (Int16)(value.Fraction - 32768));
            }
            else
            {
                Marshal.WriteInt16(baseAddr, offset, (Int16)value.Fraction);
            }
            offset += _twainBytes[ItemType.Fix32];
        }
        static void WriteFrame(IntPtr baseAddr, ref int offset, TW_FRAME value)
        {
            WriteFix32(baseAddr, ref offset, value._left);
            WriteFix32(baseAddr, ref offset, value._top);
            WriteFix32(baseAddr, ref offset, value._right);
            WriteFix32(baseAddr, ref offset, value._bottom);
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
            // TODO: mac string is not null-terminated like this?

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
