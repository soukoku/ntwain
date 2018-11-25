using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// CLass that can generate <see cref="TW_CAPABILITY"/> for use in capability negotiation.
    /// </summary>
    public class CapWriter
    {
        private readonly TwainConfig config;

        /// <summary>
        /// Creates a new <see cref="CapWriter"/>.
        /// </summary>
        /// <param name="config"></param>
        internal CapWriter(TwainConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Generates a <see cref="TW_CAPABILITY"/> using single value (aka TW_ONEVALUE).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cap"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TW_CAPABILITY Generate<T>(CapabilityId cap, ItemType type, T value)
        {
            // size of data + uint16 item type
            var valueSz = type.GetSize();
            if (valueSz < 4) valueSz = 4; // onevalue container value minimum is 32bit
            var memSz = valueSz + 2; // + item type field

            var twCap = new TW_CAPABILITY
            {
                Capability = cap,
                ContainerType = ContainerType.OneValue,
                hContainer = config.MemoryManager.Allocate((uint)memSz)
            };

            if (twCap.hContainer != IntPtr.Zero)
            {
                IntPtr baseAddr = config.MemoryManager.Lock(twCap.hContainer);
                try
                {
                    int offset = 0;
                    // TODO: type size may be different on mac
                    baseAddr.WriteValue(ref offset, ItemType.UInt16, value);
                    // ONEVALUE is special in value can be uint32 or string 
                    // if less than uint32 put it in lower word
                    // (string value seems undocumented but internet says put it as-is and not a pointer)
                    if (valueSz < 4)
                    {
                        Marshal.WriteInt16(baseAddr, offset, 0);
                        offset += 2;
                    }
                    baseAddr.WriteValue(ref offset, type, value);
                }
                finally
                {
                    config.MemoryManager.Unlock(twCap.hContainer);
                }
            }

            return twCap;
        }

        /// <summary>
        /// Generates a <see cref="TW_CAPABILITY"/> for use in capability negotiation
        /// using TWAIN's array value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TW_CAPABILITY Generate<T>(CapabilityId cap, ArrayValue<T> value)
        {
            var twCap = new TW_CAPABILITY
            {
                Capability = cap,
                ContainerType = ContainerType.Array,
                hContainer = config.MemoryManager.Allocate((uint)Marshal.SizeOf(typeof(TW_ARRAY)))
            };
            if (twCap.hContainer != IntPtr.Zero)
            {
                var listSz = value.Type.GetSize() * value.ItemList.Length;
                TW_ARRAY arr = new TW_ARRAY
                {
                    ItemType = (ushort)value.Type,
                    NumItems = (uint)value.ItemList.Length,
                    ItemList = config.MemoryManager.Allocate((uint)listSz)
                };
                if (arr.ItemList != IntPtr.Zero)
                {
                    IntPtr baseAddr = config.MemoryManager.Lock(arr.ItemList);
                    try
                    {
                        int offset = 0;
                        foreach (var it in value.ItemList)
                        {
                            baseAddr.WriteValue(ref offset, value.Type, it);
                        }
                    }
                    finally
                    {
                        config.MemoryManager.Unlock(arr.ItemList);
                    }
                }

                try
                {
                    IntPtr baseAddr = config.MemoryManager.Lock(twCap.hContainer);
                    Marshal.StructureToPtr(arr, baseAddr, false);
                }
                finally
                {
                    config.MemoryManager.Unlock(twCap.hContainer);
                }
            }
            return twCap;
        }

        ///// <summary>
        ///// Generates a <see cref="TW_CAPABILITY"/> for use in capability negotiation
        ///// using TWAIN's enum value.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="cap"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public TW_CAPABILITY Generate<T>(CapabilityId cap, EnumValue<T> value)
        //{
        //    var twCap = new TW_CAPABILITY
        //    {
        //        Capability = cap,
        //        ContainerType = ContainerType.Enum
        //    };

        //    return twCap;
        //}

        ///// <summary>
        ///// Generates a <see cref="TW_CAPABILITY"/> for use in capability negotiation
        ///// using TWAIN's range value.
        ///// </summary>
        ///// <param name="cap"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //public TW_CAPABILITY Generate(CapabilityId cap, RangeValue value)
        //{
        //    var twCap = new TW_CAPABILITY
        //    {
        //        Capability = cap,
        //        ContainerType = ContainerType.Range
        //    };

        //    return twCap;
        //}


        //void SetEnumValue(TW_ENUMERATION value, IMemoryManager memoryManager)
        //{
        //    if (value == null) { throw new ArgumentNullException("value"); }
        //    ContainerType = ContainerType.Enum;


        //    Int32 valueSize = TW_ENUMERATION.ItemOffset + value.ItemList.Length * TypeExtensions.GetItemTypeSize(value.ItemType);

        //    int offset = 0;
        //    _hContainer = memoryManager.Allocate((uint)valueSize);
        //    if (_hContainer != IntPtr.Zero)
        //    {
        //        IntPtr baseAddr = memoryManager.Lock(_hContainer);

        //        // can't safely use StructureToPtr here so write it our own
        //        baseAddr.WriteValue(ref offset, ItemType.UInt16, value.ItemType);
        //        baseAddr.WriteValue(ref offset, ItemType.UInt32, (uint)value.ItemList.Length);
        //        baseAddr.WriteValue(ref offset, ItemType.UInt32, value.CurrentIndex);
        //        baseAddr.WriteValue(ref offset, ItemType.UInt32, value.DefaultIndex);
        //        foreach (var item in value.ItemList)
        //        {
        //            baseAddr.WriteValue(ref offset, value.ItemType, item);
        //        }
        //        memoryManager.Unlock(_hContainer);
        //    }
        //}

        //void SetRangeValue(TW_RANGE value, IMemoryManager memoryManager)
        //{
        //    if (value == null) { throw new ArgumentNullException("value"); }
        //    ContainerType = ContainerType.Range;

        //    // since range value can only house UInt32 we will not allow type size > 4
        //    if (TypeExtensions.GetItemTypeSize(value.ItemType) > 4) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.BadValueType, "TW_RANGE")); }

        //    _hContainer = memoryManager.Allocate((uint)Marshal.SizeOf(value));
        //    if (_hContainer != IntPtr.Zero)
        //    {
        //        Marshal.StructureToPtr(value, _hContainer, false);
        //    }
        //}
    }
}
