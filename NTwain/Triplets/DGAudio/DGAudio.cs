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
namespace NTwain.Triplets
{
	/// <summary>
	/// Class for grouping triplet operations under the Audio data group.
	/// </summary>
	public sealed class DGAudio
	{
		TwainSession _session;
		internal DGAudio(TwainSession session)
		{
			if (session == null) { throw new ArgumentNullException("session"); }
			_session = session;
		}


		AudioFileXfer _audioFileXfer;
		internal AudioFileXfer AudioFileXfer
		{
			get
			{
				if (_audioFileXfer == null) { _audioFileXfer = new AudioFileXfer(_session); }
				return _audioFileXfer;
			}
		}

		AudioInfo _audioInfo;
		public AudioInfo AudioInfo
		{
			get
			{
				if (_audioInfo == null) { _audioInfo = new AudioInfo(_session); }
				return _audioInfo;
			}
		}

		AudioNativeXfer _audioNativeXfer;
		internal AudioNativeXfer AudioNativeXfer
		{
			get
			{
				if (_audioNativeXfer == null) { _audioNativeXfer = new AudioNativeXfer(_session); }
				return _audioNativeXfer;
			}
		}
	}
}
