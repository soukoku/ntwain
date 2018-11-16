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
		/// <value>The device event.</value>
		public TW_DEVICEEVENT Data { get; internal set; }
	}
}
