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
                TW_ARRAY container = new TW_ARRAY
                {
                    ItemType = (ushort)value.Type,
                    NumItems = (uint)value.ItemList.Length,
                    ItemList = config.MemoryManager.Allocate((uint)listSz)
                };
                if (container.ItemList != IntPtr.Zero)
                {
                    IntPtr baseAddr = config.MemoryManager.Lock(container.ItemList);
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
                        config.MemoryManager.Unlock(container.ItemList);
                    }

                    try
                    {
                        baseAddr = config.MemoryManager.Lock(twCap.hContainer);
                        Marshal.StructureToPtr(container, baseAddr, false);
                    }
                    finally
                    {
                        config.MemoryManager.Unlock(twCap.hContainer);
                    }
                }
                else
                {
                    config.MemoryManager.Free(twCap.hContainer);
                }
            }
            return twCap;
        }

        /// <summary>
        /// Generates a <see cref="TW_CAPABILITY"/> for use in capability negotiation
        /// using TWAIN's enum value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TW_CAPABILITY Generate<T>(CapabilityId cap, EnumValue<T> value)
        {
            var twCap = new TW_CAPABILITY
            {
                Capability = cap,
                ContainerType = ContainerType.Enum,
                hContainer = config.MemoryManager.Allocate((uint)Marshal.SizeOf(typeof(TW_ENUMERATION)))
            };
            if (twCap.hContainer != IntPtr.Zero)
            {
                var listSz = value.Type.GetSize() * value.ItemList.Length;
                TW_ENUMERATION container = new TW_ENUMERATION
                {
                    ItemType = (ushort)value.Type,
                    NumItems = (uint)value.ItemList.Length,
                    CurrentIndex = (uint)value.CurrentIndex,
                    DefaultIndex = (uint)value.DefaultIndex,
                    ItemList = config.MemoryManager.Allocate((uint)listSz)
                };
                if (container.ItemList != IntPtr.Zero)
                {
                    IntPtr baseAddr = config.MemoryManager.Lock(container.ItemList);
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
                        config.MemoryManager.Unlock(container.ItemList);
                    }

                    try
                    {
                        baseAddr = config.MemoryManager.Lock(twCap.hContainer);
                        Marshal.StructureToPtr(container, baseAddr, false);
                    }
                    finally
                    {
                        config.MemoryManager.Unlock(twCap.hContainer);
                    }
                }
                else
                {
                    config.MemoryManager.Free(twCap.hContainer);
                }
            }
            return twCap;
        }

        /// <summary>
        /// Generates a <see cref="TW_CAPABILITY"/> for use in capability negotiation
        /// using TWAIN's range value.
        /// </summary>
        /// <param name="cap"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public TW_CAPABILITY Generate(CapabilityId cap, TW_RANGE value)
        {
            var twCap = new TW_CAPABILITY
            {
                Capability = cap,
                ContainerType = ContainerType.Range,
                hContainer = config.MemoryManager.Allocate((uint)Marshal.SizeOf(typeof(TW_RANGE)))
            };
            if (twCap.hContainer != IntPtr.Zero)
            {
                try
                {
                    IntPtr baseAddr = config.MemoryManager.Lock(twCap.hContainer);
                    Marshal.StructureToPtr(value, baseAddr, false);
                }
                finally
                {
                    config.MemoryManager.Unlock(twCap.hContainer);
                }
            }
            return twCap;
        }
    }
}
