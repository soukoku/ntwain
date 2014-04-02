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
    sealed class UserInterface : OpBase
    {
        internal UserInterface(TwainSession session) : base(session) { }
        /// <summary>
        /// This operation causes the Source’s user interface, if displayed during the
        /// EnableDS operation, to be lowered. The Source is returned to
        /// State 4, where capability negotiation can again occur. The application can invoke this operation
        /// either because it wants to shut down the current session, or in response to the Source "posting"
        /// a CloseDSReq event to it. Rarely, the application may need to close the Source because an
        /// error condition was detected.
        /// </summary>
        /// <param name="userInterface">The user interface.</param>
        /// <returns></returns>
        public ReturnCode DisableDS(TWUserInterface userInterface)
        {
            Session.VerifyState(5, 5, DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS);
            var rc = PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.DisableDS, userInterface);
            if (rc == ReturnCode.Success)
            {
                Session.State = 4;
            }
            return rc;
        }

        /// <summary>
        /// Enables the DS.
        /// </summary>
        /// <param name="userInterface">The user interface.</param>
        /// <returns></returns>
        public ReturnCode EnableDS(TWUserInterface userInterface)
        {
            Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDS);
            using (var pending = Session.GetPendingStateChanger(5))
            {
                var rc = PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.EnableDS, userInterface);
                if (rc == ReturnCode.Success ||
                    (!userInterface.ShowUI && rc == ReturnCode.CheckStatus))
                {
                    pending.Commit();
                }
                return rc;
            }
        }

        /// <summary>
        /// This operation is used by applications
        /// that wish to display the source user interface to allow the user to manipulate the sources current
        /// settings for DPI, paper size, etc. but not acquire an image.
        /// </summary>
        /// <param name="userInterface">The user interface.</param>
        /// <returns></returns>
        public ReturnCode EnableDSUIOnly(TWUserInterface userInterface)
        {
            Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDSUIOnly);
            using (var pending = Session.GetPendingStateChanger(5))
            {
                var rc = PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.EnableDSUIOnly, userInterface);
                if (rc == ReturnCode.Success)
                {
                    pending.Commit();
                }
                return rc;
            }
        }
    }
}