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
	sealed class Event : OpBase
	{
		internal Event(TwainSession session) : base(session) { }

		/// <summary>
		/// This operation supports the distribution of events from the application to Sources so that the
		/// Source can maintain its user interface and return messages to the application. Once the
		/// application has enabled the Source, it must immediately begin sending to the Source all events
		/// that enter the application’s main event loop. This allows the Source to update its user interface
		/// in real-time and to return messages to the application which cause state transitions. Even if the
		/// application overrides the Source’s user interface, it must forward all events once the Source has
		/// been enabled. The Source will tell the application whether or not each event belongs to the
		/// Source.
		/// </summary>
		/// <param name="theEvent">The event.</param>
		/// <returns></returns>
		public ReturnCode ProcessEvent(TWEvent theEvent)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.Event, Message.ProcessEvent);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.ProcessEvent, theEvent);
		}
	}
}