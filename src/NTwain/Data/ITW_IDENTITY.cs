namespace NTwain.Data
{
    /// <summary>
    /// Provides identification information about a TWAIN entity. Used to maintain consistent
    /// communication between entities.
    /// </summary>
    public interface ITW_IDENTITY
    {
        /// <summary>
        /// Gets the data functionalities for TWAIN 2 detection.
        /// </summary>
        /// <value>The data functionalities.</value>
        DataFlags DataFlags { get; }

        /// <summary>
        /// Gets the supported data group. The application will normally set this field to specify which Data
        /// Group(s) it wants the Source Manager to sort Sources by when
        /// presenting the Select Source dialog, or returning a list of available
        /// Sources.
        /// </summary>
        /// <value>The data group.</value>
        DataGroups DataGroup { get; }

        /// <summary>
        /// String identifying the manufacturer of the application or Source. e.g. "Aldus".
        /// </summary>
        string Manufacturer { get; }
        /// <summary>
        /// Tells an application that performs device-specific operations which
        /// product family the Source supports. This is useful when a new Source
        /// has been released and the application doesn't know about the
        /// particular Source but still wants to perform Custom operations with it.
        /// e.g. "ScanMan".
        /// </summary>
        string ProductFamily { get; }
        /// <summary>
        /// A string uniquely identifying the Source. This is the string that will be
        /// displayed to the user at Source select-time. This string must uniquely
        /// identify your Source for the user, and should identify the application
        /// unambiguously for Sources that care. e.g. "ScanJet IIc".
        /// </summary>
        string ProductName { get; }

        /// <summary>
        /// Major number of latest TWAIN version that this element supports.
        /// </summary>
        short ProtocolMajor { get; }
        /// <summary>
        /// Minor number of latest TWAIN version that this element supports.
        /// </summary>
        short ProtocolMinor { get; }

        /// <summary>
        /// A <see cref="TW_VERSION"/> structure identifying the TWAIN entity.
        /// </summary>
        TW_VERSION Version { get; }
    }
}