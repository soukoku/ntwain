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
using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	public sealed class RgbResponse : OpBase
	{
		internal RgbResponse(TwainSession session) : base(session) { }

		/// <summary>
		/// Causes the Source to use its "identity" response curves for future RGB transfers. The identity
		/// curve causes no change in the values of the captured data when it is applied. (Note that
		/// resetting the curves for RGB data does not reset any curves for other pixel types).
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public ReturnCode Reset(out TWRgbResponse response)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.RgbResponse, Message.Reset);
			response = new TWRgbResponse();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, response);
		}

		/// <summary>
		/// Causes the Source to transform any RGB data according to the response curves specified by the
		/// application.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public ReturnCode Set(TWRgbResponse response)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.RgbResponse, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, response);
		}
	}
}