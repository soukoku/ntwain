// The MIT License (MIT)
// Copyright (c) 2013 Yin-Chun Wang
//
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF 
// OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
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
