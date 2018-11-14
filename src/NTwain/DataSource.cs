using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTwain.Data;

namespace NTwain
{
    /// <summary>
    /// A TWAIN data source.
    /// </summary>
    public class DataSource
    {
        internal readonly TwainSession Session;

        internal TW_IDENTITY Identity { get; }

        internal DataSource(TwainSession session, TW_IDENTITY src)
        {
            this.Session = session;
            this.Identity = src;
        }

        /// <summary>
        /// Gets the source name.
        /// </summary>
        public string Name => Identity.ProductName;

        /// <summary>
        /// Gets the source version info.
        /// </summary>
        public TW_VERSION Version => Identity.Version;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Name} {Version}";
        }
    }
}
