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
	sealed class AudioFileXfer : OpBase
	{
		internal AudioFileXfer(TwainSession session) : base(session) { }
		/// <summary>
		/// This operation is used to initiate the transfer of audio from the Source to the application via the
		/// disk-file transfer mechanism. It causes the transfer to begin.
		/// </summary>
		/// <returns></returns>
		public ReturnCode Get()
		{
			Session.VerifyState(6, 6, DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get);
			IntPtr z = IntPtr.Zero;
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref z);
		}
	}
}