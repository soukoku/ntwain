using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    //TODO: handle multi-value sets


    /// <summary>
    /// Wrapped class for reading/writing a TWAIN capability associated with a <see cref="DataSource"/>.
    /// </summary>
    /// <typeparam name="TValue">The TWAIN type of the value.</typeparam>
    public class CapWrapper<TValue> : NTwain.ICapWrapper<TValue>
    {
        ICapControl _source;
        Func<object, TValue> _getConvertRoutine;
        Func<TValue, ReturnCode> _setCustomRoutine;
        Func<TValue, TWOneValue> _setOneValueFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapWrapper{TValue}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="capability">The capability.</param>
        /// <param name="getConversionRoutine">The value conversion routine in Get methods.</param>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// getConversionRoutine
        /// </exception>
        public CapWrapper(ICapControl source, CapabilityId capability,
            Func<object, TValue> getConversionRoutine)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (getConversionRoutine == null) { throw new ArgumentNullException("getConversionRoutine"); }

            _source = source;
            _getConvertRoutine = getConversionRoutine;
            Capability = capability;
            SupportedActions = source.CapQuerySupport(capability);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CapWrapper{TValue}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="capability">The capability.</param>
        /// <param name="getConversionRoutine">The value conversion routine in Get methods.</param>
        /// <param name="setValueProvider">Callback to provide the capability object for set method.</param>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// getConversionRoutine
        /// or
        /// setValueProvider
        /// </exception>
        public CapWrapper(ICapControl source, CapabilityId capability,
            Func<object, TValue> getConversionRoutine,
            Func<TValue, TWOneValue> setValueProvider)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (getConversionRoutine == null) { throw new ArgumentNullException("getConversionRoutine"); }
            if (setValueProvider == null) { throw new ArgumentNullException("setValueProvider"); }

            _source = source;
            _getConvertRoutine = getConversionRoutine;
            _setOneValueFunc = setValueProvider;
            Capability = capability;
            SupportedActions = source.CapQuerySupport(capability);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CapWrapper{TValue}" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="capability">The capability.</param>
        /// <param name="getConversionRoutine">The value conversion routine in Get methods.</param>
        /// <param name="setValueRoutine">Callback to perform set value.</param>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// getConversionRoutine
        /// or
        /// setValueRoutine
        /// </exception>
        public CapWrapper(ICapControl source, CapabilityId capability,
            Func<object, TValue> getConversionRoutine,
            Func<TValue, ReturnCode> setValueRoutine)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (getConversionRoutine == null) { throw new ArgumentNullException("getConversionRoutine"); }
            if (setValueRoutine == null) { throw new ArgumentNullException("setValueRoutine"); }

            _source = source;
            _getConvertRoutine = getConversionRoutine;
            _setCustomRoutine = setValueRoutine;
            Capability = capability;
            SupportedActions = source.CapQuerySupport(capability);
        }


        bool Supports(QuerySupports flag)
        {
            return (SupportedActions & flag) == flag;
        }

        #region properties

        /// <summary>
        /// Gets the capability.
        /// </summary>
        /// <value>
        /// The capability.
        /// </value>
        public CapabilityId Capability { get; private set; }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>
        /// The supported actions.
        /// </value>
        public QuerySupports SupportedActions { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this capability is supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this capability is supported; otherwise, <c>false</c>.
        /// </value>
        public bool IsSupported { get { return SupportedActions > QuerySupports.None; } }

        /// <summary>
        /// Gets a value indicating whether <see cref="Get"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get values; otherwise, <c>false</c>.
        /// </value>
        public bool CanGet { get { return Supports(QuerySupports.Get); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetDefault"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get default value; otherwise, <c>false</c>.
        /// </value>
        public bool CanGetDefault { get { return Supports(QuerySupports.GetDefault); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetCurrent"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get current value; otherwise, <c>false</c>.
        /// </value>
        public bool CanGetCurrent { get { return Supports(QuerySupports.GetCurrent); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetLabel"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get label; otherwise, <c>false</c>.
        /// </value>
        public bool CanGetLabel { get { return Supports(QuerySupports.GetLabel); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetHelp"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get help; otherwise, <c>false</c>.
        /// </value>
        public bool CanGetHelp { get { return Supports(QuerySupports.GetHelp); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="GetLabelEnum"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can get label enum; otherwise, <c>false</c>.
        /// </value>
        public bool CanGetLabelEnum { get { return Supports(QuerySupports.GetLabelEnum); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="Reset"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can reset; otherwise, <c>false</c>.
        /// </value>
        public bool CanReset { get { return Supports(QuerySupports.Reset); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="Set"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can set; otherwise, <c>false</c>.
        /// </value>
        public bool CanSet { get { return Supports(QuerySupports.Set); } }

        /// <summary>
        /// Gets a value indicating whether <see cref="SetConstraint"/> is supported.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this capability can set constraint; otherwise, <c>false</c>.
        /// </value>
        public bool CanSetConstraint { get { return Supports(QuerySupports.SetConstraint); } }

        #endregion

        #region get methods

        /// <summary>
        /// Gets the default value of this capability.
        /// </summary>
        /// <returns></returns>
        public TValue GetDefault()
        {
            if (CanGetDefault)
            {
                return _getConvertRoutine(_source.CapGetDefault(Capability));
            }
            return default(TValue);
        }

        /// <summary>
        /// Gets the current value of this capability.
        /// </summary>
        /// <returns></returns>
        public TValue GetCurrent()
        {
            if (CanGetCurrent)
            {
                return _getConvertRoutine(_source.CapGetCurrent(Capability));
            }
            return default(TValue);
        }

        /// <summary>
        /// Gets all the possible values of this capability.
        /// </summary>
        /// <returns></returns>
        public IList<TValue> Get()
        {
            if (CanGet)
            {
                return _source.CapGet(Capability).Select(o => _getConvertRoutine(o)).ToList();
            }
            return new List<TValue>();
        }

        /// <summary>
        /// [Experimental] Gets the label value of this capability.
        /// </summary>
        /// <returns></returns>
        public string GetLabel()
        {
            object value = null;
            if (CanGetLabel)
            {
                using (TWCapability cap = new TWCapability(Capability))
                {
                    var rc = _source.DGControl.Capability.GetLabel(cap);
                    if (rc == ReturnCode.Success)
                    {
                        var read = CapabilityReader.ReadValue(cap);

                        switch (read.ContainerType)
                        {
                            case ContainerType.OneValue:
                                // most likely not correct
                                value = read.OneValue;
                                break;
                        }
                    }
                }
            }
            return value == null ? null : value.ToString();
        }

        /// <summary>
        /// [Experimental] Gets the help value of this capability.
        /// </summary>
        /// <returns></returns>
        public string GetHelp()
        {
            object value = null;
            if (CanGetHelp)
            {
                using (TWCapability cap = new TWCapability(Capability))
                {
                    var rc = _source.DGControl.Capability.GetHelp(cap);
                    if (rc == ReturnCode.Success)
                    {
                        var read = CapabilityReader.ReadValue(cap);

                        switch (read.ContainerType)
                        {
                            case ContainerType.OneValue:
                                // most likely not correct
                                value = read.OneValue;
                                break;
                        }
                    }
                }
            }
            return value == null ? null : value.ToString();
        }

        /// <summary>
        /// [Experimental] Gets the display names for possible values of this capability.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetLabelEnum()
        {
            var list = new List<object>();
            if (CanGetLabelEnum)
            {
                using (TWCapability cap = new TWCapability(Capability))
                {
                    var rc = _source.DGControl.Capability.GetLabelEnum(cap);
                    if (rc == ReturnCode.Success)
                    {
                        cap.ReadMultiCapValues(list);
                    }
                }
            }
            return list.Select(o => o.ToString()).ToList();
        }

        #endregion

        #region set methods

        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Reset()
        {
            return _source.CapReset(Capability);
        }

        /// <summary>
        /// Simplified version that sets the current value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Simple Set() is not defined for this capability.</exception>
        public ReturnCode Set(TValue value)
        {
            ReturnCode rc = ReturnCode.Failure;
            if (CanSet)
            {
                if (_setCustomRoutine != null)
                {
                    rc = _setCustomRoutine(value);
                }
                else if (_setOneValueFunc != null)
                {
                    using (var cap = new TWCapability(Capability, _setOneValueFunc(value)))
                    {
                        rc = _source.DGControl.Capability.Set(cap);
                    }
                }
                else
                {
                    throw new InvalidOperationException("Simple Set() is not defined for this capability.");
                }
            }
            return rc;
        }

        /// <summary>
        /// A version of Set that uses an array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ReturnCode Set(TWArray value)
        {
            ReturnCode rc = ReturnCode.Failure;
            if (CanSet)
            {
                using (var cap = new TWCapability(Capability, value))
                {
                    rc = _source.DGControl.Capability.Set(cap);
                }
            }
            return rc;
        }

        /// <summary>
        /// A version of Set that uses an enumeration.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ReturnCode Set(TWEnumeration value)
        {
            ReturnCode rc = ReturnCode.Failure;
            if (CanSet)
            {
                using (var cap = new TWCapability(Capability, value))
                {
                    rc = _source.DGControl.Capability.Set(cap);
                }
            }
            return rc;
        }

        /// <summary>
        /// Sets the constraint value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ReturnCode SetConstraint(TWOneValue value)
        {
            ReturnCode rc = ReturnCode.Failure;
            if (CanSetConstraint)
            {
                using (var cap = new TWCapability(Capability, value))
                {
                    rc = _source.DGControl.Capability.SetConstraint(cap);
                }
            }
            return rc;
        }

        /// <summary>
        /// Sets the constraint value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ReturnCode SetConstraint(TWEnumeration value)
        {
            ReturnCode rc = ReturnCode.Failure;
            if (CanSetConstraint)
            {
                using (var cap = new TWCapability(Capability, value))
                {
                    rc = _source.DGControl.Capability.SetConstraint(cap);
                }
            }
            return rc;
        }

        /// <summary>
        /// Sets the constraint value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ReturnCode SetConstraint(TWRange value)
        {
            ReturnCode rc = ReturnCode.Failure;
            if (CanSetConstraint)
            {
                using (var cap = new TWCapability(Capability, value))
                {
                    rc = _source.DGControl.Capability.SetConstraint(cap);
                }
            }
            return rc;
        }

        #endregion
    }
}