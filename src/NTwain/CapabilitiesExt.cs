using NTwain.Data;
using System;
using System.Collections.Generic;

namespace NTwain
{
    public class CapabilitiesExt : Capabilities
    {
        private IDataSource _source;
        public CapabilitiesExt(IDataSource dataSource) : base(dataSource) { _source = dataSource; }

        private List<CapabilityId> _custom;

        public List<CapabilityId> CustomCapabilities
        {
            get
            {
                return _custom ?? LoadCustomCapabilities();
            }
        }

        private List<CapabilityId> LoadCustomCapabilities()
        {
            List<CapabilityId> custom = new List<CapabilityId>();
            foreach (CapabilityId capId in CapSupportedCaps.GetValues())
            {
                var capName = capId.ToString();
                var wrapper = GetType().GetProperty(capName);

                // not defined in Capabilites
                if (wrapper == null)
                {
                    custom.Add(capId);
                }
            }
            return custom;
        }


        private Func<BoolType, TWOneValue> boolFunc = value => new TWOneValue
        {
            Item = (uint)value,
            ItemType = ItemType.Bool
        };

        private Func<int, TWOneValue> intFunc = value => new TWOneValue
        {
            Item = (uint)value,
            ItemType = ItemType.UInt16
        };

        private Func<byte, TWOneValue> byteFunc = value => new TWOneValue
        {
            Item = value,
            ItemType = ItemType.UInt8
        };

        private Func<uint, TWOneValue> uintFunc = value => new TWOneValue
        {
            Item = value,
            ItemType = ItemType.UInt32
        };


    public object GetCap<TValue> (CapabilityId Capability)
        {
            QuerySupports? s = QuerySupport(Capability);
            bool readOnly = true;
            byte b = 0;
            if (s != null)
            {
                b = (byte)s;
                if ((b & (1 << 2)) != 0)
                {
                    readOnly = false;
                }
            }

            if (typeof(TValue) == typeof(string))
            {
                return new CapWrapper<string>(_source, Capability, ValueExtensions.ConvertToString, readOnly);
            }
            else if (typeof(TValue) == typeof(int))
            {
                if (readOnly)
                {
                    return new CapWrapper<int>(_source, Capability, ValueExtensions.ConvertToEnum<int>, true);
                }
                return new CapWrapper<int>(_source, Capability, ValueExtensions.ConvertToEnum<int>, intFunc);
            }
            else if (typeof(TValue) == typeof(byte))
            {
                if (readOnly)
                {
                    return new CapWrapper<byte>(_source, Capability, ValueExtensions.ConvertToEnum<byte>, true);
                }
                return new CapWrapper<byte>(_source, Capability, ValueExtensions.ConvertToEnum<byte>, byteFunc);
            }
            else if (typeof(TValue) == typeof(uint))
            {
                if (readOnly)
                {
                    return new CapWrapper<uint>(_source, Capability, ValueExtensions.ConvertToEnum<uint>, true);
                }
                return new CapWrapper<uint>(_source, Capability, ValueExtensions.ConvertToEnum<uint>, uintFunc);
            }
            else if (typeof(TValue) == typeof(TWFrame))
            {
                return new CapWrapper<TWFrame>(_source, Capability, ValueExtensions.ConvertToFrame, readOnly);
            }
            else if (typeof(TValue) == typeof(BoolType))
            {
                if (readOnly)
                {
                    return new CapWrapper<BoolType>(_source, Capability, ValueExtensions.ConvertToEnum<BoolType>, true);
                }

                return new CapWrapper<BoolType>(_source, Capability, ValueExtensions.ConvertToEnum<BoolType>, boolFunc);
            }
            else if (typeof(TValue) == typeof(TWFix32))
            {

                if (readOnly)
                {
                    return new CapWrapper<TWFix32>(_source, Capability, ValueExtensions.ConvertToFix32, true);
                }
                return new CapWrapper<TWFix32>(_source, Capability, ValueExtensions.ConvertToFix32, value => value.ToOneValue());

                
            }
            else
            {
                throw new Exception($"Unknown defintion for type of {typeof(TValue)} in GetCap");
            }
        }
    }
}
