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
namespace NTwain.Values
{
	/// <summary>
	/// Contains magic number for some TWAIN stuff.
	/// </summary>
	public static class TwainConst
	{
		// these are specified here since the actual
		// array length doesn't reflect the name :(

		/// <summary>
		/// Length of an array that holds TW_STR32.
		/// </summary>
		public const int String32 = 34;
		/// <summary>
		/// Length of an array that holds TW_STR64.
		/// </summary>
		public const int String64 = 66;
		/// <summary>
		/// Length of an array that holds TW_STR128.
		/// </summary>
		public const int String128 = 130;
		/// <summary>
		/// Length of an array that holds TW_STR255.
		/// </summary>
		public const int String255 = 256;

		public const int String1024 = 1026;

		/// <summary>
		/// Don't care value for 8 bit types.
		/// </summary>
		public const byte DontCare8 = 0xff;
		/// <summary>
		/// Don't care value for 16 bit types.
		/// </summary>
		public const ushort DontCare16 = 0xffff;
		/// <summary>
		/// Don't care value for 32 bit types.
		/// </summary>
		public const uint DontCare32 = 0xffffffff;

		/// <summary>
		/// The major version number of TWAIN supported by this library.
		/// </summary>
		public const short ProtocolMajor = 2;
		/// <summary>
		/// The minor version number of TWAIN supported by this library.
		/// </summary>
		public const short ProtocolMinor = 2;

		/// <summary>
		/// Value for false where applicable.
		/// </summary>
		public const ushort True = 1;
		/// <summary>
		/// Value for true where applicable.
		/// </summary>
		public const ushort False = 0;
	}
}
