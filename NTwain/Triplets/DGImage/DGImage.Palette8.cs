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
	public sealed class Palette8 : OpBase
	{
		internal Palette8(TwainSession session) : base(session) { }

		/// <summary>
		/// This operation causes the Source to report its current palette information.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWPalette8 palette)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.Palette8, Message.Get);
			palette = new TWPalette8();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, palette);
		}

		/// <summary>
		/// This operation causes the Source to report its power-on default palette.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode GetDefault(out TWPalette8 palette)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.Palette8, Message.GetDefault);
			palette = new TWPalette8();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.GetDefault, palette);
		}

		/// <summary>
		/// This operation causes the Source to dispose of any current palette it has and to use its default
		/// palette for the next palette transfer.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode Reset(out TWPalette8 palette)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.Palette8, Message.Reset);
			palette = new TWPalette8();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, palette);
		}

		/// <summary>
		/// This operation requests that the Source adopt the specified palette for use with all subsequent
		/// palette transfers. The application should be careful to supply a palette that matches the bit
		/// depth of the Source. The Source is not required to adopt this palette. The application should be
		/// careful to check the return value from this operation.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode Set(TWPalette8 palette)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.Palette8, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, palette);
		}
	}
}