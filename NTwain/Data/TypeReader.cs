using System;
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
        /// <param name="baseAddr">The base addr.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="type">The TWAIN type.</param>
        /// <returns></returns>
        public static object ReadValue(IntPtr baseAddr, ref int offset, ItemType type)
        {
            object val = null;
            switch (type)
            {
                case ItemType.Int8:
                    val = (sbyte)Marshal.ReadByte(baseAddr, offset);
                    break;
                case ItemType.UInt8:
                    val = Marshal.ReadByte(baseAddr, offset);
                    break;
                case ItemType.Bool:
                case ItemType.UInt16:
                    val = (ushort)Marshal.ReadInt16(baseAddr, offset);
                    break;
                case ItemType.Int16:
                    val = Marshal.ReadInt16(baseAddr, offset);
                    break;
                case ItemType.UInt32:
                    val = (uint)Marshal.ReadInt32(baseAddr, offset);
                    break;
                case ItemType.Int32:
                    val = Marshal.ReadInt32(baseAddr, offset);
                    break;
                case ItemType.Fix32:
                    TWFix32 f32 = new TWFix32();
                    f32.Whole = Marshal.ReadInt16(baseAddr, offset);
                    f32.Fraction = (ushort)Marshal.ReadInt16(baseAddr, offset + 2);
                    val = f32;
                    break;
                case ItemType.Frame:
                    TWFrame frame = new TWFrame();
                    frame.Left = (TWFix32)ReadValue(baseAddr, ref offset, ItemType.Fix32);
                    frame.Top = (TWFix32)ReadValue(baseAddr, ref offset, ItemType.Fix32);
                    frame.Right = (TWFix32)ReadValue(baseAddr, ref offset, ItemType.Fix32);
                    frame.Bottom = (TWFix32)ReadValue(baseAddr, ref offset, ItemType.Fix32);
                    return frame; // no need to update offset again after reading fix32
                case ItemType.String128:
                    val = ReadString(baseAddr, offset, TwainConst.String128 - 2);
                    break;
                case ItemType.String255:
                    val = ReadString(baseAddr, offset, TwainConst.String255 - 1);
                    break;
                case ItemType.String32:
                    val = ReadString(baseAddr, offset, TwainConst.String32 - 2);
                    break;
                case ItemType.String64:
                    val = ReadString(baseAddr, offset, TwainConst.String64 - 2);
                    break;
                case ItemType.Handle:
                    val = new IntPtr(baseAddr.ToInt64() + offset);
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


        /// <summary>
        /// Gets the byte size of the item type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int GetItemTypeSize(ItemType type)
        {
            switch (type)
            {
                case ItemType.Int8:
                case ItemType.UInt8:
                    return 1;
                case ItemType.UInt16:
                case ItemType.Int16:
                case ItemType.Bool:
                    return 2;
                case ItemType.Int32:
                case ItemType.UInt32:
                case ItemType.Fix32:
                    return 4;
                case ItemType.Frame:
                    return 16;
                case ItemType.String32:
                    return TwainConst.String32;
                case ItemType.String64:
                    return TwainConst.String64;
                case ItemType.String128:
                    return TwainConst.String128;
                case ItemType.String255:
                    return TwainConst.String255;
                case ItemType.Handle:
                    return IntPtr.Size;
            }
            return 0;
        }
    }
}
