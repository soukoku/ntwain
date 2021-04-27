using System;
using System.Collections.Generic;
using System.Linq;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Contains operations for a <see cref="CAP"/>.
    /// </summary>
    /// <typeparam name="TValue">Individual value type of the cap. Must be one of TWAIN's supported cap value types.
    /// You are responsible for using the correct type for a cap.</typeparam>
    public class CapWrapper<TValue> where TValue : struct
    {
        protected readonly TWAIN _twain;

        public CapWrapper(TWAIN twain, CAP cap)
        {
            _twain = twain;
            Cap = cap;
        }

        /// <summary>
        /// The cap being targeted.
        /// </summary>
        public CAP Cap { get; }

        /// <summary>
        /// Gets the operations supported by the cap.
        /// Not all sources supports this so it may be unknown.
        /// </summary>
        public TWQC QuerySupport()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = Cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.QUERYSUPPORT, ref twCap);
            if (sts == STS.SUCCESS && twCap.ConType == TWON.ONEVALUE)
            {
                return ValueReader.ReadOneValueContainer<TWQC>(_twain, ref twCap);
            }
            return TWQC.Uknown;
        }

        /// <summary>
        /// Try to get list of the cap's supported values.
        /// </summary>
        /// <returns></returns>
        public IList<TValue> GetValues()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = Cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.GET, ref twCap);
            if (sts == STS.SUCCESS)
            {
                switch (twCap.ConType)
                {
                    case TWON.ONEVALUE:
                        return new[] { ValueReader.ReadOneValueContainer<TValue>(_twain, ref twCap) };
                    case TWON.ENUMERATION:
                        return ValueReader.ReadEnumerationContainer<TValue>(_twain, ref twCap).Items;
                    case TWON.ARRAY:
                        return ValueReader.ReadArrayContainer<TValue>(_twain, ref twCap);
                    case TWON.RANGE:
                        return ValueReader.ReadRangeContainer<TValue>(_twain, ref twCap).ToList();
                }
            }
            return EmptyArray<TValue>.Value;
        }

        /// <summary>
        /// Try to get the cap's current value.
        /// </summary>
        /// <returns></returns>
        public TValue GetCurrent()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = Cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.GETCURRENT, ref twCap);
            if (sts == STS.SUCCESS)
            {
                switch (twCap.ConType)
                {
                    case TWON.ONEVALUE:
                        return ValueReader.ReadOneValueContainer<TValue>(_twain, ref twCap);
                    case TWON.ENUMERATION:
                        var enumeration = ValueReader.ReadEnumerationContainer<TValue>(_twain, ref twCap);
                        if (enumeration.CurrentIndex < enumeration.Items.Length)
                            return enumeration.Items[enumeration.CurrentIndex];
                        break;
                    case TWON.RANGE:
                        return ValueReader.ReadRangeContainer<TValue>(_twain, ref twCap).CurrentValue;
                }
            }
            return default;
        }

        /// <summary>
        /// Try to get the cap's default value.
        /// </summary>
        /// <returns></returns>
        public TValue GetDefault()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = Cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.GETDEFAULT, ref twCap);
            if (sts == STS.SUCCESS)
            {
                switch (twCap.ConType)
                {
                    case TWON.ONEVALUE:
                        return ValueReader.ReadOneValueContainer<TValue>(_twain, ref twCap);
                    case TWON.ENUMERATION:
                        var enumeration = ValueReader.ReadEnumerationContainer<TValue>(_twain, ref twCap);
                        if (enumeration.DefaultIndex < enumeration.Items.Length)
                            return enumeration.Items[enumeration.DefaultIndex];
                        break;
                    case TWON.RANGE:
                        return ValueReader.ReadRangeContainer<TValue>(_twain, ref twCap).DefaultValue;
                }
            }
            return default;
        }

        /// <summary>
        /// Try to get the cap's label text.
        /// </summary>
        /// <returns></returns>
        public string GetLabel()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = Cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.GETLABEL, ref twCap);
            if (sts == STS.SUCCESS)
            {
                return ValueReader.ReadOneValueContainerString(_twain, twCap);
            }
            return null;
        }

        /// <summary>
        /// Try to get the cap's description text.
        /// </summary>
        /// <returns></returns>
        public string GetHelp()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = Cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.GETHELP, ref twCap);
            if (sts == STS.SUCCESS)
            {
                return ValueReader.ReadOneValueContainerString(_twain, twCap);
            }
            return null;
        }

        /// <summary>
        /// Resets the cap's current value to power-on default.
        /// </summary>
        /// <returns></returns>
        public STS Reset()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = Cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.RESET, ref twCap);
            return sts;
        }

        /// <summary>
        /// Try to set a one value for the cap.
        /// </summary>
        /// <param name="setMsg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public STS SetOrConstraint(MSG setMsg, TValue value)
        {
            if (setMsg != MSG.SET && setMsg != MSG.SETCONSTRAINT)
                throw new ArgumentException($"Only {nameof(MSG.SET)} and {nameof(MSG.SETCONSTRAINT)} messages are supported.", nameof(setMsg));

            var twCap = new TW_CAPABILITY
            {
                Cap = Cap,
                ConType = TWON.ONEVALUE
            };
            try
            {
                ValueWriter.WriteOneValueContainer(_twain, ref twCap, value);
                return _twain.DatCapability(DG.CONTROL, setMsg, ref twCap);
            }
            finally
            {
                if (twCap.hContainer != IntPtr.Zero)
                    _twain.DsmMemFree(ref twCap.hContainer);
            }
        }

        /// <summary>
        /// Try to set an array value for the cap.
        /// </summary>
        /// <param name="setMsg"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public STS SetOrConstraint(MSG setMsg, TValue[] values)
        {
            if (setMsg != MSG.SET && setMsg != MSG.SETCONSTRAINT)
                throw new ArgumentException($"Only {nameof(MSG.SET)} and {nameof(MSG.SETCONSTRAINT)} messages are supported.", nameof(setMsg));
            if (values == null) throw new ArgumentNullException(nameof(values));

            var twCap = new TW_CAPABILITY
            {
                Cap = Cap,
                ConType = TWON.ARRAY
            };
            try
            {
                ValueWriter.WriteArrayContainer(_twain, ref twCap, values);
                return _twain.DatCapability(DG.CONTROL, setMsg, ref twCap);
            }
            finally
            {
                if (twCap.hContainer != IntPtr.Zero)
                    _twain.DsmMemFree(ref twCap.hContainer);
            }
        }

        /// <summary>
        /// Try to set a range value for the cap.
        /// </summary>
        /// <param name="setMsg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public STS SetOrConstraint(MSG setMsg, Range<TValue> value)
        {
            if (setMsg != MSG.SET && setMsg != MSG.SETCONSTRAINT)
                throw new ArgumentException($"Only {nameof(MSG.SET)} and {nameof(MSG.SETCONSTRAINT)} messages are supported.", nameof(setMsg));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var twCap = new TW_CAPABILITY
            {
                Cap = Cap,
                ConType = TWON.RANGE
            };
            try
            {
                ValueWriter.WriteRangeContainer(_twain, ref twCap, value);
                return _twain.DatCapability(DG.CONTROL, setMsg, ref twCap);
            }
            finally
            {
                if (twCap.hContainer != IntPtr.Zero)
                    _twain.DsmMemFree(ref twCap.hContainer);
            }
        }

        /// <summary>
        /// Try to set an array value for the cap.
        /// </summary>
        /// <param name="setMsg"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public STS SetOrConstraint(MSG setMsg, Enumeration<TValue> value)
        {
            if (setMsg != MSG.SET && setMsg != MSG.SETCONSTRAINT)
                throw new ArgumentException($"Only {nameof(MSG.SET)} and {nameof(MSG.SETCONSTRAINT)} messages are supported.", nameof(setMsg));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var twCap = new TW_CAPABILITY
            {
                Cap = Cap,
                ConType = TWON.ENUMERATION
            };
            try
            {
                ValueWriter.WriteEnumContainer(_twain, ref twCap, value);
                return _twain.DatCapability(DG.CONTROL, setMsg, ref twCap);
            }
            finally
            {
                if (twCap.hContainer != IntPtr.Zero)
                    _twain.DsmMemFree(ref twCap.hContainer);
            }
        }
    }
}