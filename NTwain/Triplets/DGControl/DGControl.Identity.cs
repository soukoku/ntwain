using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.Identity"/>.
    /// </summary>
	public sealed class Identity : OpBase
	{
		internal Identity(ITwainStateInternal session) : base(session) { }
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
			var rc = Dsm.DsmEntry(Session.AppId, Message.CloseDS, Session.SourceId);
			if (rc == ReturnCode.Success)
            {
                Session.ChangeSourceId(null);
                Session.ChangeState(3, true);
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
			return Dsm.DsmEntry(Session.AppId, Message.GetDefault, source);
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
			return Dsm.DsmEntry(Session.AppId, Message.GetFirst, source);
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
			return Dsm.DsmEntry(Session.AppId, Message.GetNext, source);
		}

		/// <summary>
		/// Loads the specified Source into main memory and causes its initialization.
		/// </summary>
		/// <param name="source">The source.</param>
		/// <returns></returns>
		internal ReturnCode OpenDS(TWIdentity source)
		{
			Session.VerifyState(3, 3, DataGroups.Control, DataArgumentType.Identity, Message.OpenDS);
			var rc = Dsm.DsmEntry(Session.AppId, Message.OpenDS, source);
			if (rc == ReturnCode.Success)
            {
                Session.ChangeSourceId(source);
                Session.ChangeState(4, true);
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
			return Dsm.DsmEntry(Session.AppId, Message.Set, source);
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
			return Dsm.DsmEntry(Session.AppId, Message.UserSelect, source);
		}
	}
}