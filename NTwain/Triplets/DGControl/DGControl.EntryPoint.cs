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
	sealed class EntryPoint : OpBase
	{
		internal EntryPoint(TwainSession session) : base(session) { }
		/// <summary>
		/// Gets the function entry points for twain 2.0 or higher.
		/// </summary>
		/// <param name="entryPoint">The entry point.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWEntryPoint entryPoint)
		{
			Session.VerifyState(3, 3, DataGroups.Control, DataArgumentType.EntryPoint, Message.Get);
			entryPoint = new TWEntryPoint();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, entryPoint);
		}
	}
}