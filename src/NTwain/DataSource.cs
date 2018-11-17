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
    public partial class DataSource
    {
        internal readonly TwainSession Session;

        internal TW_IDENTITY Identity32 { get; }

        internal DataSource(TwainSession session, TW_IDENTITY src)
        {
            this.Session = session;
            this.Identity32 = src;
            ProtocolVersion = new Version(src.ProtocolMajor, src.ProtocolMinor);
        }

        /// <summary>
        /// Opens the source for capability negotiation.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Open() => Session.DGControl.Identity.OpenDS(Identity32);

        /// <summary>
        /// Closes the source.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Close() => Session.DGControl.Identity.CloseDS(Identity32);


        /// <summary>
        /// Gets the source status. Useful after getting a non-success return code.
        /// </summary>
        /// <returns></returns>
        public TW_STATUS GetStatus()
        {
            TW_STATUS stat = default;
            var rc = Session.DGControl.Status.Get(ref stat, this);
            return stat;
        }


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
