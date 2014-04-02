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
	public sealed class ImageLayout : OpBase
	{
		internal ImageLayout(TwainSession session) : base(session) { }

		public ReturnCode Get(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.Get);
			layout = new TWImageLayout();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, layout);
		}

		public ReturnCode GetDefault(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.GetDefault);
			layout = new TWImageLayout();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.GetDefault, layout);
		}

		public ReturnCode Reset(out TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Reset);
			layout = new TWImageLayout();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, layout);
		}

		public ReturnCode Set(TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, layout);
		}
	}
}