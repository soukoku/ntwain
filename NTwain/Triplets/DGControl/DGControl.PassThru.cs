using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.PassThru"/>.
    /// </summary>
	public sealed class PassThru : OpBase
	{
		internal PassThru(ITwainStateInternal session) : base(session) { }
		/// <summary>
		/// PASSTHRU is intended for the use of Source writers writing diagnostic applications. It allows
		/// raw communication with the currently selected device in the Source.
		/// </summary>
		/// <param name="sourcePassThru">The source pass thru.</param>
		/// <returns></returns>
		public ReturnCode PassThrough(TWPassThru sourcePassThru)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.PassThru, Message.PassThru);
			return PInvoke.DsmEntry(Session.GetAppId(), Session.SourceId, Message.PassThru, sourcePassThru);
		}
	}
}