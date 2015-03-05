using NTwain.Data;
using System;
using System.Collections.Generic;
namespace NTwain
{
    /// <summary>
    /// Represents a TWAIN data source.
    /// </summary>
    public interface IDataSource : ITripletControl
    {
        /// <summary>
        /// Gets the source's product name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }

        /// <summary>
        /// Gets the supported data group.
        /// </summary>
        /// <value>
        /// The data group.
        /// </value>
        DataGroups DataGroup { get; }

        /// <summary>
        /// Gets the source's manufacturer name.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        string Manufacturer { get; }

        /// <summary>
        /// Gets the source's product family.
        /// </summary>
        /// <value>
        /// The product family.
        /// </value>
        string ProductFamily { get; }

        /// <summary>
        /// Gets the supported TWAIN protocol version.
        /// </summary>
        /// <value>
        /// The protocol version.
        /// </value>
        Version ProtocolVersion { get; }

        /// <summary>
        /// Gets the source's version information.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        TWVersion Version { get; }

        /// <summary>
        /// Gets a value indicating whether this data source has been opened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this data source is open; otherwise, <c>false</c>.
        /// </value>
        bool IsOpen { get; }

        /// <summary>
        /// Gets the capabilities for this data source.
        /// </summary>
        /// <value>
        /// The capabilities.
        /// </value>
        Capabilities Capabilities { get; }

        /// <summary>
        /// Gets or sets the current settings (CustomDSData) of this source if supported.
        /// </summary>
        /// <value>
        /// The source settings.
        /// </value>
        byte[] Settings { get; set; }

        /// <summary>
        /// Opens the source for capability negotiation.
        /// </summary>
        /// <returns></returns>
        ReturnCode Open();

        /// <summary>
        /// Closes the source.
        /// </summary>
        /// <returns></returns>
        ReturnCode Close();

        /// <summary>
        /// Enables the source to start transferring.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI will display as modal.</param>
        /// <param name="windowHandle">The window handle if modal.</param>
        /// <returns></returns>
        ReturnCode Enable(SourceEnableMode mode, bool modal, IntPtr windowHandle);

        /// <summary>
        /// Gets the source status. Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        TWStatus GetStatus();

        /// <summary>
        /// Gets the source status. Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        TWStatusUtf8 GetStatusUtf8();
    }
}
