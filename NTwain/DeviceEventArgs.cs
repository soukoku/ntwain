using System;
using NTwain.Data;

namespace NTwain
{
	/// <summary>
	/// Contains event data for a TWAIN source hardware event.
	/// </summary>
	public class DeviceEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceEventArgs"/> class.
		/// </summary>
		/// <param name="deviceEvent">The device event.</param>
		internal DeviceEventArgs(TWDeviceEvent deviceEvent)
		{
			DeviceEvent = deviceEvent;
		}
		/// <summary>
		/// Gets the detailed device event.
		/// </summary>
		/// <value>The device event.</value>
		public TWDeviceEvent DeviceEvent { get; private set; }
	}
}
