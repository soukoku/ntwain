using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// General interface for a TWAIN session.
    /// </summary>
    public interface ITwainSession : ITwainState, ITwainOperation
    {
        /// <summary>
        /// Gets the supported caps for the currently open source.
        /// </summary>
        /// <value>
        /// The supported caps.
        /// </value>
        IList<CapabilityId> SupportedCaps { get; }
    }
}
