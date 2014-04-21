using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.IccProfile"/>.
    /// </summary>
	public sealed class IccProfile : OpBase
	{
		internal IccProfile(ITwainSessionInternal session) : base(session) { }

		/// <summary>
		/// This operation provides the application with the ICC profile associated with the image which is
		/// about to be transferred (state 6) or is being transferred (state 7).
		/// </summary>
		/// <param name="profile">The profile.</param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public ReturnCode Get(ref TWMemory profile)
		{
			Session.VerifyState(6, 7, DataGroups.Image, DataArgumentType.IccProfile, Message.Get);
			profile = new TWMemory();
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, DataArgumentType.IccProfile, Message.Get, ref profile);
		}
	}
}