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
                return ValueReader.ReadOneValue<TWQC>(_twain, twCap);
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
                        return new[] { ValueReader.ReadOneValue<TValue>(_twain, twCap) };
                    case TWON.ENUMERATION:
                        return ValueReader.ReadEnumeration<TValue>(_twain, twCap).Items;
                    case TWON.ARRAY:
                        return ValueReader.ReadArray<TValue>(_twain, twCap);
                    case TWON.RANGE:
                        return ValueReader.ReadRange<TValue>(_twain, twCap).ToList();
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
                        return ValueReader.ReadOneValue<TValue>(_twain, twCap);
                    case TWON.ENUMERATION:
                        var enumeration = ValueReader.ReadEnumeration<TValue>(_twain, twCap);
                        if (enumeration.CurrentIndex < enumeration.Items.Length)
                            return enumeration.Items[enumeration.CurrentIndex];
                        break;
                    case TWON.RANGE:
                        return ValueReader.ReadRange<TValue>(_twain, twCap).CurrentValue;
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
                        return ValueReader.ReadOneValue<TValue>(_twain, twCap);
                    case TWON.ENUMERATION:
                        var enumeration = ValueReader.ReadEnumeration<TValue>(_twain, twCap);
                        if (enumeration.DefaultIndex < enumeration.Items.Length)
                            return enumeration.Items[enumeration.DefaultIndex];
                        break;
                    case TWON.RANGE:
                        return ValueReader.ReadRange<TValue>(_twain, twCap).DefaultValue;
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
                return ValueReader.ReadOneString(_twain, twCap);
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
                return ValueReader.ReadOneString(_twain, twCap);
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
    }

}