using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Wrapped class for reading/writing a TWAIN capability associated with a <see cref="DataSource"/>.
    /// </summary>
    /// <typeparam name="TValue">The TWAIN type of the value.</typeparam>
    public class CapabilityControl<TValue>
    {
        DataSource _source;
        Func<object, TValue> _converter;
        Func<TValue, TWCapability> _setProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CapabilityControl"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="capability">The capability.</param>
        /// <param name="valueConversionRoutine">The value conversion routine.</param>
        /// <param name="setCapProvider">Callback to provide the capability object for set method.</param>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public CapabilityControl(DataSource source, CapabilityId capability, Func<object, TValue> valueConversionRoutine, Func<TValue, TWCapability> setCapProvider)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (valueConversionRoutine == null) { throw new ArgumentNullException("valueConversionRoutine"); }

            _source = source;
            _converter = valueConversionRoutine;
            _setProvider = setCapProvider;
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

        public bool IsSupported { get { return SupportedActions > QuerySupports.None; } }
        public bool CanGet { get { return Supports(QuerySupports.Get); } }
        public bool CanGetDefault { get { return Supports(QuerySupports.GetDefault); } }
        public bool CanGetCurrent { get { return Supports(QuerySupports.GetCurrent); } }
        public bool CanGetLabel { get { return Supports(QuerySupports.GetLabel); } }
        public bool CanGetHelp { get { return Supports(QuerySupports.GetHelp); } }
        public bool CanGetLabelEnum { get { return Supports(QuerySupports.GetLabelEnum); } }
        public bool CanReset { get { return Supports(QuerySupports.Reset); } }
        public bool CanSet { get { return Supports(QuerySupports.Set); } }
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
                return _converter(_source.CapGetDefault(Capability));
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
                return _converter(_source.CapGetCurrent(Capability));
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
                return _source.CapGet(Capability).Select(o => _converter(o)).ToList();
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
        /// Resets all values and constraint to power-on defaults.
        /// </summary>
        /// <returns></returns>
        public ReturnCode ResetAll()
        {
            return _source.ResetAll(Capability);
        }

        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Reset()
        {
            return _source.Reset(Capability);
        }

        /// <summary>
        /// Sets the current value of this capability.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ReturnCode Set(TValue value)
        {
            ReturnCode rc = ReturnCode.Failure;
            if (CanSet && _setProvider != null)
            {
                using (var cap = _setProvider(value))
                {
                    rc = _source.DGControl.Capability.Set(cap);
                }
            }
            return rc;
        }


        #endregion

        public enum SetStrategy
        {
            Once,
            Cascade,
            Custom
        }
    }
}
