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
	public sealed class Identity : OpBase
	{
		internal Identity(TwainSession session) : base(session) { }
		/// <summary>
		/// When an application is finished with a Source, it must formally close the session between them
		/// using this operation. This is necessary in case the Source only supports connection with a single
		/// application (many desktop scanners will behave this way). A Source such as this cannot be
		/// accessed by other applications until its current session is terminated.
		/// </summary>
		/// <returns></returns>
		internal ReturnCode CloseDS()
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.Identity, Message.CloseDS);
			var rc = PInvoke.DsmEntry(Session.AppId, Message.CloseDS, Session.SourceId);
			if (rc == ReturnCode.Success)
			{
				Session.State = 3;
			}
			return rc;
		}

		/// <summary>
		/// Gets the identification information of the system default Source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public ReturnCode GetDefault(out TWIdentity source)
		{
			Session.VerifyState(3, 7, DataGroups.Control, DataArgumentType.Identity, Message.GetDefault);
			source = new TWIdentity();
			return PInvoke.DsmEntry(Session.AppId, Message.GetDefault, source);
		}


		/// <summary>
		/// The application may obtain the first Source that are currently available on the system which
		/// match the application’s supported groups.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public ReturnCode GetFirst(out TWIdentity source)
		{
			Session.VerifyState(3, 7, DataGroups.Control, DataArgumentType.Identity, Message.GetFirst);
			source = new TWIdentity();
			return PInvoke.DsmEntry(Session.AppId, Message.GetFirst, source);
		}

		/// <summary>
		/// The application may obtain the next Source that are currently available on the system which
		/// match the application’s supported groups.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public ReturnCode GetNext(out TWIdentity source)
		{
			Session.VerifyState(3, 7, DataGroups.Control, DataArgumentType.Identity, Message.GetNext);
			source = new TWIdentity();
			return PInvoke.DsmEntry(Session.AppId, Message.GetNext, source);
		}

		/// <summary>
		/// Loads the specified Source into main memory and causes its initialization.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		internal ReturnCode OpenDS(TWIdentity source)
		{
			Session.VerifyState(3, 3, DataGroups.Control, DataArgumentType.Identity, Message.OpenDS);
			var rc = PInvoke.DsmEntry(Session.AppId, Message.OpenDS, source);
			if (rc == ReturnCode.Success)
			{
				Session.State = 4;
			}
			return rc;
		}


		/// <summary>
		/// It allows an application to set the
		/// default TWAIN driver, which is reported back by GetDefault.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public ReturnCode Set(TWIdentity source)
		{
			Session.VerifyState(3, 3, DataGroups.Control, DataArgumentType.Identity, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Message.Set, source);
		}

		/// <summary>
		/// This operation should be invoked when the user chooses Select Source... from the application’s
		/// File menu (or an equivalent user action). The Source selected becomes the system default Source.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		public ReturnCode UserSelect(out TWIdentity source)
		{
			Session.VerifyState(3, 7, DataGroups.Control, DataArgumentType.Identity, Message.UserSelect);
			source = new TWIdentity();
			return PInvoke.DsmEntry(Session.AppId, Message.UserSelect, source);
		}
	}
}