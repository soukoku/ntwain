// this file tracks twain state and has methods that can change the twain state and 
// manage data sources.

namespace NTwain;

/// <summary>
/// Options for enabling a data source.
/// </summary>
public enum SourceEnableOption
{
    /// <summary>
    /// Start the transfer without showing the driver settings UI.
    /// Progress indicator UI may still be shown.
    /// </summary>
    NoUI,
    /// <summary>
    /// Show the driver UI then start the transfer when user accepts.
    /// </summary>
    ShowUI,
    /// <summary>
    /// Show the driver UI but don't perform transfers (modify settings only).
    /// </summary>
    UIOnly
}
