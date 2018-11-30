using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.StatusUtf8"/>.
    /// </summary>
	sealed class StatusUtf8 : TripletBase
	{
		internal StatusUtf8(ITwainSessionInternal session) : base(session) { }

        /// <summary>
        /// Translate the contents of a TW_STATUS structure received from the manager into a localized UTF-8
        /// encoded string.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public ReturnCode GetManager(TWStatus status, out TWStatusUtf8 utf8status)
        {
            utf8status = new TWStatusUtf8 { Status = status };
            Session.VerifyState(3, 7, DataGroups.Control, DataArgumentType.StatusUtf8, Message.Get);
            return Dsm.DsmEntry(Session.AppId, null, Message.Get, status);
        }

		/// <summary>
		/// Translate the contents of a TW_STATUS structure received from a Source into a localized UTF-8
		/// encoded string.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns></returns>
		public ReturnCode GetSource(TWStatus status, out TWStatusUtf8 utf8status)
		{
            utf8status = new TWStatusUtf8 { Status = status };
			Session.VerifyState(3, 7, DataGroups.Control, DataArgumentType.StatusUtf8, Message.Get);
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, status);
		}
	}
}