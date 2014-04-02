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
	public sealed class XferGroup : OpBase
	{
		internal XferGroup(TwainSession session) : base(session) { }
		/// <summary>
		/// Returns the Data Group (the type of data) for the upcoming transfer. The Source is required to
		/// only supply one of the DGs specified in the SupportedGroups field of origin.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public ReturnCode Get(out uint value)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.XferGroup, Message.Get);
			throw new NotImplementedException();

			// TODO: I have no idea if this even works, just something that looks logical.
			// Does memory from pointer need to be released once we got the value?

			//IntPtr ptr = IntPtr.Zero;
			//var rc = Custom.DsmEntry(Session.AppId, Session.SourceId, DataGroup.Control, DataArgumentType.XferGroup, Message.Get, ref ptr);
			//unsafe
			//{
			//    uint* realPtr = (uint*)ptr.ToPointer();
			//    value = (*realPtr);
			//}
			//return rc;
		}

		public ReturnCode Set(uint value)
		{
			Session.VerifyState(6, 6, DataGroups.Control, DataArgumentType.XferGroup, Message.Set);
			throw new NotImplementedException();
		}
	}
}