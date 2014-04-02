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
	public sealed class CustomDSData : OpBase
	{
		internal CustomDSData(TwainSession session) : base(session) { }
		/// <summary>
		/// This operation is used by the application to query the data source for its current settings, e.g.
        /// DPI, paper size, color format. The actual format of the data is data source dependent and not
		/// defined by TWAIN.
		/// </summary>
		/// <param name="customData">The custom data.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWCustomDSData customData)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.CustomDSData, Message.Get);
			customData = new TWCustomDSData();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, customData);
		}

		/// <summary>
		/// This operation is used by the application to set the current settings for a data source to a
		/// previous state as defined by the data contained in the customData data structure. The actual
		/// format of the data is data source dependent and not defined by TWAIN.
		/// </summary>
		/// <param name="customData">The custom data.</param>
		/// <returns></returns>
		public ReturnCode Set(TWCustomDSData customData)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.CustomDSData, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, customData);
		}
	}
}