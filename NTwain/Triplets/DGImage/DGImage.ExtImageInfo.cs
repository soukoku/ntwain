using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	public sealed class ExtImageInfo : OpBase
	{
		internal ExtImageInfo(TwainSession session) : base(session) { }

		public ReturnCode Get(TWExtImageInfo info)
		{
			Session.VerifyState(7, 7, DataGroups.Image, DataArgumentType.ExtImageInfo, Message.Get);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, info);
		}
	}
}