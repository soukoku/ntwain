using NTwain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// General interface for a TWAIN session.
    /// </summary>
    public interface ITwainSession : INotifyPropertyChanged, ITwainOperation
    {
        /// <summary>
        /// Gets the currently open source.
        /// </summary>
        /// <value>
        /// The current source.
        /// </value>
        TwainSource Source { get; }

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>The state.</value>
        int State { get; }
    }
}
