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
	public sealed class Status : OpBase
	{
		internal Status(TwainSession session) : base(session) { }
		/// <summary>
		/// Returns the current Condition Code for the Source Manager.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns></returns>
		public ReturnCode GetManager(out TWStatus status)
		{
			Session.VerifyState(2, 7, DataGroups.Control, DataArgumentType.Status, Message.Get);
			status = new TWStatus();
			return PInvoke.DsmEntry(Session.AppId, null, Message.Get, status);
		}

		/// <summary>
		/// Returns the current Condition Code for the specified Source.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns></returns>
		public ReturnCode GetSource(out TWStatus status)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.Status, Message.Get);
			status = new TWStatus();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, status);
		}
	}
}