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
using System.Runtime.InteropServices;

namespace NTwain.Triplets
{
    sealed class Parent : OpBase
    {
        internal Parent(TwainSession session) : base(session) { }

        /// <summary>
        /// When the application has closed all the Sources it had previously opened, and is finished with
        /// the Source Manager (the application plans to initiate no other TWAIN sessions), it must close
        /// the Source Manager.
        /// </summary>
        /// <param name="handle">The handle. On Windows = points to the window handle (hWnd) that will act as the Source’s
        /// "parent". On Macintosh = should be a NULL value.</param>
        /// <returns></returns>
        public ReturnCode CloseDsm(ref IntPtr handle)
        {
            Session.VerifyState(3, 3, DataGroups.Control, DataArgumentType.Parent, Message.CloseDsm);
            var rc = PInvoke.DsmEntry(Session.AppId, null, DataGroups.Control, DataArgumentType.Parent, Message.CloseDsm, ref handle);
            if (rc == ReturnCode.Success)
            {
                Session.State = 2;
            }
            return rc;
        }

        /// <summary>
        /// Causes the Source Manager to initialize itself. This operation must be executed before any other
        /// operations will be accepted by the Source Manager.
        /// </summary>
        /// <param name="handle">The handle. On Windows = points to the window handle (hWnd) that will act as the Source’s
        /// "parent". On Macintosh = should be a NULL value.</param>
        /// <returns></returns>
        public ReturnCode OpenDsm(ref IntPtr handle)
        {
            Session.VerifyState(1, 2, DataGroups.Control, DataArgumentType.Parent, Message.OpenDsm);
            var rc = PInvoke.DsmEntry(Session.AppId, null, DataGroups.Control, DataArgumentType.Parent, Message.OpenDsm, ref handle);
            if (rc == ReturnCode.Success)
            {
                Session.State = 3;
            }
            return rc;
        }
    }
}