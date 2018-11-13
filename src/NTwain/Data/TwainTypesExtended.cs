using NTwain.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace NTwain.Data
{

    // This file contains custom logic added to the twain types.
    // Mostly this just makes the fields
    // into .net friendly properties.

    // most unit tests for the twain types only need to target 
    // code in this file since everything else is just interop and 
    // field definitions.

    // most of the doc text are copied from the twain spec pdf. 


    /// <summary>
    /// Stores a fixed point number. This can be implicitly converted 
    /// to a float in dotnet.
    /// </summary>
    public partial struct TW_FIX32 : IEquatable<TW_FIX32>, IConvertible
    {
        // the conversion logic is found in the spec.

        float ToFloat()
        {
            return (float)_whole + _frac / 65536f;
        }
        TW_FIX32(float value)
        {
            //int temp = (int)(value * 65536.0 + 0.5);
            //_whole = (short)(temp >> 16);
            //_frac = (ushort)(temp & 0x0000ffff);

            // different version from twain faq
            bool sign = value < 0;
            int temp = (int)(value * 65536.0 + (sign ? (-0.5) : 0.5));
            _whole = (short)(temp >> 16);
            _frac = (ushort)(temp & 0x0000ffff);

        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return ToFloat().ToString(CultureInfo.InvariantCulture);
        }

        ///// <summary>
        ///// Converts this to <see cref="TW_ONEVALUE"/> for capability set methods.
        ///// </summary>
        ///// <returns></returns>
        //public TW_ONEVALUE ToOneValue()
        //{
        //    // copy struct parts as-is.
        //    // probably has a faster way but can't think now

        //    byte[] array = new byte[4];
        //    var part = BitConverter.GetBytes(Whole);
        //    Buffer.BlockCopy(part, 0, array, 0, 2);

        //    part = BitConverter.GetBytes(Fraction);
        //    Buffer.BlockCopy(part, 0, array, 2, 2);

        //    var converted = BitConverter.ToUInt32(array, 0);

        //    return new TW_ONEVALUE
        //    {
        //        ItemType = ItemType.Fix32,
        //        Item = converted
        //        // old wrong conversion
        //        // (uint)this,// ((uint)dpi) << 16;
        //    };
        //}

        #region equals

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TW_FIX32))
                return false;

            return Equals((TW_FIX32)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TW_FIX32 other)
        {
            return _whole == other._whole && _frac == other._frac;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _whole ^ _frac;
        }

        #endregion

        #region static stuff

        /// <summary>
        /// Performs an implicit conversion from <see cref="NTwain.Data.TW_FIX32"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(TW_FIX32 value)
        {
            return value.ToFloat();
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single"/> to <see cref="NTwain.Data.TW_FIX32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TW_FIX32(float value)
        {
            return new TW_FIX32(value);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="NTwain.Data.TW_FIX32"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator double(TW_FIX32 value)
        {
            return value.ToFloat();
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double"/> to <see cref="NTwain.Data.TW_FIX32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TW_FIX32(double value)
        {
            return new TW_FIX32((float)value);
        }
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TW_FIX32 value1, TW_FIX32 value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TW_FIX32 value1, TW_FIX32 value2)
        {
            return !value1.Equals(value2);
        }
        #endregion

        #region IConvertable

        TypeCode IConvertible.GetTypeCode()
        {
            return TypeCode.Single;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return this != 0;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte((float)this);
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar((float)this);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime((float)this);
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal((float)this);
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble((float)this);
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16((float)this);
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32((float)this);
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64((float)this);
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return Convert.ToSByte((float)this);
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle((float)this);
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType((float)this, conversionType, CultureInfo.InvariantCulture);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return Convert.ToUInt16((float)this);
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return Convert.ToUInt32((float)this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return Convert.ToUInt64((float)this);
        }

        #endregion
    }

    /// <summary>
    /// Embedded in the <see cref="TW_IMAGELAYOUT"/> structure. 
    /// Defines a frame rectangle in ICapUnits coordinates.
    /// </summary>
    public partial struct TW_FRAME : IEquatable<TW_FRAME>
    {
        #region properties

        /// <summary>
        /// Value of the left-most edge of the rectangle.
        /// </summary>
        public float Left { get => _left; set => _left = value; }
        /// <summary>
        /// Value of the top-most edge of the rectangle.
        /// </summary>
        public float Top { get => _top; set => _top = value; }
        /// <summary>
        /// Value of the right-most edge of the rectangle.
        /// </summary>
        public float Right { get => _right; set => _right = value; }
        /// <summary>
        /// Value of the bottom-most edge of the rectangle.
        /// </summary>
        public float Bottom { get => _bottom; set => _bottom = value; }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "L={0}, T={1}, R={2}, B={3}", Left, Top, Right, Bottom);
        }

        #region equals

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TW_FRAME))
                return false;

            return Equals((TW_FRAME)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TW_FRAME other)
        {
            return _left == other._left && _top == other._top &&
                _right == other._right && _bottom == other._bottom;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _left.GetHashCode() ^ _top.GetHashCode() ^
                _right.GetHashCode() ^ _bottom.GetHashCode();
        }

        #endregion

        #region static stuff

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TW_FRAME value1, TW_FRAME value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TW_FRAME value1, TW_FRAME value2)
        {
            return !value1.Equals(value2);
        }
        #endregion
    }

    /// <summary>
    /// Embedded in the <see cref="TW_TRANSFORMSTAGE"/> structure that is embedded in the <see cref="TW_CIECOLOR"/>
    /// structure. Defines the parameters used for channel-specific transformation. The transform can be
    /// described either as an extended form of the gamma function or as a table look-up with linear
    /// interpolation.
    /// </summary>
    public partial struct TW_DECODEFUNCTION : IEquatable<TW_DECODEFUNCTION>
    {
        #region properties
        /// <summary>
        /// Starting input value of the extended gamma function. Defines the
        /// minimum input value of channel data.
        /// </summary>
        public float StartIn { get { return _startIn; } }//set { _startIn = value; } }
                                                         /// <summary>
                                                         /// Ending input value of the extended gamma function. Defines the maximum
                                                         /// input value of channel data.
                                                         /// </summary>
        public float BreakIn { get { return _breakIn; } }//set { _breakIn = value; } }
                                                         /// <summary>
                                                         /// The input value at which the transform switches from linear
                                                         /// transformation/interpolation to gamma transformation.
                                                         /// </summary>
        public float EndIn { get { return _endIn; } }//set { _endIn = value; } }
                                                     /// <summary>
                                                     /// Starting output value of the extended gamma function. Defines the
                                                     /// minimum output value of channel data.
                                                     /// </summary>
        public float StartOut { get { return _startOut; } }//set { _startOut = value; } }
                                                           /// <summary>
                                                           /// Ending output value of the extended gamma function. Defines the
                                                           /// maximum output value of channel data.
                                                           /// </summary>
        public float BreakOut { get { return _breakOut; } }//set { _breakOut = value; } }
                                                           /// <summary>
                                                           /// The output value at which the transform switches from linear
                                                           /// transformation/interpolation to gamma transformation.
                                                           /// </summary>
        public float EndOut { get { return _endOut; } }//set { _endOut = value; } }
                                                       /// <summary>
                                                       /// Constant value. The exponential used in the gamma function.
                                                       /// </summary>
        public float Gamma { get { return _gamma; } }//set { _gamma = value; } }
                                                     /// <summary>
                                                     /// The number of samples in the look-up table. Includes the values of StartIn
                                                     /// and EndIn. Zero-based index (actually, number of samples - 1). If zero, use
                                                     /// extended gamma, otherwise use table look-up.
                                                     /// </summary>
        public float SampleCount { get { return _sampleCount; } }//set { _sampleCount = value; } }
        #endregion

        #region equals

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TW_DECODEFUNCTION))
                return false;

            return Equals((TW_DECODEFUNCTION)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TW_DECODEFUNCTION other)
        {
            return _startIn == other._startIn && _startOut == other._startOut &&
                _breakIn == other._breakIn && _breakOut == other._breakOut &&
                _endIn == other._endIn && _endOut == other._endOut &&
                _gamma == other._gamma && _sampleCount == other._sampleCount;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _startIn.GetHashCode() ^ _startOut.GetHashCode() ^
                _breakIn.GetHashCode() ^ _breakOut.GetHashCode() ^
                _endIn.GetHashCode() ^ _endOut.GetHashCode() ^
                _gamma.GetHashCode() ^ _sampleCount.GetHashCode();
        }

        #endregion

        #region static stuff

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TW_DECODEFUNCTION value1, TW_DECODEFUNCTION value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TW_DECODEFUNCTION value1, TW_DECODEFUNCTION value2)
        {
            return !value1.Equals(value2);
        }
        #endregion
    }

    /// <summary>
    /// Specifies the parametrics used for either the ABC or LMN transform stages.
    /// </summary>
    public partial struct TW_TRANSFORMSTAGE
    {
        /// <summary>
        /// Channel-specific transform parameters.
        /// </summary>

        public TW_DECODEFUNCTION[] Decode { get { return _decode; } }//set { _decode = value; } }
                                                                     /// <summary>
                                                                     /// Flattened 3x3 matrix that specifies how channels are mixed in.
                                                                     /// </summary>

        public TW_FIX32[] Mix { get { return _mix; } }//set { _mix = value; } }

        /// <summary>
        /// Gets the <see cref="Mix"/> value as matrix.
        /// </summary>
        /// <returns></returns>
        public TW_FIX32[,] GetMixMatrix()
        {
            // from http://stackoverflow.com/questions/3845235/convert-array-to-matrix, haven't tested it
            TW_FIX32[,] mat = new TW_FIX32[3, 3];
            Buffer.BlockCopy(_mix, 0, mat, 0, _mix.Length * 4);
            return mat;
        }
    }

    //    /// <summary>
    //    /// Stores a group of associated individual values for a capability.
    //    /// The values need have no relationship to one another aside from 
    //    /// being used to describe the same "value" of the capability
    //    /// </summary>
    //    public partial class TW_ARRAY
    //    {
    //        /// <summary>
    //        /// The type of items in the array. All items in the array have the same size.
    //        /// </summary>
    //        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }

    //        ///// <summary>
    //        ///// How many items are in the array.
    //        ///// </summary>
    //        //public int Count { get { return (int)_numItems; } set { _numItems = (uint)value; } }

    //        /// <summary>
    //        /// Array of ItemType values starts here.
    //        /// </summary>

    //        public object[] ItemList
    //        {
    //            get { return _itemList; }
    //            set
    //            {
    //                _itemList = value;
    //                if (value != null) { _numItems = (uint)value.Length; }
    //                else { _numItems = 0; }
    //            }
    //        }
    //    }

    /// <summary>
    /// Used to get audio info.
    /// </summary>
    public partial struct TW_AUDIOINFO
    {
        /// <summary>
        /// Name of audio data.
        /// </summary>
        public string Name { get { return _name; } }
    }



    //    /// <summary>
    //    /// Used by an application either to get information about, or control the setting of a capability.
    //    /// </summary>
    //    public sealed partial class TW_CAPABILITY : IDisposable
    //    {
    //        #region ctors

    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="TW_CAPABILITY" /> class.
    //        /// </summary>
    //        /// <param name="capability">The capability.</param>
    //        public TW_CAPABILITY(CapabilityId capability)
    //        {
    //            Capability = capability;
    //            ContainerType = ContainerType.DontCare;
    //        }
    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="TW_CAPABILITY" /> class.
    //        /// </summary>
    //        /// <param name="capability">The capability.</param>
    //        /// <param name="value">The value.</param>
    //        public TW_CAPABILITY(CapabilityId capability, TW_ONEVALUE value)
    //        {
    //            Capability = capability;
    //            SetOneValue(value, PlatformInfo.Current.MemoryManager);
    //        }
    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="TW_CAPABILITY"/> class.
    //        /// </summary>
    //        /// <param name="capability">The capability.</param>
    //        /// <param name="value">The value.</param>
    //        /// <param name="type">The type.</param>
    //        public TW_CAPABILITY(CapabilityId capability, string value, ItemType type)
    //        {
    //            Capability = capability;
    //            SetOneValue(value, type, PlatformInfo.Current.MemoryManager);
    //        }
    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="TW_CAPABILITY"/> class.
    //        /// </summary>
    //        /// <param name="capability">The capability.</param>
    //        /// <param name="value">The value.</param>
    //        public TW_CAPABILITY(CapabilityId capability, TW_FRAME value)
    //        {
    //            Capability = capability;
    //            SetOneValue(value, PlatformInfo.Current.MemoryManager);
    //        }

    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="TW_CAPABILITY" /> class.
    //        /// </summary>
    //        /// <param name="capability">The capability.</param>
    //        /// <param name="value">The value.</param>
    //        public TW_CAPABILITY(CapabilityId capability, TW_ENUMERATION value)
    //        {
    //            Capability = capability;
    //            SetEnumValue(value, PlatformInfo.Current.MemoryManager);
    //        }

    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="TW_CAPABILITY" /> class.
    //        /// </summary>
    //        /// <param name="capability">The capability.</param>
    //        /// <param name="value">The value.</param>
    //        public TW_CAPABILITY(CapabilityId capability, TW_RANGE value)
    //        {
    //            Capability = capability;
    //            SetRangeValue(value, PlatformInfo.Current.MemoryManager);
    //        }

    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="TW_CAPABILITY" /> class.
    //        /// </summary>
    //        /// <param name="capability">The capability.</param>
    //        /// <param name="value">The value.</param>
    //        public TW_CAPABILITY(CapabilityId capability, TW_ARRAY value)
    //        {
    //            Capability = capability;
    //            SetArrayValue(value, PlatformInfo.Current.MemoryManager);
    //        }
    //        #endregion

    //        #region properties

    //        /// <summary>
    //        /// Id of capability to set or get.
    //        /// </summary>
    //        public CapabilityId Capability { get { return (CapabilityId)_cap; } set { _cap = (ushort)value; } }
    //        /// <summary>
    //        /// The type of the container structure referenced by the pointer internally. The container
    //        /// will be one of four types: <see cref="TW_ARRAY"/>, <see cref="TW_ENUMERATION"/>,
    //        /// <see cref="TW_ONEVALUE"/>, or <see cref="TW_RANGE"/>.
    //        /// </summary>
    //        public ContainerType ContainerType { get { return (ContainerType)_conType; } set { _conType = (ushort)value; } }

    //        internal IntPtr Container { get { return _hContainer; } }

    //        #endregion

    //        #region value functions

    //        void SetOneValue(string value, ItemType type, IMemoryManager memoryManager)
    //        {
    //            ContainerType = ContainerType.OneValue;
    //            switch (type)
    //            {
    //                case ItemType.String128:
    //                case ItemType.String255:
    //                case ItemType.String32:
    //                case ItemType.String64:

    //                    _hContainer = memoryManager.Allocate((uint)(Marshal.SizeOf(typeof(TW_FRAME)) + 2));
    //                    if (_hContainer != IntPtr.Zero)
    //                    {
    //                        IntPtr baseAddr = memoryManager.Lock(_hContainer);
    //                        int offset = 0;
    //                        baseAddr.WriteValue(ref offset, ItemType.UInt16, type);
    //                        baseAddr.WriteValue(ref offset, type, value);
    //                        memoryManager.Unlock(_hContainer);
    //                    }
    //                    break;
    //                default:
    //                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Type {0} is not valid for string.", type));
    //            }
    //        }
    //        void SetOneValue(TW_FRAME value, IMemoryManager memoryManager)
    //        {
    //            ContainerType = ContainerType.OneValue;

    //            _hContainer = memoryManager.Allocate((uint)(Marshal.SizeOf(typeof(TW_FRAME)) + 2));
    //            if (_hContainer != IntPtr.Zero)
    //            {
    //                IntPtr baseAddr = memoryManager.Lock(_hContainer);
    //                int offset = 0;
    //                baseAddr.WriteValue(ref offset, ItemType.UInt16, ItemType.Frame);
    //                baseAddr.WriteValue(ref offset, ItemType.Frame, value);
    //                memoryManager.Unlock(_hContainer);
    //            }
    //        }

    //        void SetOneValue(TW_ONEVALUE value, IMemoryManager memoryManager)
    //        {
    //            if (value == null) { throw new ArgumentNullException("value"); }
    //            ContainerType = ContainerType.OneValue;

    //            // since one value can only house UInt32 we will not allow type size > 4
    //            if (TypeExtensions.GetItemTypeSize(value.ItemType) > 4) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.BadValueType, "TW_ONEVALUE")); }

    //            _hContainer = memoryManager.Allocate((uint)Marshal.SizeOf(value));
    //            if (_hContainer != IntPtr.Zero)
    //            {
    //                Marshal.StructureToPtr(value, _hContainer, false);
    //            }
    //        }

    //        void SetEnumValue(TW_ENUMERATION value, IMemoryManager memoryManager)
    //        {
    //            if (value == null) { throw new ArgumentNullException("value"); }
    //            ContainerType = ContainerType.Enum;


    //            Int32 valueSize = TW_ENUMERATION.ItemOffset + value.ItemList.Length * TypeExtensions.GetItemTypeSize(value.ItemType);

    //            int offset = 0;
    //            _hContainer = memoryManager.Allocate((uint)valueSize);
    //            if (_hContainer != IntPtr.Zero)
    //            {
    //                IntPtr baseAddr = memoryManager.Lock(_hContainer);

    //                // can't safely use StructureToPtr here so write it our own
    //                baseAddr.WriteValue(ref offset, ItemType.UInt16, value.ItemType);
    //                baseAddr.WriteValue(ref offset, ItemType.UInt32, (uint)value.ItemList.Length);
    //                baseAddr.WriteValue(ref offset, ItemType.UInt32, value.CurrentIndex);
    //                baseAddr.WriteValue(ref offset, ItemType.UInt32, value.DefaultIndex);
    //                foreach (var item in value.ItemList)
    //                {
    //                    baseAddr.WriteValue(ref offset, value.ItemType, item);
    //                }
    //                memoryManager.Unlock(_hContainer);
    //            }
    //        }

    //        void SetRangeValue(TW_RANGE value, IMemoryManager memoryManager)
    //        {
    //            if (value == null) { throw new ArgumentNullException("value"); }
    //            ContainerType = ContainerType.Range;

    //            // since range value can only house UInt32 we will not allow type size > 4
    //            if (TypeExtensions.GetItemTypeSize(value.ItemType) > 4) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.BadValueType, "TW_RANGE")); }

    //            _hContainer = memoryManager.Allocate((uint)Marshal.SizeOf(value));
    //            if (_hContainer != IntPtr.Zero)
    //            {
    //                Marshal.StructureToPtr(value, _hContainer, false);
    //            }
    //        }

    //        void SetArrayValue(TW_ARRAY value, IMemoryManager memoryManager)
    //        {
    //            if (value == null) { throw new ArgumentNullException("value"); }
    //            ContainerType = ContainerType.Array;

    //            Int32 valueSize = 6 + value.ItemList.Length * TypeExtensions.GetItemTypeSize(value.ItemType);

    //            int offset = 0;
    //            _hContainer = memoryManager.Allocate((uint)valueSize);
    //            if (_hContainer != IntPtr.Zero)
    //            {
    //                IntPtr baseAddr = memoryManager.Lock(_hContainer);

    //                // can't safely use StructureToPtr here so write it our own
    //                baseAddr.WriteValue(ref offset, ItemType.UInt16, value.ItemType);
    //                baseAddr.WriteValue(ref offset, ItemType.UInt32, (uint)value.ItemList.Length);
    //                foreach (var item in value.ItemList)
    //                {
    //                    baseAddr.WriteValue(ref offset, value.ItemType, item);
    //                }
    //                memoryManager.Unlock(_hContainer);
    //            }
    //        }

    //        #endregion


    //        #region IDisposable Members

    //        /// <summary>
    //        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    //        /// </summary>
    //        public void Dispose()
    //        {
    //            Dispose(true);
    //            GC.SuppressFinalize(this);
    //        }

    //        void Dispose(bool disposing)
    //        {
    //            if (disposing) { }
    //            if (_hContainer != IntPtr.Zero)
    //            {
    //                PlatformInfo.Current.MemoryManager.Free(_hContainer);
    //                _hContainer = IntPtr.Zero;
    //            }
    //        }

    //        /// <summary>
    //        /// Finalizes an instance of the <see cref="TW_CAPABILITY"/> class.
    //        /// </summary>
    //        ~TW_CAPABILITY()
    //        {
    //            Dispose(false);
    //        }
    //        #endregion

    //    }

    /// <summary>
    /// Embedded in the <see cref="TW_CIECOLOR"/> structure;
    /// defines a CIE XYZ space tri-stimulus value.
    /// </summary>
    public partial struct TW_CIEPOINT : IEquatable<TW_CIEPOINT>
    {
        #region properties
        /// <summary>
        /// First tri-stimulus value of the CIE space representation.
        /// </summary>
        public float X { get { return _z; } }
        /// <summary>
        /// Second tri-stimulus value of the CIE space representation.
        /// </summary>
        public float Y { get { return _z; } }
        /// <summary>
        /// Third tri-stimulus value of the CIE space representation.
        /// </summary>
        public float Z { get { return _z; } }
        #endregion

        #region equals

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TW_CIEPOINT))
                return false;

            return Equals((TW_CIEPOINT)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TW_CIEPOINT other)
        {
            return _x == other._x && _y == other._y &&
                _z == other._z;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^
                _z.GetHashCode();
        }

        #endregion

        #region static stuff

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TW_CIEPOINT value1, TW_CIEPOINT value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TW_CIEPOINT value1, TW_CIEPOINT value2)
        {
            return !value1.Equals(value2);
        }
        #endregion
    }

    //    /// <summary>
    //    /// Defines the mapping from an RGB color space device into CIE 1931 (XYZ) color space.
    //    /// </summary>
    //    public partial struct TW_CIECOLOR
    //    {
    //        internal TW_CIECOLOR() { }

    //        /// <summary>
    //        /// Defines the original color space that was transformed into CIE XYZ. 
    //        /// This value is not set-able by the application. 
    //        /// </summary>
    //        public ushort ColorSpace { get { return _colorSpace; } }
    //        /// <summary>
    //        /// Used to indicate which data byte is taken first. If zero, then high byte is
    //        /// first. If non-zero, then low byte is first.
    //        /// </summary>
    //        public short LowEndian { get { return _lowEndian; } }
    //        /// <summary>
    //        /// If non-zero then color data is device-dependent and only ColorSpace is
    //        /// valid in this structure.
    //        /// </summary>
    //        public short DeviceDependent { get { return _deviceDependent; } }
    //        /// <summary>
    //        /// Version of the color space descriptor specification used to define the
    //        /// transform data. The current version is zero.
    //        /// </summary>
    //        public int VersionNumber { get { return _versionNumber; } }
    //        /// <summary>
    //        /// Describes parametrics for the first stage transformation of the Postscript
    //        /// Level 2 CIE color space transform process.
    //        /// </summary>
    //        public TW_TRANSFORMSTAGE StageABC { get { return _stageABC; } }
    //        /// <summary>
    //        /// Describes parametrics for the first stage transformation of the Postscript
    //        /// Level 2 CIE color space transform process.
    //        /// </summary>
    //        public TW_TRANSFORMSTAGE StageLMN { get { return _stageLMN; } }
    //        /// <summary>
    //        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of the
    //        /// diffused white point.
    //        /// </summary>
    //        public TW_CIEPOINT WhitePoint { get { return _whitePoint; } }
    //        /// <summary>
    //        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of the
    //        /// diffused black point.
    //        /// </summary>
    //        public TW_CIEPOINT BlackPoint { get { return _blackPoint; } }
    //        /// <summary>
    //        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of inkless
    //        /// "paper" from which the image was acquired.
    //        /// </summary>
    //        public TW_CIEPOINT WhitePaper { get { return _whitePaper; } }
    //        /// <summary>
    //        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of solid
    //        /// black ink on the "paper" from which the image was acquired.
    //        /// </summary>
    //        public TW_CIEPOINT BlackInk { get { return _blackInk; } }
    //        /// <summary>
    //        /// Optional table look-up values used by the decode function. Samples
    //        /// are ordered sequentially and end-to-end as A, B, C, L, M, and N.
    //        /// </summary>

    //        public TW_FIX32[] Samples { get { return _samples; } }
    //    }

    /// <summary>
    /// Allows for a data source and application to pass custom data to each other.
    /// </summary>
    partial struct TW_CUSTOMDSDATA
    {
        /// <summary>
        /// Length, in bytes, of data.
        /// </summary>
        public uint InfoLength { get { return _infoLength; } set { _infoLength = value; } }
        /// <summary>
        /// Handle to memory containing InfoLength bytes of data.
        /// </summary>
        public IntPtr hData { get { return _hData; } set { _hData = value; } }
    }

    /// <summary>
    /// Provides information about the Event that was raised by the Source. The Source should only fill
    /// in those fields applicable to the Event. The Application must only read those fields applicable to
    /// the Event.
    /// </summary>
    public partial struct TW_DEVICEEVENT
    {
        /// <summary>
        /// Defines event that has taken place.
        /// </summary>
        public DeviceEvent Event { get { return (DeviceEvent)_event; } }
        /// <summary>
        /// The name of the device that generated the event.
        /// </summary>
        public string DeviceName { get { return _deviceName; } }
        /// <summary>
        /// Battery minutes remaining. Valid for BatteryCheck event only.
        /// </summary>
        public int BatteryMinutes { get { return (int)_batteryMinutes; } }
        /// <summary>
        /// Battery percentage remaining. Valid for BatteryCheck event only.
        /// </summary>
        public int BatteryPercentage { get { return (int)_batteryPercentage; } }
        /// <summary>
        /// Current power supply in use. Valid for PowerSupply event only.
        /// </summary>
        public int PowerSupply { get { return (int)_powerSupply; } }
        /// <summary>
        /// Current X Resolution. Valid for Resolution event only.
        /// </summary>
        public float XResolution { get { return _xResolution; } }
        /// <summary>
        /// Current Y Resolution. Valid for Resolution event only.
        /// </summary>
        public float YResolution { get { return _yResolution; } }
        /// <summary>
        /// Current flash setting. Valid for BatteryCheck event only.
        /// </summary>
        public FlashedUsed FlashUsed2 { get { return (FlashedUsed)_flashUsed2; } }
        /// <summary>
        /// Number of images camera will capture. Valid for AutomaticCapture event only.
        /// </summary>
        public int AutomaticCapture { get { return (int)_automaticCapture; } }
        /// <summary>
        /// Number of seconds before first capture. Valid for AutomaticCapture event only.
        /// </summary>
        public int TimeBeforeFirstCapture { get { return (int)_timeBeforeFirstCapture; } }
        /// <summary>
        /// Hundredths of a second between captures. Valid for AutomaticCapture event only.
        /// </summary>
        public int TimeBetweenCaptures { get { return (int)_timeBetweenCaptures; } }
    }

    /// <summary>
    /// Embedded in the <see cref="TWGrayResponse"/>, <see cref="TW_PALETTE8"/>, and <see cref="TWRgbResponse"/> structures.
    /// This structure holds the tri-stimulus color palette information for <see cref="TW_PALETTE8"/> structures.
    /// The order of the channels shall match their alphabetic representation. That is, for RGB data, R
    /// shall be channel 1. For CMY data, C shall be channel 1. This allows the application and Source
    /// to maintain consistency. Grayscale data will have the same values entered in all three channels.
    /// </summary>
    public partial struct TW_ELEMENT8 : IEquatable<TW_ELEMENT8>
    {
        /// <summary>
        /// Value used to index into the color table.
        /// </summary>
        public byte Index { get { return _index; } set { _index = value; } }
        /// <summary>
        /// First tri-stimulus value (e.g. Red).
        /// </summary>
        public byte Channel1 { get { return _channel1; } set { _channel1 = value; } }
        /// <summary>
        /// Second tri-stimulus value (e.g Green).
        /// </summary>
        public byte Channel2 { get { return _channel2; } set { _channel2 = value; } }
        /// <summary>
        /// Third  tri-stimulus value (e.g Blue).
        /// </summary>
        public byte Channel3 { get { return _channel3; } set { _channel3 = value; } }


        #region equals

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TW_ELEMENT8))
                return false;

            return Equals((TW_ELEMENT8)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TW_ELEMENT8 other)
        {
            return _channel1 == other._channel1 && _channel2 == other._channel2 &&
                _channel3 == other._channel3 && _index == other._index;
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _channel1.GetHashCode() ^ _channel2.GetHashCode() ^
                _channel3.GetHashCode() ^ _index.GetHashCode();
        }

        /// <summary>
        /// Check for value equality.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static bool operator ==(TW_ELEMENT8 v1, TW_ELEMENT8 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Check for value inequality.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static bool operator !=(TW_ELEMENT8 v1, TW_ELEMENT8 v2)
        {
            return !(v1 == v2);
        }


        #endregion
    }

    //    /// <summary>
    //    /// An enumeration stores a list of individual values, with one of the items designated as the current
    //    /// value. There is no required order to the values in the list.
    //    /// </summary>
    //    public partial class TW_ENUMERATION
    //    {
    //        /// <summary>
    //        /// The type of items in the enumerated list. All items in the array have the same size.
    //        /// </summary>
    //        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }
    //        ///// <summary>
    //        ///// How many items are in the enumeration.
    //        ///// </summary>
    //        //public int Count { get { return (int)_numItems; } set { _numItems = (uint)value; } }
    //        /// <summary>
    //        /// The item number, or index (zero-based) into <see cref="ItemList"/>, of the "current"
    //        /// value for the capability.
    //        /// </summary>
    //        public int CurrentIndex { get { return (int)_currentIndex; } set { _currentIndex = (uint)value; } }
    //        /// <summary>
    //        /// The item number, or index (zero-based) into <see cref="ItemList"/>, of the "power-on"
    //        /// value for the capability.
    //        /// </summary>
    //        public int DefaultIndex { get { return (int)_defaultIndex; } set { _defaultIndex = (uint)value; } }
    //        /// <summary>
    //        /// The enumerated list: one value resides within each array element.
    //        /// </summary>

    //        public object[] ItemList
    //        {
    //            get { return _itemList; }
    //            set
    //            {
    //                _itemList = value;
    //                if (value != null) { _numItems = (uint)value.Length; }
    //                else { _numItems = 0; }
    //            }
    //        }

    //        /// <summary>
    //        /// Gets the byte offset of the item list from a Ptr to the first item.
    //        /// </summary>
    //        internal const int ItemOffset = 14;
    //    }


    /// <summary>
    /// Used on Windows and Macintosh pre OS X to pass application events/messages from the
    /// application to the Source.
    /// </summary>
    public partial struct TW_EVENT
    {
        /// <summary>
        /// A pointer to the event/message to be examined by the Source.
        /// Under Microsoft Windows, pEvent is a pMSG (pointer to a Microsoft
        /// Windows MSG struct). That is, the message the application received from
        /// GetMessage(). On the Macintosh, pEvent is a pointer to an EventRecord.
        /// </summary>
        public IntPtr pEvent { get { return _pEvent; } set { _pEvent = value; } }
        /// <summary>
        /// Any message the Source needs to send to the application in
        /// response to processing the event/message. The messages currently defined for
        /// this purpose are <see cref="Message.Null"/>, <see cref="Message.XferReady"/> 
        /// and <see cref="Message.CloseDSReq"/>.
        /// </summary>
        public Message TWMessage { get { return (Message)_tWMessage; } }
    }

    //    /// <summary>
    //    /// This structure is used to pass specific information between the data source and the application
    //    /// through <see cref="TW_EXTIMAGEINFO"/>.
    //    /// </summary>
    //    [DebuggerDisplay("ID = {InfoID}, Type = {ItemType}")]
    //    public partial struct TW_INFO
    //    {
    //        /// <summary>
    //        /// Tag identifying an information.
    //        /// </summary>
    //        public ExtendedImageInfo InfoID { get { return (ExtendedImageInfo)_infoID; } set { _infoID = (ushort)value; } }
    //        /// <summary>
    //        /// Item data type.
    //        /// </summary>
    //        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }
    //        /// <summary>
    //        /// Number of items.
    //        /// </summary>
    //        public ushort NumItems { get { return _numItems; } }

    //        /// <summary>
    //        /// This is the return code of availability of data for extended image attribute requested.
    //        /// </summary>
    //        /// <value>
    //        /// The return code.
    //        /// </value>
    //        public ReturnCode ReturnCode { get { return (ReturnCode)_returnCode; } }

    //        /// <summary>
    //        /// Contains either data or a handle to data. The field
    //        /// contains data if the total amount of data is less than or equal to four bytes. The
    //        /// field contains a handle if the total amount of data is more than four bytes.
    //        /// The amount of data is determined by multiplying NumItems times
    //        /// the byte size of the data type specified by ItemType.
    //        /// If the Item field contains a handle to data, then the Application is
    //        /// responsible for freeing that memory.
    //        /// </summary>
    //        public IntPtr Item { get { return _item; } internal set { _item = value; } }

    //        bool ItemIsPointer
    //        {
    //            get
    //            {
    //                return ItemType == Data.ItemType.Handle ||
    //                    (TypeExtensions.GetItemTypeSize(ItemType) * NumItems) > 4;// IntPtr.Size
    //            }
    //        }

    //        /// <summary>
    //        /// Try to reads the values from the <see cref="Item"/> property.
    //        /// </summary>
    //        /// <returns></returns>
    //        public IList<object> ReadValues()
    //        {
    //            var values = new List<object>();
    //            if (NumItems > 0)
    //            {
    //                if (ItemIsPointer)
    //                {
    //                    if (Item != IntPtr.Zero)
    //                    {
    //                        IntPtr lockPtr = IntPtr.Zero;
    //                        try
    //                        {
    //                            int offset = 0;
    //                            lockPtr = PlatformInfo.Current.MemoryManager.Lock(Item);

    //                            for (int i = 0; i < NumItems; i++)
    //                            {
    //                                values.Add(TypeExtensions.ReadValue(lockPtr, ref offset, ItemType));
    //                            }
    //                        }
    //                        finally
    //                        {
    //                            if (lockPtr != IntPtr.Zero)
    //                            {
    //                                PlatformInfo.Current.MemoryManager.Unlock(Item);
    //                                //PlatformInfo.Current.MemoryManager.Unlock(lockPtr);
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    // do the lame and create a ptr to the value so we can reuse TypeReader
    //                    IntPtr tempPtr = IntPtr.Zero;
    //                    try
    //                    {
    //                        tempPtr = Marshal.AllocHGlobal(IntPtr.Size);
    //                        Marshal.WriteIntPtr(tempPtr, Item);
    //                        int offset = 0;
    //                        values.Add(TypeExtensions.ReadValue(tempPtr, ref offset, ItemType));
    //                    }
    //                    finally
    //                    {
    //                        if (tempPtr != IntPtr.Zero)
    //                        {
    //                            Marshal.FreeHGlobal(tempPtr);
    //                        }
    //                    }
    //                }
    //            }
    //            return values;
    //        }

    //        #region IDisposable Members

    //        /// <summary>
    //        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    //        /// </summary>
    //        public void Dispose()
    //        {
    //            Dispose(true);
    //            //GC.SuppressFinalize(this);
    //        }

    //        private void Dispose(bool disposing)
    //        {
    //            if (disposing) { }

    //            if (ItemIsPointer && Item != IntPtr.Zero)
    //            {
    //                PlatformInfo.Current.MemoryManager.Free(Item);
    //            }
    //            Item = IntPtr.Zero;
    //        }


    //        //~TW_INFO()
    //        //{
    //        //    Dispose(false);
    //        //}
    //        #endregion
    //    }

    /// <summary>
    /// This structure is used to pass extended image information from the data source to application at
    /// the end of State 7. The application creates this structure at the end of State 7, when it receives
    /// XFERDONE. Application fills NumInfos for Number information it needs, and array of
    /// extended information attributes in Info[ ] array. Application, then, sends it down to the source
    /// using the above operation triplet. The data source then examines each Info, and fills the rest of
    /// data with information allocating memory when necessary.
    /// </summary>
    partial struct TW_EXTIMAGEINFO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TW_EXTIMAGEINFO"/> class.
        /// </summary>
        public static TW_EXTIMAGEINFO Create()
        {
            return new TW_EXTIMAGEINFO
            {
                _info = new TW_INFO[200]
            };
        }

        /// <summary>
        /// Number of information that application is requesting. This is filled by the
        /// application. If positive, then the application is requesting specific extended
        /// image information. The application should allocate memory and fill in the
        /// attribute tag for image information.
        /// </summary>
        public uint NumInfos
        {
            //get { return _numInfos; } 
            set { _numInfos = value; }
        }

        /// <summary>
        /// Array of information.
        /// </summary>

        public TW_INFO[] Info { get { return _info; } }
    }

    /// <summary>
    /// Provides information about the currently selected device.
    /// </summary>
    public partial struct TW_FILESYSTEM
    {
        /// <summary>
        /// The name of the input or source file.
        /// </summary>
        public string InputName
        {
            get { return _inputName; }
            set { _inputName = value; }
        }
        /// <summary>
        /// The result of an operation or the name of a destination file.
        /// </summary>
        public string OutputName
        {
            get { return _outputName; }
            set { _outputName = value; }
        }
        /// <summary>
        /// A pointer to Source specific data used to remember state
        /// information, such as the current directory.
        /// </summary>
        public IntPtr Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        /// When set to TRUE recursively apply the operation. (ex: deletes
        /// all subdirectories in the directory being deleted; or copies all
        /// sub-directories in the directory being copied.
        /// </summary>
        public bool Recursive
        {
            get { return _subdirectories == TwainConst.True; }
            set { _subdirectories = value ? TwainConst.True : TwainConst.False; }
        }

        /// <summary>
        /// Gets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public FileType FileType
        {
            get { return (FileType)_fileType; }
            set { _fileType = (int)value; }
        }

        /// <summary>
        /// If <see cref="NTwain.Data.FileType.Directory"/>, total size of media in bytes.
        /// If <see cref="NTwain.Data.FileType.Image"/>, size of image in bytes.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public uint Size { get { return _size; } set { _size = value; } }
        /// <summary>
        /// The create date of the file, in the form "YYYY/MM/DD
        /// HH:mm:SS:sss" where YYYY is the year, MM is the numerical
        /// month, DD is the numerical day, HH is the hour, mm is the
        /// minute, SS is the second, and sss is the millisecond.
        /// </summary>
        public string CreateTimeDate
        {
            get { return _createTimeDate; }
            set { _createTimeDate = value; }
        }
        /// <summary>
        /// Last date the file was modified. Same format as <see cref="CreateTimeDate"/>.
        /// </summary>
        public string ModifiedTimeDate
        {
            get { return _modifiedTimeDate; }
            set { _modifiedTimeDate = value; }
        }
        /// <summary>
        /// The bytes of free space left on the current device.
        /// </summary>
        public uint FreeSpace
        {
            get { return _freeSpace; }
            set { _freeSpace = value; }
        }
        /// <summary>
        /// An estimate of the amount of space a new image would take
        /// up, based on image layout, resolution and compression.
        /// Dividing this value into the FreeSpace will yield the
        /// approximate number of images that the Device has room for.
        /// </summary>
        public int NewImageSize
        {
            get { return _newImageSize; }
            set { _newImageSize = value; }
        }
        /// <summary>
        /// If applicable, return the number of <see cref="NTwain.Data.FileType.Image"/> files on the file system including those in all sub-directories.
        /// </summary>
        /// <value>
        /// The number of files.
        /// </value>
        public uint NumberOfFiles
        {
            get { return _numberOfFiles; }
            set { _numberOfFiles = value; }
        }
        /// <summary>
        /// The number of audio snippets associated with a file of type <see cref="NTwain.Data.FileType.Image"/>.
        /// </summary>
        /// <value>
        /// The number of snippets.
        /// </value>
        public uint NumberOfSnippets
        {
            get { return _numberOfSnippets; }
            set { _numberOfSnippets = value; }
        }
        /// <summary>
        /// Used to group cameras (ex: front/rear bitonal, front/rear grayscale...).
        /// </summary>
        public uint DeviceGroupMask
        {
            get { return _deviceGroupMask; }
            set { _deviceGroupMask = value; }
        }
    }

    //    ///// <summary>
    //    ///// This structure is used by the application to specify a set of mapping values to be applied to
    //    ///// grayscale data.
    //    ///// </summary>
    //    //public partial class TWGrayResponse
    //    //{
    //    //    /// <summary>
    //    //    /// Transfer curve descriptors. All three channels (Channel1, Channel2
    //    //    /// and Channel3) must contain the same value for every entry.
    //    //    /// </summary>

    //    //    public TW_ELEMENT8[] Response { get { return _response; } set { _response = value; } }
    //    //}

    /// <summary>
    /// A general way to describe the version of app or data source.
    /// </summary>
    public partial struct TW_VERSION : IEquatable<TW_VERSION>
    {
        /// <summary>
        /// This refers to your application or Source’s major revision number. e.g. The
        /// "2" in "version 2.1".
        /// </summary>
        public short Major
        {
            get { return (short)_majorNum; }
            internal set { _majorNum = (ushort)value; }
        }
        /// <summary>
        /// The incremental revision number of your application or Source. e.g. The "1"
        /// in "version 2.1".
        /// </summary>
        public short Minor
        {
            get { return (short)_minorNum; }
            internal set { _minorNum = (ushort)value; }
        }
        /// <summary>
        /// The primary language for your Source or application.
        /// </summary>
        public Language Language
        {
            get { return (Language)_language; }
            internal set { _language = (ushort)value; }
        }
        /// <summary>
        /// The primary country where your Source or application is intended to be
        /// distributed. e.g. Germany.
        /// </summary>
        public Country Country
        {
            get { return (Country)_country; }
            internal set { _country = (ushort)value; }
        }
        /// <summary>
        /// General information string - fill in as needed. e.g. "1.0b3 Beta release".
        /// </summary>
        public string Info
        {
            get { return _info; }
            internal set { _info.VerifyLengthUnder(TwainConst.String32 - 1); _info = value; }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1} {2}", Major, Minor, Info);
        }

        #region equals

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is TW_VERSION))
                return false;

            return Equals((TW_VERSION)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TW_VERSION other)
        {
            return _majorNum == other._majorNum && _minorNum == other._minorNum &&
                _language == other._language && _country == other._country &&
                string.Equals(_info, other._info);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return _majorNum.GetHashCode() ^ _minorNum.GetHashCode() ^
                _language.GetHashCode() ^ _country.GetHashCode() ^
                (_info == null ? 0 : _info.GetHashCode());
        }

        /// <summary>
        /// Check for value equality.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static bool operator ==(TW_VERSION v1, TW_VERSION v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Check for value inequality.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static bool operator !=(TW_VERSION v1, TW_VERSION v2)
        {
            return !(v1 == v2);
        }

        #endregion
    }

    /// <summary>
    /// Provides identification information about a TWAIN entity. Used to maintain consistent
    /// communication between entities.
    /// </summary>
    partial class TW_IDENTITY : ITW_IDENTITY
    {
        public DataGroups DataGroup
        {
            get { return (DataGroups)(_supportedGroups & 0xffff); }
            internal set { _supportedGroups = ((uint)value & 0xffff) | (0xffff0000 & _supportedGroups); }
        }

        public short ProtocolMajor
        {
            get { return (short)_protocolMajor; }
            internal set { _protocolMajor = (ushort)value; }
        }

        public short ProtocolMinor
        {
            get { return (short)_protocolMinor; }
            internal set { _protocolMinor = (ushort)value; }
        }

        public TW_VERSION Version
        {
            get { return _version; }
            internal set { _version = value; }
        }

        public DataFunctionalities DataFunctionalities
        {
            get { return (DataFunctionalities)(_supportedGroups & 0xffff0000); }
            internal set { _supportedGroups = ((uint)value & 0xffff0000) | (0x0000ffff & _supportedGroups); }
        }


        public string Manufacturer
        {
            get { return _manufacturer; }
            internal set { value.VerifyLengthUnder(TwainConst.String32 - 1); _manufacturer = value; }
        }

        public string ProductFamily
        {
            get { return _productFamily; }
            internal set { value.VerifyLengthUnder(TwainConst.String32 - 1); _productFamily = value; }
        }

        public string ProductName
        {
            get { return _productName; }
            internal set { value.VerifyLengthUnder(TwainConst.String32 - 1); _productName = value; }
        }
    }

    /// <summary>
    /// Describes the "real" image data, that is, the complete image being transferred between the
    /// Source and application. The Source may transfer the data in a different format--the information
    /// may be transferred in "strips" or "tiles" in either compressed or uncompressed form.
    /// </summary>
    public partial struct TW_IMAGEINFO
    {
        /// <summary>
        /// The number of pixels per ICapUnits in the horizontal direction. The
        /// current unit is assumed to be "inches" unless it has been otherwise
        /// negotiated between the application and Source.
        /// </summary>
        public float XResolution { get { return _xResolution; } }
        /// <summary>
        /// The number of pixels per ICapUnits in the vertical direction.
        /// </summary>
        public float YResolution { get { return _yResolution; } }
        /// <summary>
        /// How wide, in pixels, the entire image to be transferred is. If the Source
        /// doesn’t know, set this field to -1 (hand scanners may do this).
        /// </summary>
        public int ImageWidth { get { return _imageWidth; } }
        /// <summary>
        /// How tall/long, in pixels, the image to be transferred is. If the Source
        /// doesn’t know, set this field to -1 (hand scanners may do this).
        /// </summary>
        public int ImageLength { get { return _imageLength; } }
        /// <summary>
        /// The number of samples being returned. For R-G-B, this field would be
        /// set to 3. For C-M-Y-K, 4. For Grayscale or Black and White, 1.
        /// </summary>
        public short SamplesPerPixel { get { return _samplesPerPixel; } }

        /// <summary>
        /// For each sample, the number of bits of information. 24-bit R-G-B will
        /// typically have 8 bits of information in each sample (8+8+8). Some 8-bit
        /// color is sampled at 3 bits Red, 3 bits Green, and 2 bits Blue. Such a
        /// scheme would put 3, 3, and 2 into the first 3 elements of this array. The
        /// supplied array allows up to 8 samples. Samples are not limited to 8
        /// bits. However, both the application and Source must simultaneously
        /// support sample sizes greater than 8 bits per color.
        /// </summary>

        public short[] BitsPerSample { get { return _bitsPerSample; } }
        /// <summary>
        /// The number of bits in each image pixel (or bit depth). This value is
        /// invariant across the image. 24-bit R-G-B has BitsPerPixel = 24. 40-bit 
        /// CMYK has BitsPerPixel=40. 8-bit Grayscale has BitsPerPixel = 8. Black
        /// and White has BitsPerPixel = 1.
        /// </summary>
        public short BitsPerPixel { get { return _bitsPerPixel; } }
        /// <summary>
        /// If SamplesPerPixel > 1, indicates whether the samples follow one
        /// another on a pixel-by-pixel basis (R-G-B-R-G-B-R-G-B...) as is common
        /// with a one-pass scanner or all the pixels for each sample are grouped
        /// together (complete group of R, complete group of G, complete group of
        /// B) as is common with a three-pass scanner. If the pixel-by-pixel
        /// method (also known as "chunky") is used, the Source should set Planar
        /// = FALSE. If the grouped method (also called "planar") is used, the
        /// Source should set Planar = TRUE.
        /// </summary>
        public bool Planar { get { return _planar == TwainConst.True; } }
        /// <summary>
        /// This is the highest categorization for how the data being transferred
        /// should be interpreted by the application. This is how the application
        /// can tell if the data is Black and White, Grayscale, or Color. Currently,
        /// the only color type defined is "tri-stimulus", or color described by three
        /// characteristics. Most popular color description methods use tristimulus
        /// descriptors. For simplicity, the constant used to identify tristimulus
        /// color is called Rgb, for R-G-B color. There is no
        /// default for this value.
        /// </summary>
        public PixelType PixelType { get { return (PixelType)_pixelType; } }
        /// <summary>
        /// The compression method used to process the data being transferred.
        /// Default is no compression.
        /// </summary>
        public CompressionType Compression { get { return (CompressionType)_compression; } }
    }

    /// <summary>
    /// Involves information about the original size of the acquired image and its position on the
    /// original "page" relative to the "page’s" upper-left corner. Default measurements are in inches
    /// (units of measure can be changed by negotiating the ICapUnits capability). This information
    /// may be used by the application to relate the acquired (and perhaps processed image) to the
    /// original. Further, the application can, using this structure, set the size of the image it wants
    /// acquired.
    /// 
    /// Another attribute of this structure is the included frame, page, and document indexing
    /// information. Most Sources and applications, at least at first, will likely set all these fields to one.
    /// For Sources that can acquire more than one frame from a page in a single acquisition, the
    /// FrameNumber field will be handy. Sources that can acquire more than one page from a
    /// document feeder will use PageNumber and DocumentNumber. These fields will be especially
    /// useful for forms-processing applications and other applications with similar document tracking
    /// requirements.
    /// </summary>
    public partial struct TW_IMAGELAYOUT
    {
        /// <summary>
        /// Frame coords within larger document.
        /// </summary>
        public TW_FRAME Frame { get { return _frame; } set { _frame = value; } }
        /// <summary>
        /// The document number, assigned by the Source, that the acquired data
        /// originated on. Useful for grouping pages together.
        /// Initial value is 1. Increment when a new document is placed into the
        /// document feeder (usually tell this has happened when the feeder
        /// empties). Reset when no longer acquiring from the feeder.
        /// </summary>
        public int DocumentNumber { get { return (int)_documentNumber; } set { _documentNumber = (uint)value; } }
        /// <summary>
        /// The page which the acquired data was captured from. Useful for
        /// grouping Frames together that are in some way related. 
        /// Initial value is 1. Increment for each page fed from
        /// a page feeder. Reset when a new document is placed into the feeder.
        /// </summary>
        public int PageNumber { get { return (int)_pageNumber; } set { _pageNumber = (uint)value; } }
        /// <summary>
        /// Usually a chronological index of the acquired frame. These frames
        /// are related to one another in some way; usually they were acquired
        /// from the same page. The Source assigns these values. Initial value is
        /// 1. Reset when a new page is acquired from.
        /// </summary>
        public int FrameNumber { get { return (int)_frameNumber; } set { _frameNumber = (uint)value; } }
    }

    /// <summary>
    /// Provides information for managing memory buffers. Memory for transfer buffers is allocated
    /// by the application--the Source is asked to fill these buffers. This structure keeps straight which
    /// entity is responsible for deallocation.
    /// </summary>
    public partial struct TW_MEMORY
    {
        // not a class due to embedded

        /// <summary>
        /// Encodes which entity releases the buffer and how the buffer is referenced.
        /// </summary>
        public MemoryFlags Flags { get { return (MemoryFlags)_flags; } set { _flags = (uint)value; } }
        /// <summary>
        /// The size of the buffer in bytes. Should always be an even number and wordaligned.
        /// </summary>
        public uint Length { get { return _length; } set { _length = value; } }
        /// <summary>
        /// Reference to the buffer. May be a Pointer or a Handle (see Flags field to make
        /// this determination).
        /// </summary>
        public IntPtr TheMem { get { return _theMem; } set { _theMem = value; } }
    }

    /// <summary>
    /// Describes the form of the acquired data being passed from the Source to the application in memory transfer mode.
    /// </summary>
    public partial struct TW_IMAGEMEMXFER
    {
        /// <summary>
        /// The compression method used to process the data being transferred.
        /// </summary>
        public CompressionType Compression { get { return (CompressionType)_compression; } }
        /// <summary>
        /// The number of uncompressed bytes in each row of the piece of the image
        /// being described in this buffer.
        /// </summary>
        public uint BytesPerRow { get { return _bytesPerRow; } }
        /// <summary>
        /// The number of uncompressed columns (in pixels) in this buffer.
        /// </summary>
        public uint Columns { get { return _columns; } }
        /// <summary>
        /// The number or uncompressed rows (in pixels) in this buffer.
        /// </summary>
        public uint Rows { get { return _rows; } }
        /// <summary>
        /// How far, in pixels, the left edge of the piece of the image being described
        /// by this structure is inset from the "left" side of the original image. If the
        /// Source is transferring in "strips", this value will equal zero. If the Source
        /// is transferring in "tiles", this value will often be non-zero.
        /// </summary>
        public uint XOffset { get { return _xOffset; } }
        /// <summary>
        /// Same idea as XOffset, but the measure is in pixels from the "top" of the
        /// original image to the upper edge of this piece.
        /// </summary>
        public uint YOffset { get { return _yOffset; } }
        /// <summary>
        /// The number of bytes written into the transfer buffer. This field must
        /// always be filled in correctly, whether compressed or uncompressed data
        /// is being transferred.
        /// </summary>
        public uint BytesWritten { get { return _bytesWritten; } }
        /// <summary>
        /// A structure of type <see cref="TW_MEMORY"/> describing who must dispose of the
        /// buffer, the actual size of the buffer, in bytes, and where the buffer is
        /// located in memory.
        /// </summary>
        internal TW_MEMORY Memory { get { return _memory; } set { _memory = value; } }
    }

    /// <summary>
    /// Describes the information necessary to transfer a JPEG-compressed image during a buffered
    /// transfer. Images compressed in this fashion will be compatible with the JPEG File Interchange
    /// Format, version 1.1.
    /// </summary>
    public partial struct TW_JPEGCOMPRESSION
    {
        /// <summary>
        /// Defines the color space in which the
        /// compressed components are stored.
        /// </summary>
        public PixelType ColorSpace { get { return (PixelType)_colorSpace; } set { _colorSpace = (ushort)value; } }
        /// <summary>
        /// Encodes the horizontal and vertical subsampling in the form
        /// ABCDEFGH, where ABCD are the high-order four nibbles which
        /// represent the horizontal subsampling and EFGH are the low-order four
        /// nibbles which represent the vertical subsampling. Each nibble may
        /// have a value of 0, 1, 2, 3, or 4. However, max(A,B,C,D) * max(E,F,G,H)
        /// must be less than or equal to 10. Subsampling is irrelevant for single
        /// component images. Therefore, the corresponding nibbles should be set
        /// to 1. e.g. To indicate subsampling two Y for each U and V in a YUV
        /// space image, where the same subsampling occurs in both horizontal
        /// and vertical axes, this field would hold 0x21102110. For a grayscale
        /// image, this field would hold 0x10001000. A CMYK image could hold
        /// 0x11111111.
        /// </summary>
        public uint Subsampling { get { return _subSampling; } set { _subSampling = value; } }
        /// <summary>
        /// Number of color components in the image to be compressed.
        /// </summary>
        public ushort NumComponents { get { return _numComponents; } set { _numComponents = value; } }
        /// <summary>
        /// Number of MDUs (Minimum Data Units) between restart markers.
        /// Default is 0, indicating that no restart markers are used. An MDU is
        /// defined for interleaved data (i.e. R-G-B, Y-U-V, etc.) as a minimum
        /// complete set of 8x8 component blocks.
        /// </summary>
        public ushort RestartFrequency { get { return _restartFrequency; } set { _restartFrequency = value; } }
        /// <summary>
        /// Mapping of components to Quantization tables.
        /// </summary>

        public ushort[] QuantMap { get { return _quantMap; } set { _quantMap = value; } }
        /// <summary>
        /// Quantization tables.
        /// </summary>

        public TW_MEMORY[] QuantTable { get { return _quantTable; } set { _quantTable = value; } }
        /// <summary>
        /// Mapping of components to Huffman tables. Null entries signify
        /// selection of the default tables.
        /// </summary>

        public ushort[] HuffmanMap { get { return _huffmanMap; } set { _huffmanMap = value; } }
        /// <summary>
        /// DC Huffman tables. Null entries signify selection of the default tables.
        /// </summary>

        public TW_MEMORY[] HuffmanDC { get { return _huffmanDC; } set { _huffmanDC = value; } }
        /// <summary>
        /// AC Huffman tables. Null entries signify selection of the default tables.
        /// </summary>

        public TW_MEMORY[] HuffmanAC { get { return _huffmanAC; } set { _huffmanAC = value; } }
    }

    public partial struct TW_METRICS
    {
        public uint SizeOf => _sizeOf;
        public uint ImageCount => _imageCount;
        public uint SheetCount => _sheetCount;
    }

    //    /// <summary>
    //    /// Container for one value.
    //    /// </summary>
    //    public partial class TW_ONEVALUE
    //    {
    //        /// <summary>
    //        /// The type of the item.
    //        /// </summary>
    //        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }
    //        /// <summary>
    //        /// The value.
    //        /// </summary>
    //        public uint Item { get { return _item; } set { _item = value; } }
    //    }


    /// <summary>
    /// This structure holds the color palette information for buffered memory transfers of type
    /// ICapPixelType = Palette.
    /// </summary>
    public partial struct TW_PALETTE8
    {
        /// <summary>
        /// Number of colors in the color table; maximum index into the color table
        /// should be one less than this (since color table indexes are zero-based).
        /// </summary>
        public ushort NumColors { get { return _numColors; } set { _numColors = value; } }
        /// <summary>
        /// Specifies type of palette.
        /// </summary>
        public PaletteType PaletteType { get { return (PaletteType)_paletteType; } set { _paletteType = (ushort)value; } }
        /// <summary>
        /// Array of palette values.
        /// </summary>

        public TW_ELEMENT8[] Colors { get { return _colors; } set { _colors = value; } }
    }

    /// <summary>
    /// Used to bypass the TWAIN protocol when communicating with a device. All memory must be
    /// allocated and freed by the Application.
    /// </summary>
    public partial struct TW_PASSTHRU
    {
        /// <summary>
        /// Pointer to Command buffer.
        /// </summary>
        public IntPtr pCommand { get { return _pCommand; } set { _pCommand = value; } }
        /// <summary>
        /// Number of bytes in Command buffer.
        /// </summary>
        public uint CommandBytes { get { return _commandBytes; } set { _commandBytes = value; } }
        /// <summary>
        /// Defines the direction of data flow.
        /// </summary>
        public Direction Direction { get { return (Direction)_direction; } set { _direction = (int)value; } }
        /// <summary>
        /// Pointer to Data buffer.
        /// </summary>
        public IntPtr pData { get { return _pData; } set { _pData = value; } }
        /// <summary>
        /// Number of bytes in Data buffer.
        /// </summary>
        public uint DataBytes { get { return _dataBytes; } set { _dataBytes = value; } }
        /// <summary>
        /// Number of bytes successfully transferred.
        /// </summary>
        public uint DataBytesXfered { get { return _dataBytesXfered; } set { _dataBytesXfered = value; } }
    }

    /// <summary>
    /// This structure tells the application how many more complete transfers the Source currently has
    /// available.
    /// </summary>
    partial struct TW_PENDINGXFERS
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="TW_PENDINGXFERS"/> class.
        ///// </summary>
        //public TW_PENDINGXFERS()
        //{
        //    _count = TwainConst.DontCare16;
        //}

        /// <summary>
        /// The number of complete transfers a Source has available for the application it is
        /// connected to. If no more transfers are available, set to zero. If an unknown and
        /// non-zero number of transfers are available, set to -1.
        /// </summary>
        public int Count { get { return _count == TwainConst.DontCare16 ? -1 : (int)_count; } }
        /// <summary>
        /// The application should check this field if the CapJobControl is set to other
        /// than None. If this is not 0, the application should expect more data
        /// from the driver according to CapJobControl settings.
        /// </summary>
        public EndXferJob EndOfJob { get { return (EndXferJob)_eOJ; } }
    }


    //    /// <summary>
    //    /// Container for a range of values.
    //    /// </summary>
    //    public partial class TW_RANGE
    //    {
    //        /// <summary>
    //        /// The type of items in the list.
    //        /// </summary>
    //        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }
    //        /// <summary>
    //        /// The least positive/most negative value of the range.
    //        /// </summary>
    //        public uint MinValue { get { return _minValue; } set { _minValue = value; } }
    //        /// <summary>
    //        /// The most positive/least negative value of the range.
    //        /// </summary>
    //        public uint MaxValue { get { return _maxValue; } set { _maxValue = value; } }
    //        /// <summary>
    //        /// The delta between two adjacent values of the range.
    //        /// e.g. Item2 - Item1 = StepSize;
    //        /// </summary>
    //        public uint StepSize { get { return _stepSize; } set { _stepSize = value; } }
    //        /// <summary>
    //        /// The device’s "power-on" value for the capability. If the application is
    //        /// performing a MSG_SET operation and isn’t sure what the default
    //        /// value is, set this field to <see cref="TwainConst.DontCare32"/>.
    //        /// </summary>
    //        public uint DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }
    //        /// <summary>
    //        /// The value to which the device (or its user interface) is currently set to
    //        /// for the capability.
    //        /// </summary>
    //        public uint CurrentValue { get { return _currentValue; } set { _currentValue = value; } }
    //    }

    //    ///// <summary>
    //    ///// This structure is used by the application to specify a set of mapping values to be applied to RGB
    //    ///// color data. Use this structure for RGB data whose bit depth is up to, and including, 8-bits.
    //    ///// The number of elements in the array is determined by <see cref="TW_IMAGEINFO.BitsPerPixel"/>—the number of
    //    ///// elements is 2 raised to the power of <see cref="TW_IMAGEINFO.BitsPerPixel"/>.
    //    ///// </summary>
    //    //public partial class TWRgbResponse
    //    //{
    //    //    /// <summary>
    //    //    /// Transfer curve descriptors. To minimize color shift problems, writing the
    //    //    /// same values into each channel is desirable.
    //    //    /// </summary>

    //    //    public TW_ELEMENT8[] Response { get { return _response; } set { _response = value; } }
    //    //}

    /// <summary>
    /// Describes the file format and file specification information for a transfer through a disk file.
    /// </summary>
    public partial struct TW_SETUPFILEXFER
    {
        /// <summary>
        /// A complete file specifier to the target file. On Windows, be sure to include the
        /// complete pathname.
        /// </summary>
        public string FileName { get { return _fileName; } set { value.VerifyLengthUnder(TwainConst.String255 - 1); _fileName = value; } }
        /// <summary>
        /// The format of the file the Source is to fill. 
        /// </summary>
        public FileFormat Format { get { return (FileFormat)_format; } set { _format = (ushort)value; } }

        /// <summary>
        /// The volume reference number for the file. This applies to Macintosh only. On
        /// Windows, fill the field with -1.
        /// </summary>
        public short VRefNum { get { return _vRefNum; } set { _vRefNum = value; } }
    }


    /// <summary>
    /// Provides the application information about the Source’s requirements and preferences
    /// regarding allocation of transfer buffer(s).
    /// </summary>
    public partial struct TW_SETUPMEMXFER
    {
        /// <summary>
        /// The size of the smallest transfer buffer, in bytes, that a Source can be
        /// successful with. This will typically be the number of bytes in an
        /// uncompressed row in the block to be transferred. An application should
        /// never allocate a buffer smaller than this.
        /// </summary>
        public uint MinBufferSize { get { return _minBufSize; } }
        /// <summary>
        /// The size of the largest transfer buffer, in bytes, that a Source can fill. If a
        /// Source can fill an arbitrarily large buffer, it might set this field to negative 1 to
        /// indicate this (a hand-held scanner might do this, depending on how long its
        /// cord is). Other Sources, such as frame grabbers, cannot fill a buffer larger than
        /// a certain size. Allocation of a transfer buffer larger than this value is wasteful.
        /// </summary>
        public uint MaxBufferSize { get { return _maxBufSize; } }
        /// <summary>
        /// The size of the optimum transfer buffer, in bytes. A smart application will
        /// allocate transfer buffers of this size, if possible. Buffers of this size will
        /// optimize the Source’s performance. Sources should be careful to put
        /// reasonable values in this field. Buffers that are 10’s of kbytes will be easier for
        /// applications to allocate than buffers that are 100’s or 1000’s of kbytes.
        /// </summary>
        public uint Preferred { get { return _preferred; } }
    }

    /// <summary>
    /// Describes the status of a source.
    /// </summary>
    public partial struct TW_STATUS
    {
        /// <summary>
        /// Condition Code describing the status.
        /// </summary>
        public ConditionCode ConditionCode { get { return (ConditionCode)_conditionCode; } }
        /// <summary>
        /// Valid for TWAIN 2.1 and later. This field contains additional
        /// scanner-specific data. If there is no data, then this value must be zero.
        /// </summary>
        public ushort Data { get { return _data; } }
    }

    //    /// <summary>
    //    /// Translates the contents of Status into a localized UTF8string, with the total number of bytes
    //    /// in the string.
    //    /// </summary>
    //    public sealed partial struct TW_STATUSUTF8 : IDisposable
    //    {
    //        /// <summary>
    //        /// <see cref="TW_STATUS"/> data received from a previous call.
    //        /// </summary>
    //        public TW_STATUS Status
    //        {
    //            get { return new TW_STATUS(_conditionCode, _data); }
    //        }

    //        /// <summary>
    //        /// Total number of bytes in the UTF8string, plus the terminating NULL byte. 
    //        /// This is not the same as the total number of characters in the string.
    //        /// </summary>
    //        public int Size { get { return (int)_size; } }

    //        /// <summary>
    //        /// TW_HANDLE to a UTF-8 encoded localized string (based on 
    //        /// TW_IDENTITY.Language or CapLanguage). The Source allocates
    //        /// it, the Application frees it.
    //        /// </summary>
    //        public IntPtr UTF8StringPtr { get { return _uTF8string; } }

    //        /// <summary>
    //        /// Gets the actual string from the pointer. This may be incorrect.
    //        /// </summary>
    //        /// <returns></returns>
    //        public string TryGetString()
    //        {
    //            if (_uTF8string != IntPtr.Zero)
    //            {
    //                var sb = new StringBuilder(Size - 1);
    //                byte bt;
    //                while (sb.Length < _size &&
    //                    (bt = Marshal.ReadByte(_uTF8string, sb.Length)) != 0)
    //                {
    //                    sb.Append((char)bt);
    //                }
    //                return sb.ToString();
    //            }
    //            return null;
    //        }


    //        #region IDisposable Members

    //        /// <summary>
    //        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    //        /// </summary>
    //        public void Dispose()
    //        {
    //            Dispose(true);
    //            GC.SuppressFinalize(this);
    //        }

    //        void Dispose(bool disposing)
    //        {
    //            if (disposing)
    //            {

    //            }
    //            if (_uTF8string != IntPtr.Zero)
    //            {
    //                PlatformInfo.Current.MemoryManager.Free(_uTF8string);
    //                _uTF8string = IntPtr.Zero;
    //            }
    //        }

    //        /// <summary>
    //        /// Finalizes an instance of the <see cref="TW_STATUSUTF8"/> class.
    //        /// </summary>
    //        ~TW_STATUSUTF8()
    //        {
    //            Dispose(false);
    //        }

    //        #endregion
    //    }

    /// <summary>
    /// Provides identification information about a TWAIN entity. Used to maintain consistent
    /// communication between entities.
    /// </summary>
    public partial struct TW_TWAINDIRECT
    {
        public TW_TWAINDIRECT(ushort manager, IntPtr send, uint sendSize)
        {
            _SizeOf = (uint)Marshal.SizeOf(typeof(TW_TWAINDIRECT));
            _CommunicationManager = manager;
            _Send = send;
            _SendSize = sendSize;
            _Receive = IntPtr.Zero;
            _ReceiveSize = 0;
        }

        /// <summary>
        /// The interpretation of the Send data may be influenced by the communication
        /// manager that is helping the application connect to the scanner.
        /// For instance, a task action to “scan” may need to be ignored under some
        /// circumstances.
        /// </summary>
        public ushort CommunicationManager { get => _CommunicationManager; }
        /// <summary>
        /// A handle to data to be sent from the application to the driver. The application
        /// owns this handle.If there is no data, this field is set to NULL.
        /// </summary>
        public IntPtr Send { get => _Send; set => _Send = value; }
        /// <summary>
        /// The number of bytes in the Send buffer.
        /// If there is no data this field is set to 0.
        /// </summary>
        public uint SendSize { get => _SendSize; set => _SendSize = value; }
        /// <summary>
        /// A handle to data sent from the driver to the application.
        /// The driver creates this handle, the application must free it.
        /// If there is no data this field is set to NULL.
        /// </summary>
        public IntPtr Receive { get => _Receive; }
        /// <summary>
        /// The number of bytes in the Receive buffer, set by the driver.
        /// If there is no data this field is set to 0.
        /// </summary>
        public uint ReceiveSize { get => _ReceiveSize; }
    }

    /// <summary>
    /// This structure is used to handle the user interface coordination between an application and a
    /// Source.
    /// </summary>
    partial struct TW_USERINTERFACE
    {
        /// <summary>
        /// Set to TRUE by the application if the Source should activate its built-in user
        /// interface. Otherwise, set to FALSE. Note that not all sources support ShowUI =
        /// FALSE.
        /// </summary>
        public bool ShowUI
        {
            get { return _showUI > 0; }
            set { _showUI = value ? TwainConst.True : TwainConst.False; }
        }
        /// <summary>
        /// If ShowUI is TRUE, then an application setting this to TRUE requests the Source to
        /// run Modal.
        /// </summary>
        public bool ModalUI
        {
            //get { return _modalUI > 0; } 
            set { _modalUI = value ? TwainConst.True : TwainConst.False; }
        }
        /// <summary>
        /// Microsoft Windows only: Application’s window handle. The Source designates
        /// the hWnd as its parent when creating the Source dialog.
        /// </summary>
        public IntPtr hParent
        {
            //get { return _hParent; } 
            set { _hParent = value; }
        }
    }



    /// <summary>
    /// Provides entry points required by TWAIN 2.0 Applications and Sources.
    /// </summary>
    partial struct TW_ENTRYPOINT : IMemoryManager
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="TW_ENTRYPOINT"/> class.
        ///// </summary>
        //public TW_ENTRYPOINT()
        //{
        //    _size = (uint)Marshal.SizeOf(this);
        //}

        #region IMemoryManager Members

        public IntPtr Allocate(uint size)
        {
            return DSM_MemAllocate(size);
        }

        public void Free(IntPtr handle)
        {
            DSM_MemFree(handle);
        }

        public IntPtr Lock(IntPtr handle)
        {
            return DSM_MemLock(handle);
        }

        public void Unlock(IntPtr handle)
        {
            DSM_MemUnlock(handle);
        }

        #endregion
    }


    /// <summary>
    /// The range of colors specified by this structure is replaced with Replacement grayscale value in the
    /// binary image. The color is specified in HSV color space.
    /// </summary>
    public partial struct TW_FILTER_DESCRIPTOR
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="TW_FILTER_DESCRIPTOR"/> struct.
        ///// </summary>
        //public TW_FILTER_DESCRIPTOR()
        //{
        //    _size = (uint)Marshal.SizeOf(this);
        //}

        /// <summary>
        /// Size of this structure in bytes.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public uint Size { get { return _size; } set { _size = value; } }
        /// <summary>
        /// Hue starting number. Valid values 0 to 3600 (0° to 360°).
        /// </summary>
        public uint HueStart { get { return _hueStart; } set { _hueStart = value; } }
        /// <summary>
        /// Hue ending number. Valid values 0 to 3600 (0° to 360°).
        /// </summary>
        public uint HueEnd { get { return _hueEnd; } set { _hueEnd = value; } }
        /// <summary>
        /// Saturation starting number. Valid values 0 to 1000 (0% to 100%).
        /// </summary>
        public uint SaturationStart { get { return _saturationStart; } set { _saturationStart = value; } }
        /// <summary>
        /// Saturation ending number. Valid values 0 to 1000 (0% to 100%).
        /// </summary>
        public uint SaturationEnd { get { return _saturationEnd; } set { _saturationEnd = value; } }
        /// <summary>
        /// Luminosity starting number. Valid values 0 to 1000 (0% to 100%).
        /// </summary>
        public uint ValueStart { get { return _valueStart; } set { _valueStart = value; } }
        /// <summary>
        /// Luminosity ending number. Valid values 0 to 1000 (0% to 100%).
        /// </summary>
        public uint ValueEnd { get { return _valueEnd; } set { _valueEnd = value; } }
        /// <summary>
        /// Replacement grayscale value. Valid values 0 to (2^32)–1 (Maximum value
        /// depends on grayscale bit depth).
        /// </summary>
        public uint Replacement { get { return _replacement; } set { _replacement = value; } }
    }

    /// <summary>
    /// Specifies the filter to be applied during image acquisition. More than one descriptor can be
    /// specified. All descriptors are applied with an OR statement.
    /// </summary>
    public partial struct TW_FILTER
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="TW_FILTER"/> class.
        ///// </summary>
        //public TW_FILTER()
        //{
        //    _size = (uint)Marshal.SizeOf(this);
        //}

        /// <summary>
        /// Number of descriptors in hDescriptors array.
        /// </summary>
        /// <value>
        /// The descriptor count.
        /// </value>
        public uint DescriptorCount { get { return _descriptorCount; } set { _descriptorCount = value; } }
        /// <summary>
        /// Maximum possible descriptors. Valid only for GET and GETDEFAULT operations.
        /// </summary>
        /// <value>
        /// The maximum descriptor count.
        /// </value>
        public uint MaxDescriptorCount { get { return _maxDescriptorCount; } set { _maxDescriptorCount = value; } }
        /// <summary>
        /// If the value is 0 filter will check if current pixel color is inside the area
        /// specified by the descriptor. If the value is 1 it will check if it is outside
        /// of this area.
        /// </summary>
        /// <value>
        /// The condition.
        /// </value>
        public uint Condition { get { return _condition; } set { _condition = value; } }
        /// <summary>
        /// Handle to array of <see cref="TW_FILTER_DESCRIPTOR"/>.
        /// </summary>
        /// <value>
        /// The descriptors.
        /// </value>
        public IntPtr hDescriptors { get { return _hDescriptors; } set { _hDescriptors = value; } }
    }


}
