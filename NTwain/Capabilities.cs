using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Contains known capabilities for TWAIN 2.4.
    /// </summary>
    public class Capabilities
    {
        private readonly TWAIN _twain;

        public Capabilities(TWAIN twain)
        {
            _twain = twain;
        }


        /// <summary>
        /// Resets all cap values and constraint to power-on defaults.
        /// Not all sources will support this.
        /// </summary>
        /// <returns></returns>
        public STS ResetAll()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = CAP.CAP_SUPPORTEDCAPS
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.RESETALL, ref twCap);
            return sts;
        }
    }
}
