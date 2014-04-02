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
using NTwain.Values;

namespace NTwain.Triplets
{
	sealed class AudioNativeXfer : OpBase
	{
		internal AudioNativeXfer(TwainSession session) : base(session) { }
		/// <summary>
		/// Causes the transfer of an audio data from the Source to the application, via the Native
		/// transfer mechanism, to begin. The resulting data is stored in main memory in a single block.
		/// The data is stored in AIFF format on the Macintosh and as a WAV format under Microsoft
		/// Windows. The size of the audio snippet that can be transferred is limited to the size of the
		/// memory block that can be allocated by the Source.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		public ReturnCode Get(ref IntPtr handle)
		{
			Session.VerifyState(6, 6, DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);
		}
	}
}