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
            ProtocolVersion = new Version(src.ProtocolMajor, src.ProtocolMinor);
        }

        /// <summary>
        /// Gets the source name.
        /// </summary>
        public string Name => Identity.ProductName;

        /// <summary>
        /// Gets the source's manufacturer name.
        /// </summary>
        public string Manufacturer => Identity.Manufacturer;

        /// <summary>
        /// Gets the source's product family.
        /// </summary>
        public string ProductFamily => Identity.ProductFamily;

        /// <summary>
        /// Gets the source's version info.
        /// </summary>
        public TW_VERSION Version => Identity.Version;

        /// <summary>
        /// Gets the supported data group.
        /// </summary>
        public DataGroups DataGroup => Identity.DataGroup;

        /// <summary>
        /// Gets the supported TWAIN protocol version.
        /// </summary>
        public Version ProtocolVersion { get; }

        /// <summary>
        /// Gets a value indicating whether this data source is open.
        /// </summary>
        public bool IsOpen => Session.State > TwainState.DsmOpened && Session.CurrentSource == this;



        /// <summary>
        /// Opens the source for capability negotiation.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Open() => Session.DGControl.Identity.OpenDS(Identity);

        /// <summary>
        /// Closes the source.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Close() => Session.DGControl.Identity.CloseDS(Identity);



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
