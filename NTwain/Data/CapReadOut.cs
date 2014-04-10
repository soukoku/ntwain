using NTwain.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Data
{
    /// <summary>
    /// The one-stop class for reading TWAIN cap values.
    /// This contains all the properties for the 4 container types.
    /// </summary>
    public class CapReadOut
    {
        /// <summary>
        /// Reads the value from a <see cref="TWCapability"/> that was returned
        /// from a TWAIN source.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">capability</exception>
        /// <exception cref="System.ArgumentException">
        /// Capability contains no data.;capability
        /// or
        /// capability
        /// </exception>
        public static CapReadOut ReadValue(TWCapability capability)
        {
            if (capability == null) { throw new ArgumentNullException("capability"); }
            if (capability.Container == IntPtr.Zero) { throw new ArgumentException("Capability contains no data.", "capability"); }

            IntPtr baseAddr = IntPtr.Zero;
            try
            {
                baseAddr = MemoryManager.Instance.Lock(capability.Container);
                switch (capability.ContainerType)
                {
                    case ContainerType.Array:
                        return new CapReadOut
                        {
                            ContainerType = capability.ContainerType,
                        }.ReadArrayValue(baseAddr);
                    case ContainerType.Enum:
                        return new CapReadOut
                        {
                            ContainerType = capability.ContainerType,
                        }.ReadEnumValue(baseAddr);
                    case ContainerType.OneValue:
                        return new CapReadOut
                        {
                            ContainerType = capability.ContainerType,
                        }.ReadOneValue(baseAddr);
                    case ContainerType.Range:
                        return new CapReadOut
                        {
                            ContainerType = capability.ContainerType,
                        }.ReadRangeValue(baseAddr);
                    default:
                        throw new ArgumentException(string.Format("Capability has invalid container type {0}.", capability.ContainerType), "capability");
                }
            }
            finally
            {
                if (baseAddr != IntPtr.Zero)
                {
                    MemoryManager.Instance.Unlock(baseAddr);
                }
            }
        }

        #region common prop

        /// <summary>
        /// Gets the underlying container type. 
        /// </summary>
        /// <value>
        /// The container.
        /// </value>
        public ContainerType ContainerType { get; private set; }

        /// <summary>
        /// Gets the type of the TWAIN value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public ItemType ItemType { get; private set; }

        /// <summary>
        /// Gets the one value if container is <see cref="ContainerType.Array"/>.
        /// </summary>
        /// <value>
        /// The one value.
        /// </value>
        public object OneValue { get; private set; }

        /// <summary>
        /// Gets the collection values if container is <see cref="ContainerType.Enum"/> or <see cref="ContainerType.Range"/> .
        /// </summary>
        /// <value>
        /// The collection values.
        /// </value>
        public IList<object> CollectionValues { get; private set; }

        #endregion

        #region enum prop

        /// <summary>
        /// Gets the current value index if container is <see cref="ContainerType.Enum"/>.
        /// </summary>
        public int EnumCurrentIndex { get; private set; }
        /// <summary>
        /// Gets the default value index if container is <see cref="ContainerType.Enum" />.
        /// </summary>
        public int EnumDefaultIndex { get; private set; }

        #endregion

        #region range prop

        /// <summary>
        /// Gets the current value if container is <see cref="ContainerType.Range" />.
        /// </summary>
        /// <value>
        /// The range current value.
        /// </value>
        public object RangeCurrentValue { get; private set; }
        /// <summary>
        /// Gets the default value if container is <see cref="ContainerType.Range" />.
        /// </summary>
        /// <value>
        /// The range default value.
        /// </value>
        public object RangeDefaultValue { get; private set; }
        /// <summary>
        /// The least positive/most negative value of the range.
        /// </summary>
        /// <value>
        /// The range minimum value.
        /// </value>
        public uint RangeMinValue { get; private set; }
        /// <summary>
        /// The most positive/least negative value of the range.
        /// </summary>
        /// <value>
        /// The range maximum value.
        /// </value>
        public uint RangeMaxValue { get; private set; }
        /// <summary>
        /// The delta between two adjacent values of the range.
        /// e.g. Item2 - Item1 = StepSize;
        /// </summary>
        /// <value>
        /// The size of the range step.
        /// </value>
        public uint RangeStepSize { get; private set; }

        #endregion

        #region reader methods

        CapReadOut ReadOneValue(IntPtr baseAddr)
        {
            int offset = 0;
            ItemType = (ItemType)(ushort)Marshal.ReadInt16(baseAddr, offset);
            offset += 2;
            OneValue = ReadValue(baseAddr, ref offset, ItemType);
            return this;
        }

        CapReadOut ReadArrayValue(IntPtr baseAddr)
        {
            int offset = 0;
            ItemType = (ItemType)(ushort)Marshal.ReadInt16(baseAddr, offset);
            offset += 2;
            var count = Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            if (count > 0)
            {
                CollectionValues = new object[count];
                for (int i = 0; i < count; i++)
                {
                    CollectionValues[i] = ReadValue(baseAddr, ref offset, ItemType);
                }
            }
            return this;
        }

        CapReadOut ReadEnumValue(IntPtr baseAddr)
        {
            int offset = 0;
            ItemType = (ItemType)(ushort)Marshal.ReadInt16(baseAddr, offset);
            offset += 2;
            int count = Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            EnumCurrentIndex = Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            EnumDefaultIndex = Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            if (count > 0)
            {
                CollectionValues = new object[count];
                for (int i = 0; i < count; i++)
                {
                    CollectionValues[i] = ReadValue(baseAddr, ref offset, ItemType);
                }
            }
            return this;
        }

        CapReadOut ReadRangeValue(IntPtr baseAddr)
        {
            int offset = 0;
            ItemType = (ItemType)(ushort)Marshal.ReadInt16(baseAddr, offset);
            offset += 2;
            RangeMinValue = (uint)Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            RangeMaxValue = (uint)Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            RangeStepSize = (uint)Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            RangeDefaultValue = (uint)Marshal.ReadInt32(baseAddr, offset);
            offset += 4;
            RangeCurrentValue = (uint)Marshal.ReadInt32(baseAddr, offset);

            return this;
        }

        static object ReadValue(IntPtr baseAddr, ref int offset, ItemType type)
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

        /// <summary>
        /// Read string value.
        /// </summary>
        /// <param name="baseAddr"></param>
        /// <param name="offset"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
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
        internal static int GetItemTypeSize(ItemType type)
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
        #endregion
    }
}
