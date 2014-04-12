using System;
using NTwain.Data;
using NTwain.Values;
using System.Runtime.InteropServices;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.Parent"/>.
    /// </summary>
    sealed class Parent : OpBase
    {
        internal Parent(ITwainStateInternal session) : base(session) { }

        /// <summary>
        /// When the application has closed all the Sources it had previously opened, and is finished with
        /// the Source Manager (the application plans to initiate no other TWAIN sessions), it must close
        /// the Source Manager.
        /// </summary>
        /// <param name="handle">The handle. On Windows = points to the window handle (hWnd) that will act as the Source’s
        /// "parent". On Macintosh = should be a NULL value.</param>
        /// <returns></returns>
        public ReturnCode CloseDsm(IntPtr handle)
        {
            Session.VerifyState(3, 3, DataGroups.Control, DataArgumentType.Parent, Message.CloseDsm);
            var rc = Dsm.DsmEntry(Session.GetAppId(), null, DataGroups.Control, DataArgumentType.Parent, Message.CloseDsm, ref handle);
            if (rc == ReturnCode.Success)
            {
                Session.ChangeState(2, true);
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
        public ReturnCode OpenDsm(IntPtr handle)
        {
            Session.VerifyState(1, 2, DataGroups.Control, DataArgumentType.Parent, Message.OpenDsm);
            var rc = Dsm.DsmEntry(Session.GetAppId(), null, DataGroups.Control, DataArgumentType.Parent, Message.OpenDsm, ref handle);
            if (rc == ReturnCode.Success)
            {
                Session.ChangeState(3, true);
            }
            return rc;
        }
    }
}