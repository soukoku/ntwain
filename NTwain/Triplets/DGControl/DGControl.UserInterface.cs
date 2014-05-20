using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.UserInterface"/>.
    /// </summary>
    sealed class UserInterface : OpBase
    {
        internal UserInterface(ITwainSessionInternal session) : base(session) { }
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
            var rc = Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.DisableDS, userInterface);
            if (rc == ReturnCode.Success)
            {
                Session.ChangeState(4, true);
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
                var rc = Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.EnableDS, userInterface);
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
                var rc = Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.EnableDSUIOnly, userInterface);
                if (rc == ReturnCode.Success)
                {
                    pending.Commit();
                }
                return rc;
            }
        }
    }
}