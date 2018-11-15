using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NTwain.Data;

namespace NTwain
{
    partial class DataSource : INotifyPropertyChanged
    {
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
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        internal protected void RaisePropertyChanged(string propertyName)
        {
            var handle = PropertyChanged;
            handle?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
