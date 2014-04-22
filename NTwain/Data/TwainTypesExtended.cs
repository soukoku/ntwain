using NTwain.Internals;
using NTwain.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace NTwain.Data
{

    //// This file contains custom logic added to the twain types.
    //// Separating the field definitions out makes finding all the
    //// custom code logic easier. Mostly this is just making the fields
    //// into .net friendly properties.

    //// potentially unit tests for the twain types only need to target 
    //// code in this file since everything else is just interop and 
    //// field definitions (pretty much have to hope it's correct).


    /// <summary>
    /// Stores a fixed point number. This can be implicitly converted 
    /// to a float in dotnet.
    /// </summary>
    public partial struct TWFix32 : IEquatable<TWFix32>, IConvertible
    {
        // the conversion logic is found in the spec.

        float ToFloat()
        {
            return (float)_whole + _frac / 65536f;
        }
        TWFix32(float value)
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
        /// The Whole part of the floating point number. This number is signed.
        /// </summary>
        public short Whole { get { return _whole; } set { _whole = value; } }
        /// <summary>
        /// The Fractional part of the floating point number. This number is unsigned.
        /// </summary>
        public ushort Fraction { get { return _frac; } set { _frac = value; } }

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
            if (!(obj is TWFix32))
                return false;

            return Equals((TWFix32)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TWFix32 other)
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
        /// Performs an implicit conversion from <see cref="NTwain.Data.TWFix32"/> to <see cref="System.Single"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator float(TWFix32 value)
        {
            return value.ToFloat();
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Single"/> to <see cref="NTwain.Data.TWFix32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TWFix32(float value)
        {
            return new TWFix32(value);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="NTwain.Data.TWFix32"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator double(TWFix32 value)
        {
            return value.ToFloat();
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Double"/> to <see cref="NTwain.Data.TWFix32"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator TWFix32(double value)
        {
            return new TWFix32((float)value);
        }
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(TWFix32 value1, TWFix32 value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TWFix32 value1, TWFix32 value2)
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
    /// Embedded in the <see cref="TWImageLayout"/> structure. 
    /// Defines a frame rectangle in ICapUnits coordinates.
    /// </summary>
    public partial struct TWFrame : IEquatable<TWFrame>
    {
        #region properties

        /// <summary>
        /// Value of the left-most edge of the rectangle.
        /// </summary>
        public float Left { get { return _left; } set { _left = value; } }
        /// <summary>
        /// Value of the top-most edge of the rectangle.
        /// </summary>
        public float Top { get { return _top; } set { _top = value; } }
        /// <summary>
        /// Value of the right-most edge of the rectangle.
        /// </summary>
        public float Right { get { return _right; } set { _right = value; } }
        /// <summary>
        /// Value of the bottom-most edge of the rectangle.
        /// </summary>
        public float Bottom { get { return _bottom; } set { _bottom = value; } }

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
            if (!(obj is TWFrame))
                return false;

            return Equals((TWFrame)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TWFrame other)
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
        public static bool operator ==(TWFrame value1, TWFrame value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TWFrame value1, TWFrame value2)
        {
            return !value1.Equals(value2);
        }
        #endregion
    }

    /// <summary>
    /// Embedded in the <see cref="TWTransformStage"/> structure that is embedded in the <see cref="TWCieColor"/>
    /// structure. Defines the parameters used for channel-specific transformation. The transform can be
    /// described either as an extended form of the gamma function or as a table look-up with linear
    /// interpolation.
    /// </summary>
    public partial struct TWDecodeFunction : IEquatable<TWDecodeFunction>
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
            if (!(obj is TWDecodeFunction))
                return false;

            return Equals((TWDecodeFunction)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TWDecodeFunction other)
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
        public static bool operator ==(TWDecodeFunction value1, TWDecodeFunction value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TWDecodeFunction value1, TWDecodeFunction value2)
        {
            return !value1.Equals(value2);
        }
        #endregion
    }

    /// <summary>
    /// Specifies the parametrics used for either the ABC or LMN transform stages.
    /// </summary>
    public partial struct TWTransformStage
    {
        /// <summary>
        /// Channel-specific transform parameters.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWDecodeFunction[] Decode { get { return _decode; } }//set { _decode = value; } }
        /// <summary>
        /// Flattened 3x3 matrix that specifies how channels are mixed in.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWFix32[] Mix { get { return _mix; } }//set { _mix = value; } }

        /// <summary>
        /// Gets the <see cref="Mix"/> value as matrix.
        /// </summary>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Body"), 
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional", MessageId = "Return")]
        public TWFix32[,] GetMixMatrix()
        {
            // from http://stackoverflow.com/questions/3845235/convert-array-to-matrix, haven't tested it
            TWFix32[,] mat = new TWFix32[3, 3]; 
            Buffer.BlockCopy(_mix, 0, mat, 0, _mix.Length * 4);
            return mat;
        }
    }

    /// <summary>
    /// Stores a group of associated individual values for a capability.
    /// The values need have no relationship to one another aside from 
    /// being used to describe the same "value" of the capability
    /// </summary>
    public partial class TWArray
    {
        /// <summary>
        /// The type of items in the array. All items in the array have the same size.
        /// </summary>
        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }

        ///// <summary>
        ///// How many items are in the array.
        ///// </summary>
        //public int Count { get { return (int)_numItems; } set { _numItems = (uint)value; } }

        /// <summary>
        /// Array of ItemType values starts here.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[] ItemList
        {
            get { return _itemList; }
            set
            {
                _itemList = value;
                if (value != null) { _numItems = (uint)value.Length; }
                else { _numItems = 0; }
            }
        }
    }

    /// <summary>
    /// Used to get audio info.
    /// </summary>
    public partial class TWAudioInfo
    {
        internal TWAudioInfo() { }

        /// <summary>
        /// Name of audio data.
        /// </summary>
        public string Name { get { return _name; } }
    }

    /// <summary>
    /// Used in Callback mechanism for sending messages from the Source to the Application.
    /// Applications version 2.2 or higher must use <see cref="TWCallback2"/>.
    /// </summary>
    partial class TWCallback
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TWCallback"/> class.
        /// </summary>
        /// <param name="callback">The callback function’s entry point.</param>
        public TWCallback(CallbackDelegate callback)
        {
            _callBackProc = callback;
        }

        /// <summary>
        /// An application defined reference constant.
        /// </summary>
        /// <value>
        /// The reference constant.
        /// </value>
        public uint RefCon { get { return _refCon; } set { _refCon = value; } }

        /// <summary>
        /// Initialized to any valid DG_CONTROL / DAT_NULL message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public short Message { get { return _message; } set { _message = value; } }
    }
    /// <summary>
    /// Used in the Callback mechanism for sending messages from the Source to the Application.
    /// </summary>
    partial class TWCallback2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TWCallback2"/> class.
        /// </summary>
        /// <param name="callback">The callback function’s entry point.</param>
        public TWCallback2(CallbackDelegate callback)
        {
            _callBackProc = callback;
        }

        /// <summary>
        /// An application defined reference constant. It has a different size on different
        /// platforms.
        /// </summary>
        /// <value>
        /// The reference constant.
        /// </value>
        public UIntPtr RefCon { get { return _refCon; } set { _refCon = value; } }

        /// <summary>
        /// Initialized to any valid DG_CONTROL / DAT_NULL message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public short Message { get { return _message; } set { _message = value; } }
    }

    /// <summary>
    /// Used by an application either to get information about, or control the setting of a capability.
    /// </summary>
    public sealed partial class TWCapability : IDisposable
    {
        #region ctors

        /// <summary>
        /// Initializes a new instance of the <see cref="TWCapability" /> class.
        /// </summary>
        /// <param name="capability">The capability.</param>
        public TWCapability(CapabilityId capability)
        {
            Capability = capability;
            ContainerType = ContainerType.DontCare;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TWCapability" /> class.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <param name="value">The value.</param>
        public TWCapability(CapabilityId capability, TWOneValue value)
        {
            Capability = capability;
            ContainerType = ContainerType.OneValue;
            SetOneValue(value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TWCapability" /> class.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <param name="value">The value.</param>
        public TWCapability(CapabilityId capability, TWEnumeration value)
        {
            Capability = capability;
            ContainerType = ContainerType.Enum;
            SetEnumValue(value);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TWCapability" /> class.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <param name="value">The value.</param>
        public TWCapability(CapabilityId capability, TWRange value)
        {
            Capability = capability;
            ContainerType = ContainerType.Range;
            SetRangeValue(value);
        }
        #endregion

        #region properties

        /// <summary>
        /// Id of capability to set or get.
        /// </summary>
        public CapabilityId Capability { get { return (CapabilityId)_cap; } set { _cap = (ushort)value; } }
        /// <summary>
        /// The type of the container structure referenced by the pointer internally. The container
        /// will be one of four types: <see cref="TWArray"/>, <see cref="TWEnumeration"/>,
        /// <see cref="TWOneValue"/>, or <see cref="TWRange"/>.
        /// </summary>
        public ContainerType ContainerType { get { return (ContainerType)_conType; } set { _conType = (ushort)value; } }

        internal IntPtr Container { get { return _hContainer; } }

        #endregion

        #region value functions

        void SetOneValue(TWOneValue value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            ContainerType = ContainerType.OneValue;

            // since one value can only house UInt32 we will not allow type size > 4
            if (TypeReader.GetItemTypeSize(value.ItemType) > 4) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.BadValueType, "TWOneValue")); }

            _hContainer = Platform.MemoryManager.Allocate((uint)Marshal.SizeOf(value));
            if (_hContainer != IntPtr.Zero)
            {
                Marshal.StructureToPtr(value, _hContainer, false);
            }
        }

        void SetEnumValue(TWEnumeration value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            ContainerType = ContainerType.Enum;


            Int32 valueSize = TWEnumeration.ItemOffset + value.ItemList.Length * TypeReader.GetItemTypeSize(value.ItemType);

            int offset = 0;
            _hContainer = Platform.MemoryManager.Allocate((uint)valueSize);
            IntPtr baseAddr = Platform.MemoryManager.Lock(_hContainer);

            // can't safely use StructureToPtr here so write it our own
            WriteValue(baseAddr, ref offset, ItemType.UInt16, value.ItemType);
            WriteValue(baseAddr, ref offset, ItemType.UInt32, (uint)value.ItemList.Length);
            WriteValue(baseAddr, ref offset, ItemType.UInt32, value.CurrentIndex);
            WriteValue(baseAddr, ref offset, ItemType.UInt32, value.DefaultIndex);
            foreach (var item in value.ItemList)
            {
                WriteValue(baseAddr, ref offset, value.ItemType, item);
            }
            Platform.MemoryManager.Unlock(baseAddr);
        }


        void SetRangeValue(TWRange value)
        {
            if (value == null) { throw new ArgumentNullException("value"); }
            ContainerType = ContainerType.Range;

            // since range value can only house UInt32 we will not allow type size > 4
            if (TypeReader.GetItemTypeSize(value.ItemType) > 4) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Resources.BadValueType, "TWRange")); }

            _hContainer = Platform.MemoryManager.Allocate((uint)Marshal.SizeOf(value));
            if (_hContainer != IntPtr.Zero)
            {
                Marshal.StructureToPtr(value, _hContainer, false);
            }
        }
        #endregion

        #region writes

        /// <summary>
        /// Entry call for writing values to a pointer.
        /// </summary>
        /// <param name="baseAddr"></param>
        /// <param name="offset"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void WriteValue(IntPtr baseAddr, ref int offset, ItemType type, object value)
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
                    TWFix32 f32 = (TWFix32)value;
                    Marshal.WriteInt16(baseAddr, offset, f32.Whole);
                    if (f32.Fraction > Int16.MaxValue)
                    {
                        Marshal.WriteInt16(baseAddr, offset + 2, (Int16)(f32.Fraction - 32768));
                    }
                    else
                    {
                        Marshal.WriteInt16(baseAddr, offset + 2, (Int16)f32.Fraction);
                    }
                    break;
                case ItemType.Frame:
                    TWFrame frame = (TWFrame)value;
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Left);
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Top);
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Right);
                    WriteValue(baseAddr, ref offset, ItemType.Fix32, frame.Bottom);
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
            offset += TypeReader.GetItemTypeSize(type);
        }
        /// <summary>
        /// Writes string value.
        /// </summary>
        /// <param name="baseAddr"></param>
        /// <param name="offset"></param>
        /// <param name="item"></param>
        /// <param name="maxLength"></param>
        static void WriteString(IntPtr baseAddr, int offset, string item, int maxLength)
        {
            if (string.IsNullOrEmpty(item))
            {
                // write zero
                Marshal.WriteByte(baseAddr, offset, 0);
            }
            else
            {
                for (int i = 0; i < maxLength; i++)
                {
                    if (i == item.Length)
                    {
                        // string end reached, so write \0 and quit
                        Marshal.WriteByte(baseAddr, offset, 0);
                        return;
                    }
                    else
                    {
                        Marshal.WriteByte(baseAddr, offset, (byte)item[i]);
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

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing) { }
            if (_hContainer != IntPtr.Zero)
            {
                Platform.MemoryManager.Free(_hContainer);
                _hContainer = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TWCapability"/> class.
        /// </summary>
        ~TWCapability()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// Embedded in the <see cref="TWCieColor"/> structure;
    /// defines a CIE XYZ space tri-stimulus value.
    /// </summary>
    public partial struct TWCiePoint : IEquatable<TWCiePoint>
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
            if (!(obj is TWCiePoint))
                return false;

            return Equals((TWCiePoint)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TWCiePoint other)
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
        public static bool operator ==(TWCiePoint value1, TWCiePoint value2)
        {
            return value1.Equals(value2);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(TWCiePoint value1, TWCiePoint value2)
        {
            return !value1.Equals(value2);
        }
        #endregion
    }

    /// <summary>
    /// Defines the mapping from an RGB color space device into CIE 1931 (XYZ) color space.
    /// </summary>
    public partial class TWCieColor
    {
        internal TWCieColor() { }

        /// <summary>
        /// Defines the original color space that was transformed into CIE XYZ. 
        /// This value is not set-able by the application. 
        /// </summary>
        public ushort ColorSpace { get { return _colorSpace; } }
        /// <summary>
        /// Used to indicate which data byte is taken first. If zero, then high byte is
        /// first. If non-zero, then low byte is first.
        /// </summary>
        public short LowEndian { get { return _lowEndian; } }
        /// <summary>
        /// If non-zero then color data is device-dependent and only ColorSpace is
        /// valid in this structure.
        /// </summary>
        public short DeviceDependent { get { return _deviceDependent; } }
        /// <summary>
        /// Version of the color space descriptor specification used to define the
        /// transform data. The current version is zero.
        /// </summary>
        public int VersionNumber { get { return _versionNumber; } }
        /// <summary>
        /// Describes parametrics for the first stage transformation of the Postscript
        /// Level 2 CIE color space transform process.
        /// </summary>
        public TWTransformStage StageABC { get { return _stageABC; } }
        /// <summary>
        /// Describes parametrics for the first stage transformation of the Postscript
        /// Level 2 CIE color space transform process.
        /// </summary>
        public TWTransformStage StageLMN { get { return _stageLMN; } }
        /// <summary>
        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of the
        /// diffused white point.
        /// </summary>
        public TWCiePoint WhitePoint { get { return _whitePoint; } }
        /// <summary>
        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of the
        /// diffused black point.
        /// </summary>
        public TWCiePoint BlackPoint { get { return _blackPoint; } }
        /// <summary>
        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of inkless
        /// "paper" from which the image was acquired.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "WhitePaper")]
        public TWCiePoint WhitePaper { get { return _whitePaper; } }
        /// <summary>
        /// Values that specify the CIE 1931 (XYZ space) tri-stimulus value of solid
        /// black ink on the "paper" from which the image was acquired.
        /// </summary>
        public TWCiePoint BlackInk { get { return _blackInk; } }
        /// <summary>
        /// Optional table look-up values used by the decode function. Samples
        /// are ordered sequentially and end-to-end as A, B, C, L, M, and N.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWFix32[] Samples { get { return _samples; } }
    }

    /// <summary>
    /// Allows for a data source and application to pass custom data to each other.
    /// </summary>
    public partial class TWCustomDSData
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
    public partial class TWDeviceEvent
    {
        internal TWDeviceEvent() { }

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
    /// Embedded in the <see cref="TWGrayResponse"/>, <see cref="TWPalette8"/>, and <see cref="TWRgbResponse"/> structures.
    /// This structure holds the tri-stimulus color palette information for <see cref="TWPalette8"/> structures.
    /// The order of the channels shall match their alphabetic representation. That is, for RGB data, R
    /// shall be channel 1. For CMY data, C shall be channel 1. This allows the application and Source
    /// to maintain consistency. Grayscale data will have the same values entered in all three channels.
    /// </summary>
    public partial struct TWElement8 : IEquatable<TWElement8>
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
            if (!(obj is TWElement8))
                return false;

            return Equals((TWElement8)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TWElement8 other)
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
        public static bool operator ==(TWElement8 v1, TWElement8 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Check for value inequality.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static bool operator !=(TWElement8 v1, TWElement8 v2)
        {
            return !(v1 == v2);
        }


        #endregion
    }

    /// <summary>
    /// An enumeration stores a list of individual values, with one of the items designated as the current
    /// value. There is no required order to the values in the list.
    /// </summary>
    public partial class TWEnumeration
    {
        /// <summary>
        /// The type of items in the enumerated list. All items in the array have the same size.
        /// </summary>
        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }
        ///// <summary>
        ///// How many items are in the enumeration.
        ///// </summary>
        //public int Count { get { return (int)_numItems; } set { _numItems = (uint)value; } }
        /// <summary>
        /// The item number, or index (zero-based) into <see cref="ItemList"/>, of the "current"
        /// value for the capability.
        /// </summary>
        public int CurrentIndex { get { return (int)_currentIndex; } set { _currentIndex = (uint)value; } }
        /// <summary>
        /// The item number, or index (zero-based) into <see cref="ItemList"/>, of the "power-on"
        /// value for the capability.
        /// </summary>
        public int DefaultIndex { get { return (int)_defaultIndex; } set { _defaultIndex = (uint)value; } }
        /// <summary>
        /// The enumerated list: one value resides within each array element.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public object[] ItemList
        {
            get { return _itemList; }
            set
            {
                _itemList = value;
                if (value != null) { _numItems = (uint)value.Length; }
                else { _numItems = 0; }
            }
        }

        /// <summary>
        /// Gets the byte offset of the item list from a Ptr to the first item.
        /// </summary>
        internal const int ItemOffset = 14;
    }


    /// <summary>
    /// Used on Windows and Macintosh pre OS X to pass application events/messages from the
    /// application to the Source.
    /// </summary>
    public partial class TWEvent
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

    /// <summary>
    /// This structure is used to pass specific information between the data source and the application
    /// through <see cref="TWExtImageInfo"/>.
    /// </summary>
    public partial struct TWInfo
    {
        /// <summary>
        /// Tag identifying an information.
        /// </summary>
        public ExtendedImageInfo InfoID { get { return (ExtendedImageInfo)_infoID; } }
        /// <summary>
        /// Item data type.
        /// </summary>
        public ItemType ItemType { get { return (ItemType)_itemType; } }
        /// <summary>
        /// Number of items.
        /// </summary>
        public ushort NumItems { get { return _numItems; } }

        /// <summary>
        /// This is the return code of availability of data for extended image attribute requested.
        /// </summary>
        /// <value>
        /// The return code.
        /// </value>
        public ReturnCode ReturnCode { get { return (ReturnCode)_returnCode; } }

        /// <summary>
        /// Contains either data or a handle to data. The field
        /// contains data if the total amount of data is less than or equal to four bytes. The
        /// field contains a handle if the total amount of data is more than four bytes.
        /// The amount of data is determined by multiplying NumItems times
        /// the byte size of the data type specified by ItemType.
        /// If the Item field contains a handle to data, then the Application is
        /// responsible for freeing that memory.
        /// </summary>
        public UIntPtr Item { get { return _item; } internal set { _item = value; } }
    }

    /// <summary>
    /// This structure is used to pass extended image information from the data source to application at
    /// the end of State 7. The application creates this structure at the end of State 7, when it receives
    /// XFERDONE. Application fills NumInfos for Number information it needs, and array of
    /// extended information attributes in Info[ ] array. Application, then, sends it down to the source
    /// using the above operation triplet. The data source then examines each Info, and fills the rest of
    /// data with information allocating memory when necessary.
    /// </summary>
    public sealed partial class TWExtImageInfo : IDisposable
    {
        internal TWExtImageInfo() { }

        /// <summary>
        /// Number of information that application is requesting. This is filled by the
        /// application. If positive, then the application is requesting specific extended
        /// image information. The application should allocate memory and fill in the
        /// attribute tag for image information.
        /// </summary>
        public uint NumInfos { get { return _numInfos; } }
        /// <summary>
        /// Array of information.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWInfo[] Info { get { return _info; } }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing) { }
            // this is iffy & may have to flatten info array as individual fields in this class to work
            if (_info != null)
            {
                for (int i = 0; i < _info.Length; i++)
                {
                    var it = _info[i];
                    if (it.Item != UIntPtr.Zero)
                    {
                        var sz = it.NumItems * TypeReader.GetItemTypeSize(it.ItemType);
                        if (sz > UIntPtr.Size || it.ItemType == ItemType.Handle)
                        {
                            // uintptr to intptr could be bad
                            var ptr = new IntPtr(BitConverter.ToInt64(BitConverter.GetBytes(it.Item.ToUInt64()), 0));
                            Platform.MemoryManager.Free(ptr);
                        }
                    }
                    it.Item = UIntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TWExtImageInfo"/> class.
        /// </summary>
        ~TWExtImageInfo()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// Provides information about the currently selected device.
    /// </summary>
    public partial class TWFileSystem
    {
        /// <summary>
        /// The name of the input or source file.
        /// </summary>
        public string InputName { get { return _inputName; } set { _inputName = value; } }
        /// <summary>
        /// The result of an operation or the name of a destination file.
        /// </summary>
        public string OutputName { get { return _outputName; } set { _outputName = value; } }
        /// <summary>
        /// A pointer to Source specific data used to remember state
        /// information, such as the current directory.
        /// </summary>
        public IntPtr Context { get { return _context; } set { _context = value; } }

        /// <summary>
        /// When set to TRUE recursively apply the operation. (ex: deletes
        /// all subdirectories in the directory being deleted; or copies all
        /// sub-directories in the directory being copied.
        /// </summary>
        public bool Recursive { get { return _subdirectories == TwainConst.True; } set { _subdirectories = value ? TwainConst.True : TwainConst.False; } }

        /// <summary>
        /// Gets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        public FileType FileType { get { return (FileType)_fileType; } set { _fileType = (int)value; } }

        /// <summary>
        /// If <see cref="NTwain.Values.FileType.Directory"/>, total size of media in bytes.
        /// If <see cref="NTwain.Values.FileType.Image"/>, size of image in bytes.
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
        public string CreateTimeDate { get { return _createTimeDate; } set { _createTimeDate = value; } }
        /// <summary>
        /// Last date the file was modified. Same format as <see cref="CreateTimeDate"/>.
        /// </summary>
        public string ModifiedTimeDate { get { return _modifiedTimeDate; } set { _modifiedTimeDate = value; } }
        /// <summary>
        /// The bytes of free space left on the current device.
        /// </summary>
        public uint FreeSpace { get { return _freeSpace; } set { _freeSpace = value; } }
        /// <summary>
        /// An estimate of the amount of space a new image would take
        /// up, based on image layout, resolution and compression.
        /// Dividing this value into the FreeSpace will yield the
        /// approximate number of images that the Device has room for.
        /// </summary>
        public int NewImageSize { get { return _newImageSize; } set { _newImageSize = value; } }
        /// <summary>
        /// If applicable, return the number of <see cref="NTwain.Values.FileType.Image"/> files on the file system including those in all sub-directories.
        /// </summary>
        /// <value>
        /// The number of files.
        /// </value>
        public uint NumberOfFiles { get { return _numberOfFiles; } set { _numberOfFiles = value; } }
        /// <summary>
        /// The number of audio snippets associated with a file of type <see cref="NTwain.Values.FileType.Image"/>.
        /// </summary>
        /// <value>
        /// The number of snippets.
        /// </value>
        public uint NumberOfSnippets { get { return _numberOfSnippets; } set { _numberOfSnippets = value; } }
        /// <summary>
        /// Used to group cameras (ex: front/rear bitonal, front/rear grayscale...).
        /// </summary>
        public uint DeviceGroupMask { get { return _deviceGroupMask; } set { _deviceGroupMask = value; } }
    }

    /// <summary>
    /// This structure is used by the application to specify a set of mapping values to be applied to
    /// grayscale data.
    /// </summary>
    public partial class TWGrayResponse
    {
        /// <summary>
        /// Transfer curve descriptors. All three channels (Channel1, Channel2
        /// and Channel3) must contain the same value for every entry.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWElement8[] Response { get { return _response; } set { _response = value; } }
    }

    /// <summary>
    /// A general way to describe the version of software that is running.
    /// </summary>
    public partial struct TWVersion : IEquatable<TWVersion>
    {
        /// <summary>
        /// This refers to your application or Source’s major revision number. e.g. The
        /// "2" in "version 2.1".
        /// </summary>
        public short Major { get { return (short)_majorNum; } set { _majorNum = (ushort)value; } }
        /// <summary>
        /// The incremental revision number of your application or Source. e.g. The "1"
        /// in "version 2.1".
        /// </summary>
        public short Minor { get { return (short)_minorNum; } set { _minorNum = (ushort)value; } }
        /// <summary>
        /// The primary language for your Source or application.
        /// </summary>
        public Language Language { get { return (Language)_language; } set { _language = (ushort)value; } }
        /// <summary>
        /// The primary country where your Source or application is intended to be
        /// distributed. e.g. Germany.
        /// </summary>
        public Country Country { get { return (Country)_country; } set { _country = (ushort)value; } }
        /// <summary>
        /// General information string - fill in as needed. e.g. "1.0b3 Beta release".
        /// </summary>
        public string Info { get { return _info; } set { _info.VerifyLengthUnder(TwainConst.String32 - 1); _info = value; } }

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
            if (!(obj is TWVersion))
                return false;

            return Equals((TWVersion)obj);
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns></returns>
        public bool Equals(TWVersion other)
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
        public static bool operator ==(TWVersion v1, TWVersion v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Check for value inequality.
        /// </summary>
        /// <param name="v1">The v1.</param>
        /// <param name="v2">The v2.</param>
        /// <returns></returns>
        public static bool operator !=(TWVersion v1, TWVersion v2)
        {
            return !(v1 == v2);
        }

        #endregion
    }

    /// <summary>
    /// Provides identification information about a TWAIN entity. Used to maintain consistent
    /// communication between entities.
    /// </summary>
    public partial class TWIdentity
    {
        /// <summary>
        /// A unique, internal identifier for the TWAIN entity. This field is only
        /// filled by the Source Manager. Neither an application nor a Source
        /// should fill this field. The Source uses the contents of this field to
        /// "identify" which application is invoking the operation sent to the
        /// Source.
        /// </summary>
        public int Id { get { return (int)_id; } }

        /// <summary>
        /// Gets the supported data group. The application will normally set this field to specify which Data
        /// Group(s) it wants the Source Manager to sort Sources by when
        /// presenting the Select Source dialog, or returning a list of available
        /// Sources.
        /// </summary>
        /// <value>The data group.</value>
        public DataGroups DataGroup
        {
            get { return (DataGroups)(_supportedGroups & 0xffff); }
            set { _supportedGroups |= (uint)value; }
        }

        /// <summary>
        /// Major number of latest TWAIN version that this element supports.
        /// </summary>
        public short ProtocolMajor { get { return (short)_protocolMajor; } set { _protocolMajor = (ushort)value; } }

        /// <summary>
        /// Minor number of latest TWAIN version that this element supports.
        /// </summary>
        public short ProtocolMinor { get { return (short)_protocolMinor; } set { _protocolMinor = (ushort)value; } }


        /// <summary>
        /// A <see cref="TWVersion"/> structure identifying the TWAIN entity.
        /// </summary>
        public TWVersion Version { get { return _version; } set { _version = value; } }
        /// <summary>
        /// Gets the data functionalities for TWAIN 2 detection.
        /// </summary>
        /// <value>The data functionalities.</value>
        public DataFunctionalities DataFunctionalities
        {
            get { return (DataFunctionalities)(_supportedGroups & 0xffff0000); }
            set { _supportedGroups |= (uint)value; }
        }

        /// <summary>
        /// String identifying the manufacturer of the application or Source. e.g. "Aldus".
        /// </summary>
        public string Manufacturer { get { return _manufacturer; } set { value.VerifyLengthUnder(TwainConst.String32 - 1); _manufacturer = value; } }
        /// <summary>
        /// Tells an application that performs device-specific operations which
        /// product family the Source supports. This is useful when a new Source
        /// has been released and the application doesn't know about the
        /// particular Source but still wants to perform Custom operations with it.
        /// e.g. "ScanMan".
        /// </summary>
        public string ProductFamily { get { return _productFamily; } set { value.VerifyLengthUnder(TwainConst.String32 - 1); _productFamily = value; } }
        /// <summary>
        /// A string uniquely identifying the Source. This is the string that will be
        /// displayed to the user at Source select-time. This string must uniquely
        /// identify your Source for the user, and should identify the application
        /// unambiguously for Sources that care. e.g. "ScanJet IIc".
        /// </summary>
        public string ProductName { get { return _productName; } set { value.VerifyLengthUnder(TwainConst.String32 - 1); _productName = value; } }

        /// <summary>
        /// Creates a <see cref="TWIdentity"/> from assembly values.
        /// </summary>
        /// <param name="supportedGroups">The supported groups.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">assembly</exception>
        public static TWIdentity CreateFromAssembly(DataGroups supportedGroups, Assembly assembly)
        {
            if (assembly == null) { throw new ArgumentNullException("assembly"); }

            var info = FileVersionInfo.GetVersionInfo(assembly.Location);

            return Create(supportedGroups, assembly.GetName().Version, info.CompanyName, info.ProductName, info.ProductName, info.FileDescription);
        }

        /// <summary>
        /// Creates a <see cref="TWIdentity" /> from specified values.
        /// </summary>
        /// <param name="supportedGroups">The supported groups.</param>
        /// <param name="version">The version.</param>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <param name="productFamily">The product family.</param>
        /// <param name="productName">Name of the product.</param>
        /// <param name="productDescription">The product description.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">version</exception>
        public static TWIdentity Create(DataGroups supportedGroups, Version version,
            string manufacturer, string productFamily, string productName, string productDescription)
        {
            if (version == null) { throw new ArgumentNullException("version"); }

            return new TWIdentity
            {
                Manufacturer = string.IsNullOrEmpty(manufacturer) ? "UNKNOWN" : manufacturer,
                ProtocolMajor = TwainConst.ProtocolMajor,
                ProtocolMinor = TwainConst.ProtocolMinor,
                DataGroup = DataGroups.Control | supportedGroups,
                DataFunctionalities = DataFunctionalities.App2,

                ProductFamily = string.IsNullOrEmpty(productFamily) ? "UNKNOWN" : productFamily,
                ProductName = string.IsNullOrEmpty(productName) ? "UNKNOWN" : productName,
                Version = new TWVersion
                {
                    Major = (short)version.Major,
                    Minor = (short)version.Minor,
                    Country = Country.Usa,
                    Language = Language.EnglishUSA,
                    Info = productDescription ?? string.Empty,
                }
            };
        }
    }

    /// <summary>
    /// Describes the "real" image data, that is, the complete image being transferred between the
    /// Source and application. The Source may transfer the data in a different format--the information
    /// may be transferred in "strips" or "tiles" in either compressed or uncompressed form.
    /// </summary>
    public partial class TWImageInfo
    {
        internal TWImageInfo() { }

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
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
    public partial class TWImageLayout
    {
        /// <summary>
        /// Frame coords within larger document.
        /// </summary>
        public TWFrame Frame { get { return _frame; } set { _frame = value; } }
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
    public partial struct TWMemory
    {
        // not a class due to embedded

        /// <summary>
        /// Encodes which entity releases the buffer and how the buffer is referenced.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags")]
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
    /// Describes the form of the acquired data being passed from the Source to the application.
    /// </summary>
    partial class TWImageMemXfer
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
        /// A structure of type <see cref="TWMemory"/> describing who must dispose of the
        /// buffer, the actual size of the buffer, in bytes, and where the buffer is
        /// located in memory.
        /// </summary>
        public TWMemory Memory { get { return _memory; } internal set { _memory = value; } }
    }

    /// <summary>
    /// Describes the information necessary to transfer a JPEG-compressed image during a buffered
    /// transfer. Images compressed in this fashion will be compatible with the JPEG File Interchange
    /// Format, version 1.1.
    /// </summary>
    public partial class TWJpegCompression
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public ushort[] QuantMap { get { return _quantMap; } set { _quantMap = value; } }
        /// <summary>
        /// Quantization tables.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWMemory[] QuantTable { get { return _quantTable; } set { _quantTable = value; } }
        /// <summary>
        /// Mapping of components to Huffman tables. Null entries signify
        /// selection of the default tables.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public ushort[] HuffmanMap { get { return _huffmanMap; } set { _huffmanMap = value; } }
        /// <summary>
        /// DC Huffman tables. Null entries signify selection of the default tables.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWMemory[] HuffmanDC { get { return _huffmanDC; } set { _huffmanDC = value; } }
        /// <summary>
        /// AC Huffman tables. Null entries signify selection of the default tables.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWMemory[] HuffmanAC { get { return _huffmanAC; } set { _huffmanAC = value; } }
    }

    /// <summary>
    /// Container for one value.
    /// </summary>
    public partial class TWOneValue
    {
        /// <summary>
        /// The type of the item.
        /// </summary>
        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }
        /// <summary>
        /// The value.
        /// </summary>
        public uint Item { get { return _item; } set { _item = value; } }
    }


    /// <summary>
    /// This structure holds the color palette information for buffered memory transfers of type
    /// ICapPixelType = Palette.
    /// </summary>
    public partial class TWPalette8
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWElement8[] Colors { get { return _colors; } set { _colors = value; } }
    }

    /// <summary>
    /// Used to bypass the TWAIN protocol when communicating with a device. All memory must be
    /// allocated and freed by the Application.
    /// </summary>
    public partial class TWPassThru
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
    partial class TWPendingXfers
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TWPendingXfers"/> class.
        /// </summary>
        public TWPendingXfers()
        {
            _count = TwainConst.DontCare16;
        }

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
        public uint EndOfJob { get { return _eOJ; } }
    }


    /// <summary>
    /// Container for a range of values.
    /// </summary>
    public partial class TWRange
    {
        /// <summary>
        /// The type of items in the list.
        /// </summary>
        public ItemType ItemType { get { return (ItemType)_itemType; } set { _itemType = (ushort)value; } }
        /// <summary>
        /// The least positive/most negative value of the range.
        /// </summary>
        public uint MinValue { get { return _minValue; } set { _minValue = value; } }
        /// <summary>
        /// The most positive/least negative value of the range.
        /// </summary>
        public uint MaxValue { get { return _maxValue; } set { _maxValue = value; } }
        /// <summary>
        /// The delta between two adjacent values of the range.
        /// e.g. Item2 - Item1 = StepSize;
        /// </summary>
        public uint StepSize { get { return _stepSize; } set { _stepSize = value; } }
        /// <summary>
        /// The device’s "power-on" value for the capability. If the application is
        /// performing a MSG_SET operation and isn’t sure what the default
        /// value is, set this field to <see cref="TwainConst.DontCare32"/>.
        /// </summary>
        public uint DefaultValue { get { return _defaultValue; } set { _defaultValue = value; } }
        /// <summary>
        /// The value to which the device (or its user interface) is currently set to
        /// for the capability.
        /// </summary>
        public uint CurrentValue { get { return _currentValue; } set { _currentValue = value; } }
    }

    /// <summary>
    /// This structure is used by the application to specify a set of mapping values to be applied to RGB
    /// color data. Use this structure for RGB data whose bit depth is up to, and including, 8-bits.
    /// The number of elements in the array is determined by <see cref="TWImageInfo.BitsPerPixel"/>—the number of
    /// elements is 2 raised to the power of <see cref="TWImageInfo.BitsPerPixel"/>.
    /// </summary>
    public partial class TWRgbResponse
    {
        /// <summary>
        /// Transfer curve descriptors. To minimize color shift problems, writing the
        /// same values into each channel is desirable.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public TWElement8[] Response { get { return _response; } set { _response = value; } }
    }

    /// <summary>
    /// Describes the file format and file specification information for a transfer through a disk file.
    /// </summary>
    public partial class TWSetupFileXfer
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
    public partial class TWSetupMemXfer
    {
        internal TWSetupMemXfer() { }

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
    public partial class TWStatus
    {
        internal TWStatus() { }
        internal TWStatus(ushort code, ushort data)
        {
            _conditionCode = code;
            _data = data;
        }
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

    /// <summary>
    /// Translates the contents of Status into a localized UTF8string, with the total number of bytes
    /// in the string.
    /// </summary>
    public sealed partial class TWStatusUtf8 : IDisposable
    {
        /// <summary>
        /// <see cref="TWStatus"/> data received from a previous call.
        /// </summary>
        public TWStatus Status
        {
            get { return new TWStatus(_conditionCode, _data); }
        }

        /// <summary>
        /// Total number of bytes in the UTF8string, plus the terminating NULL byte. 
        /// This is not the same as the total number of characters in the string.
        /// </summary>
        public int Size { get { return (int)_size; } }

        /// <summary>
        /// TW_HANDLE to a UTF-8 encoded localized string (based on 
        /// TwIdentity.Language or CapLanguage). The Source allocates
        /// it, the Application frees it.
        /// </summary>
        public IntPtr UTF8StringPtr { get { return _uTF8string; } }

        /// <summary>
        /// Gets the actual string from the pointer. This may be incorrect.
        /// </summary>
        /// <returns></returns>
        public string TryGetString()
        {
            if (_uTF8string != IntPtr.Zero)
            {
                var sb = new StringBuilder(Size - 1);
                byte bt;
                while (sb.Length < _size &&
                    (bt = Marshal.ReadByte(_uTF8string, sb.Length)) != 0)
                {
                    sb.Append((char)bt);
                }
                return sb.ToString();
            }
            return null;
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            if (_uTF8string != IntPtr.Zero)
            {
                Platform.MemoryManager.Free(_uTF8string);
                _uTF8string = IntPtr.Zero;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TWStatusUtf8"/> class.
        /// </summary>
        ~TWStatusUtf8()
        {
            Dispose(false);
        }

        #endregion
    }


    /// <summary>
    /// This structure is used to handle the user interface coordination between an application and a
    /// Source.
    /// </summary>
    partial class TWUserInterface
    {
        /// <summary>
        /// Set to TRUE by the application if the Source should activate its built-in user
        /// interface. Otherwise, set to FALSE. Note that not all sources support ShowUI =
        /// FALSE.
        /// </summary>
        public bool ShowUI { get { return _showUI > 0; } set { _showUI = value ? TwainConst.True : TwainConst.False; } }
        /// <summary>
        /// If ShowUI is TRUE, then an application setting this to TRUE requests the Source to
        /// run Modal.
        /// </summary>
        public bool ModalUI { get { return _modalUI > 0; } set { _modalUI = value ? TwainConst.True : TwainConst.False; } }
        /// <summary>
        /// Microsoft Windows only: Application’s window handle. The Source designates
        /// the hWnd as its parent when creating the Source dialog.
        /// </summary>
        public IntPtr hParent { get { return _hParent; } set { _hParent = value; } }
    }



    /// <summary>
    /// Provides entry points required by TWAIN 2.0 Applications and Sources.
    /// </summary>
    partial class TWEntryPoint : IMemoryManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TWEntryPoint"/> class.
        /// </summary>
        public TWEntryPoint()
        {
            _size = (uint)Marshal.SizeOf(this);
        }

        #region IMemoryManager Members

        public IntPtr Allocate(uint size)
        {
            return _dSM_MemAllocate(size);
        }

        public void Free(IntPtr handle)
        {
            _dSM_MemFree(handle);
        }

        public IntPtr Lock(IntPtr handle)
        {
            return _dSM_MemLock(handle);
        }

        public void Unlock(IntPtr handle)
        {
            _dSM_MemUnlock(handle);
        }

        #endregion
    }


    /// <summary>
    /// The range of colors specified by this structure is replaced with Replacement grayscale value in the
    /// binary image. The color is specified in HSV color space.
    /// </summary>
    public partial class TWFilterDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TWFilterDescriptor"/> struct.
        /// </summary>
        public TWFilterDescriptor()
        {
            _size = (uint)Marshal.SizeOf(this);
        }

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
    public partial class TWFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TWFilter"/> class.
        /// </summary>
        public TWFilter()
        {
            _size = (uint)Marshal.SizeOf(this);
        }

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
        /// Handle to array of <see cref="TWFilterDescriptor"/>.
        /// </summary>
        /// <value>
        /// The descriptors.
        /// </value>
        public IntPtr hDescriptors { get { return _hDescriptors; } set { _hDescriptors = value; } }
    }


}
