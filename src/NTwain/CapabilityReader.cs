using NTwain.Data;
using NTwain.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace NTwain
{
    /// <summary>
    /// The one-stop class for reading raw TWAIN cap values from the cap container.
    /// This contains all the properties for the 4 container types.
    /// </summary>
    public class CapabilityReader
    {
        /// <summary>
        /// Reads the value from a <see cref="TWCapability" /> that was returned
        /// from a TWAIN source.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">capability</exception>
        /// <exception cref="System.ArgumentException">Capability contains no data.;capability
        /// or
        /// capability</exception>
        public static CapabilityReader ReadValue(TWCapability capability)
        {
            return ReadValue(capability, PlatformInfo.Current.MemoryManager);
        }

        /// <summary>
        /// Reads the value from a <see cref="TWCapability" /> that was returned
        /// from a TWAIN source.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <param name="memoryManager">The memory manager.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// capability
        /// or
        /// memoryManager
        /// </exception>
        /// <exception cref="System.ArgumentException">capability</exception>
        public static CapabilityReader ReadValue(TWCapability capability, IMemoryManager memoryManager)
        {
            if (capability == null) { throw new ArgumentNullException("capability"); }
            if (memoryManager == null) { throw new ArgumentNullException("memoryManager"); }

            if (capability.Container != IntPtr.Zero)
            {
                IntPtr baseAddr = IntPtr.Zero;
                try
                {
                    baseAddr = memoryManager.Lock(capability.Container);
                    switch (capability.ContainerType)
                    {
                        case ContainerType.Array:
                            return new CapabilityReader
                            {
                                ContainerType = capability.ContainerType,
                            }.ReadArrayValue(baseAddr);
                        case ContainerType.Enum:
                            return new CapabilityReader
                            {
                                ContainerType = capability.ContainerType,
                            }.ReadEnumValue(baseAddr);
                        case ContainerType.OneValue:
                            return new CapabilityReader
                            {
                                ContainerType = capability.ContainerType,
                            }.ReadOneValue(baseAddr);
                        case ContainerType.Range:
                            return new CapabilityReader
                            {
                                ContainerType = capability.ContainerType,
                            }.ReadRangeValue(baseAddr);
                        default:
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                                Resources.CapHasBadContainer, capability.Capability, capability.ContainerType), "capability");
                    }
                }
                finally
                {
                    if (baseAddr != IntPtr.Zero)
                    {
                        //memoryManager.Unlock(baseAddr);
                        memoryManager.Unlock(capability.Container);
                    }
                }
            }
            else
            {
                return new CapabilityReader();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilityReader"/> class.
        /// </summary>
        public CapabilityReader()
        {
            RangeCount = -1;
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
        /// Gets the one value if container is <see cref="NTwain.Data.ContainerType.OneValue"/>.
        /// </summary>
        /// <value>
        /// The one value.
        /// </value>
        public object OneValue { get; private set; }

        /// <summary>
        /// Gets the collection values if container is <see cref="NTwain.Data.ContainerType.Enum"/> or <see cref="NTwain.Data.ContainerType.Array"/> .
        /// </summary>
        /// <value>
        /// The collection values.
        /// </value>
        public IList<object> CollectionValues { get; private set; }

        #endregion

        #region enum prop

        /// <summary>
        /// Gets the current value index if container is <see cref="NTwain.Data.ContainerType.Enum"/>.
        /// </summary>
        public int EnumCurrentIndex { get; private set; }
        /// <summary>
        /// Gets the default value index if container is <see cref="NTwain.Data.ContainerType.Enum" />.
        /// </summary>
        public int EnumDefaultIndex { get; private set; }

        #endregion

        #region range prop

        /// <summary>
        /// Gets the current value if container is <see cref="NTwain.Data.ContainerType.Range" />.
        /// </summary>
        /// <value>
        /// The range current value.
        /// </value>
        public object RangeCurrentValue { get; private set; }
        /// <summary>
        /// Gets the default value if container is <see cref="NTwain.Data.ContainerType.Range" />.
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
        public object RangeMinValue { get; private set; }
        /// <summary>
        /// The most positive/least negative value of the range.
        /// </summary>
        /// <value>
        /// The range maximum value.
        /// </value>
        public object RangeMaxValue { get; private set; }
        /// <summary>
        /// The delta between two adjacent values of the range.
        /// e.g. Item2 - Item1 = StepSize;
        /// </summary>
        /// <value>
        /// The size of the range step.
        /// </value>
        public object RangeStepSize { get; private set; }

        /// <summary>
        /// Gets the number of range values if range is expanded.
        /// You should check for this before expanding range type containers to prevent
        /// possible <see cref="OutOfMemoryException"/>.
        /// </summary>
        /// <value>
        /// The range count.
        /// </value>
        public int RangeCount { get; private set; }

        #endregion

        #region reader methods

        /// <summary>
        /// Don't care what container it is, just enumerate the capability values.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> EnumerateCapValues()
        {
            switch (ContainerType)
            {
                case ContainerType.OneValue:
                    return EnumerateOneValue();
                case ContainerType.Array:
                case ContainerType.Enum:
                    if (CollectionValues != null)
                    {
                        return CollectionValues;
                    }
                    break;
                case ContainerType.Range:
                    return EnumerateRange();
            }
            return Enumerable.Empty<object>();
        }

        IEnumerable<object> EnumerateOneValue()
        {
            if (OneValue != null)
            {
                yield return OneValue;
            }
        }

        IEnumerable<object> EnumerateRange()
        {
            // horrible cast but should work.
            // in the for loop we also compare against min in case the step
            // is parsed as negative number and causes infinite loop.
            switch (ItemType)
            {
                case Data.ItemType.Fix32:
                    {
                        var min = (TWFix32)RangeMinValue;
                        var max = (TWFix32)RangeMaxValue;
                        var step = (TWFix32)RangeStepSize;

                        for (var i = min; i >= min && i <= max; i += step)
                        {
                            yield return i;
                        }
                    }
                    break;
                case Data.ItemType.UInt32:
                    {
                        var min = (uint)RangeMinValue;
                        var max = (uint)RangeMaxValue;
                        var step = (uint)RangeStepSize;

                        for (var i = min; i >= min && i <= max; i += step)
                        {
                            yield return i;
                        }
                    }
                    break;
                case Data.ItemType.Int32:
                    {
                        var min = (int)RangeMinValue;
                        var max = (int)RangeMaxValue;
                        var step = (int)RangeStepSize;

                        for (var i = min; i >= min && i <= max; i += step)
                        {
                            yield return i;
                        }
                    }
                    break;
                // these should never happen since TW_RANGE fields are 4 bytes but you never know
                case Data.ItemType.UInt16:
                    {
                        var min = (ushort)RangeMinValue;
                        var max = (ushort)RangeMaxValue;
                        var step = (ushort)RangeStepSize;

                        for (var i = min; i >= min && i <= max; i += step)
                        {
                            yield return i;
                        }
                    }
                    break;
                case Data.ItemType.Int16:
                    {
                        var min = (short)RangeMinValue;
                        var max = (short)RangeMaxValue;
                        var step = (short)RangeStepSize;

                        for (var i = min; i >= min && i <= max; i += step)
                        {
                            yield return i;
                        }
                    }
                    break;
                case Data.ItemType.UInt8:
                    {
                        var min = (byte)RangeMinValue;
                        var max = (byte)RangeMaxValue;
                        var step = (byte)RangeStepSize;

                        for (var i = min; i >= min && i <= max; i += step)
                        {
                            yield return i;
                        }
                    }
                    break;
                case Data.ItemType.Int8:
                    {
                        var min = (sbyte)RangeMinValue;
                        var max = (sbyte)RangeMaxValue;
                        var step = (sbyte)RangeStepSize;

                        for (var i = min; i >= min && i <= max; i += step)
                        {
                            yield return i;
                        }
                    }
                    break;
            }
        }

        ///// <summary>
        ///// Don't care what container it is, just populate the specified list with the capability values (count be one or many).
        ///// </summary>
        ///// <param name="toPopulate">The list to populate the values.</param>
        ///// <returns></returns>
        //public IList<object> PopulateFromCapValues(IList<object> toPopulate)
        //{
        //    if (toPopulate == null) { toPopulate = new List<object>(); }
        //    foreach(var obj in EnumerateCapValues())
        //    {
        //        toPopulate.Add(obj);
        //    }
        //    return toPopulate;
        //}

        CapabilityReader ReadOneValue(IntPtr baseAddr)
        {
            int offset = 0;
            ItemType = (ItemType)(ushort)Marshal.ReadInt16(baseAddr, offset);
            offset += 2;
            OneValue = baseAddr.ReadValue(ref offset, ItemType);
            return this;
        }

        CapabilityReader ReadArrayValue(IntPtr baseAddr)
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
                    CollectionValues[i] = baseAddr.ReadValue(ref offset, ItemType);
                }
            }
            return this;
        }

        CapabilityReader ReadEnumValue(IntPtr baseAddr)
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
                    CollectionValues[i] = baseAddr.ReadValue(ref offset, ItemType);
                }
            }
            return this;
        }

        CapabilityReader ReadRangeValue(IntPtr baseAddr)
        {
            int offset = 0;
            ItemType = (ItemType)(ushort)Marshal.ReadInt16(baseAddr, offset);
            offset += 2;

            RangeMinValue = baseAddr.ReadValue(ref offset, ItemType);
            RangeMaxValue = baseAddr.ReadValue(ref offset, ItemType);
            RangeStepSize = baseAddr.ReadValue(ref offset, ItemType);
            RangeDefaultValue = baseAddr.ReadValue(ref offset, ItemType);
            RangeCurrentValue = baseAddr.ReadValue(ref offset, ItemType);


            // do range count
            switch (ItemType)
            {
                case Data.ItemType.Fix32:
                    {
                        var min = (TWFix32)RangeMinValue;
                        var max = (TWFix32)RangeMaxValue;
                        var step = (TWFix32)RangeStepSize;

                        RangeCount = (int)((max - min) / step) + 1;
                    }
                    break;
                case Data.ItemType.UInt32:
                    {
                        var min = (uint)RangeMinValue;
                        var max = (uint)RangeMaxValue;
                        var step = (uint)RangeStepSize;

                        RangeCount = (int)((max - min) / step) + 1;
                    }
                    break;
                case Data.ItemType.Int32:
                    {
                        var min = (int)RangeMinValue;
                        var max = (int)RangeMaxValue;
                        var step = (int)RangeStepSize;

                        RangeCount = ((max - min) / step) + 1;
                    }
                    break;
                // these should never happen since TW_RANGE fields are 4 bytes but you never know
                case Data.ItemType.UInt16:
                    {
                        var min = (ushort)RangeMinValue;
                        var max = (ushort)RangeMaxValue;
                        var step = (ushort)RangeStepSize;

                        RangeCount = ((max - min) / step) + 1;
                    }
                    break;
                case Data.ItemType.Int16:
                    {
                        var min = (short)RangeMinValue;
                        var max = (short)RangeMaxValue;
                        var step = (short)RangeStepSize;

                        RangeCount = ((max - min) / step) + 1;
                    }
                    break;
                case Data.ItemType.UInt8:
                    {
                        var min = (byte)RangeMinValue;
                        var max = (byte)RangeMaxValue;
                        var step = (byte)RangeStepSize;

                        RangeCount = ((max - min) / step) + 1;
                    }
                    break;
                case Data.ItemType.Int8:
                    {
                        var min = (sbyte)RangeMinValue;
                        var max = (sbyte)RangeMaxValue;
                        var step = (sbyte)RangeStepSize;

                        RangeCount = ((max - min) / step) + 1;
                    }
                    break;
            }
            return this;
        }

        #endregion
    }
}
