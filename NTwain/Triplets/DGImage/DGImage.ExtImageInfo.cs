using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ExtImageInfo"/>.
    /// </summary>
	public sealed class ExtImageInfo : OpBase
	{
		internal ExtImageInfo(ITwainStateInternal session) : base(session) { }

		public ReturnCode Get(TWExtImageInfo info)
		{
			Session.VerifyState(7, 7, DataGroups.Image, DataArgumentType.ExtImageInfo, Message.Get);
			return PInvoke.DsmEntry(Session.GetAppId(), Session.SourceId, Message.Get, info);
		}
	}
}