using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NTwain.Data
{
    /// <summary>
    /// Class to read common TWAIN values.
    /// </summary>
    public static class TypeReader
    {

        /// <summary>
        /// Reads a TWAIN value.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="type">The TWAIN type.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "1#")]
        public static object ReadValue(IntPtr baseAddress, ref int offset, ItemType type)
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
                    val = ReadString(baseAddress, offset, TwainConst.String128 - 2);
                    break;
                case ItemType.String255:
                    val = ReadString(baseAddress, offset, TwainConst.String255 - 1);
                    break;
                case ItemType.String32:
                    val = ReadString(baseAddress, offset, TwainConst.String32 - 2);
                    break;
                case ItemType.String64:
                    val = ReadString(baseAddress, offset, TwainConst.String64 - 2);
                    break;
                case ItemType.Handle:
                    val = Marshal.ReadIntPtr(baseAddress, offset);
                    break;
            }
            offset += GetItemTypeSize(type);
            return val;
        }

        static string ReadString(IntPtr baseAddr, int offset, int maxLength)
        {
            // does this work cross-platform?
            var val = Marshal.PtrToStringAnsi(new IntPtr(baseAddr.ToInt64() + offset));
            if (val.Length > maxLength)
            {
                // bad source, whatever
            }
            return val;
            //var sb = new StringBuilder(maxLength);
            //byte bt;
            //while (sb.Length < maxLength &&
            //    (bt = Marshal.ReadByte(baseAddr, offset++)) != 0)
            //{
            //    sb.Append((char)bt);
            //}
            //return sb.ToString();
        }

        static readonly Dictionary<ItemType, int> __sizes = GenerateSizes();

        private static Dictionary<ItemType, int> GenerateSizes()
        {
            var sizes = new Dictionary<ItemType, int>();
            sizes[ItemType.Int8] = 1;
            sizes[ItemType.UInt8] = 1;
            sizes[ItemType.Int16] = 2;
            sizes[ItemType.UInt16] = 2;
            sizes[ItemType.Bool] = 2;
            sizes[ItemType.Int32] = 4;
            sizes[ItemType.UInt32] = 4;
            sizes[ItemType.Fix32] = 4;
            sizes[ItemType.Frame] = 16;
            sizes[ItemType.String32] = TwainConst.String32;
            sizes[ItemType.String64] = TwainConst.String64;
            sizes[ItemType.String128] = TwainConst.String128;
            sizes[ItemType.String255] = TwainConst.String255;
            sizes[ItemType.Handle] = IntPtr.Size;

            return sizes;
        }

        /// <summary>
        /// Gets the byte size of the item type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetItemTypeSize(ItemType type)
        {
            if (__sizes.ContainsKey(type))
            {
                return __sizes[type];
            }
            return 0;
        }
    }
}
