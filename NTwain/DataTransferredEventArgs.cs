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

namespace NTwain
{
	/// <summary>
	/// Contains event data after whatever data from the source has been transferred.
	/// </summary>
	public class DataTransferredEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataTransferredEventArgs" /> class.
		/// </summary>
		/// <param name="data">The data pointer.</param>
		/// <param name="filePath">The file.</param>
		internal DataTransferredEventArgs(IntPtr data, string filePath)
		{
			Data = data;
			FilePath = filePath;
		}
		/// <summary>
		/// Gets pointer to the image data if applicable.
		/// The data will be freed once the event handler ends
		/// so consumers must complete whatever processing
		/// required by then.
		/// </summary>
		/// <value>The data pointer.</value>
		public IntPtr Data { get; private set; }

		/// <summary>
		/// Gets the filepath to the transferrerd data if applicable.
		/// </summary>
		/// <value>
		/// The file.
		/// </value>
		public string FilePath { get; private set; }
	}
}