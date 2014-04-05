using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.GrayResponse"/>.
    /// </summary>
	public sealed class GrayResponse : OpBase
	{
		internal GrayResponse(ITwainStateInternal session) : base(session) { }

		/// <summary>
		/// The Reset operation causes the Source to use its "identity response curve." The identity
		/// curve causes no change in the values of the captured data when it is applied.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public ReturnCode Reset(out TWGrayResponse response)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.GrayResponse, Message.Reset);
			response = new TWGrayResponse();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, response);
		}

		/// <summary>
		/// This operation causes the Source to transform any grayscale data according to the response
		/// curve specified.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public ReturnCode Set(TWGrayResponse response)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.GrayResponse, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, response);
		}
	}
}