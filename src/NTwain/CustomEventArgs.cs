using NTwain.Data;
using System;

namespace NTwain
{
    /// <summary>
    /// Contains data for a TWAIN source event.
    /// </summary>
    public class DeviceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the detailed device event.
        /// </summary>
        public TW_DEVICEEVENT Data { get; internal set; }
    }

    /// <summary>
    /// Contains data when data source came down from being enabled.
    /// </summary>
    public class SourceDisabledEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the affected source.
        /// </summary>
        public DataSource Source { get; internal set; }

        /// <summary>
        /// Whether the source was enabled with UI-only (no transfer).
        /// The app could do something different if this is <code>true</code>, such as
        /// getting the <see cref="TW_CUSTOMDSDATA"/>.
        /// </summary>
        public bool UIOnly { get; internal set; }
    }
}
