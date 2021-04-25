using System;
using System.Collections.Generic;
using System.Linq;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Contains operations for a <see cref="CAP"/>.
    /// </summary>
    public class CapWrapper
    {
        private readonly TWAIN _twain;

        public CapWrapper(TWAIN twain, CAP cap)
        {
            _twain = twain;
            Cap = cap;

            var twCap = new TW_CAPABILITY
            {
                Cap = cap
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.QUERYSUPPORT, ref twCap);
            if (sts == STS.SUCCESS)
            {
                if (Enum.TryParse(_twain.CapabilityOneValueToString(twCap), out TWQC qc))
                {
                    Supports = qc;
                }
            }
        }

        /// <summary>
        /// The cap in question.
        /// </summary>
        public CAP Cap { get; }

        /// <summary>
        /// The operations supported by the cap.
        /// Not all sources supports this so it may be unknown.
        /// </summary>
        public TWQC Supports { get; }

        /// <summary>
        /// Try to get string representation of the cap's supported values.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetValues()
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
                        return new[] { _twain.CapabilityOneValueToString(twCap) };
                    case TWON.ENUMERATION:
                        var csv = _twain.CapabilityToCsv(twCap, true);
                        return csv.Split(',').Skip(6).ToList();
                    default:
                        csv = _twain.CapabilityToCsv(twCap, true);
                        return csv.Split(',').Skip(4).ToList();
                }
            }
            return new string[0];
        }

        /// <summary>
        /// Try to get string representation of the cap's current value.
        /// </summary>
        /// <returns></returns>
        public string GetCurrent()
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
                        return _twain.CapabilityOneValueToString(twCap);
                    case TWON.ENUMERATION:
                        var csv = _twain.CapabilityToCsv(twCap, true);
                        return csv.Split(new[] { ',' }, 7)[6];
                    default:
                        csv = _twain.CapabilityToCsv(twCap, true);
                        return csv.Split(new[] { ',' }, 5)[4];
                }
            }
            return null;
        }

        /// <summary>
        /// Try to get string representation of the cap's default value.
        /// </summary>
        /// <returns></returns>
        public string GetDefault()
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
                        return _twain.CapabilityOneValueToString(twCap);
                    case TWON.ENUMERATION:
                        var csv = _twain.CapabilityToCsv(twCap, true);
                        return csv.Split(new[] { ',' }, 7)[6];
                    default:
                        csv = _twain.CapabilityToCsv(twCap, true);
                        return csv.Split(new[] { ',' }, 5)[4];
                }
            }
            return null;
        }
    }
}