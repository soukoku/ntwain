using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	public sealed class ImageInfo : OpBase
	{
		internal ImageInfo(TwainSession session) : base(session) { }

		public ReturnCode Get(out TWImageInfo info)
		{
			Session.VerifyState(6, 7, DataGroups.Image, DataArgumentType.ImageInfo, Message.Get);
			info = new TWImageInfo();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, info);
		}
	}
}