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
	sealed class PendingXfers : OpBase
	{
		internal PendingXfers(TwainSession session) : base(session) { }
		/// <summary>
		/// This triplet is used to cancel or terminate a transfer. Issued in state 6, this triplet cancels the next
		/// pending transfer, discards the transfer data, and decrements the pending transfers count. In
		/// state 7, this triplet terminates the current transfer. If any data has not been transferred (this is
		/// only possible during a memory transfer) that data is discarded.
		/// </summary>
		/// <param name="pendingXfers">The pending xfers.</param>
		/// <returns></returns>
		internal ReturnCode EndXfer(TWPendingXfers pendingXfers)
		{
			Session.VerifyState(6, 7, DataGroups.Control, DataArgumentType.PendingXfers, Message.EndXfer);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.EndXfer, pendingXfers);
		}

		/// <summary>
		/// Returns the number of transfers the Source is ready to supply to the application, upon demand.
		/// If DAT_XFERGROUP is set to DG_Image, this is the number of images. If DAT_XFERGROUP is set
		/// to DG_AUDIO, this is the number of audio snippets for the current image. If there is no current
		/// image, this call must return Failure / SeqError.
		/// </summary>
		/// <param name="pendingXfers">The pending xfers.</param>
		/// <returns></returns>
		public ReturnCode Get(TWPendingXfers pendingXfers)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.PendingXfers, Message.Get);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, pendingXfers);
		}

		/// <summary>
		/// Sets the number of pending transfers in the Source to zero.
		/// </summary>
		/// <param name="pendingXfers">The pending xfers.</param>
		/// <returns></returns>
		internal ReturnCode Reset(TWPendingXfers pendingXfers)
		{
			Session.VerifyState(6, 6, DataGroups.Control, DataArgumentType.PendingXfers, Message.Reset);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, pendingXfers);
		}

		/// <summary>
		/// If CapAutoScan is TRUE, this command will stop the operation of the scanner’s automatic
		/// feeder. No other action is taken.
		/// </summary>
		/// <param name="pendingXfers">The pending xfers.</param>
		/// <returns></returns>
		public ReturnCode StopFeeder(TWPendingXfers pendingXfers)
		{
			Session.VerifyState(6, 6, DataGroups.Control, DataArgumentType.PendingXfers, Message.StopFeeder);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.StopFeeder, pendingXfers);
		}
	}
}