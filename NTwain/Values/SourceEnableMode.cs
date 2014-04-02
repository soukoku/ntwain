namespace NTwain.Values
{
	/// <summary>
	/// Indicates how the source should be enabled in a TWAIN session.
	/// </summary>
	public enum SourceEnableMode
	{
		/// <summary>
		/// Start acquiring without driver UI.
		/// </summary>
		NoUI,
		/// <summary>
		/// Start acquiring with driver UI.
		/// </summary>
		ShowUI,
		/// <summary>
		/// Show driver UI for settings change but no acquisition.
		/// </summary>
		ShowUIOnly
	}
}
