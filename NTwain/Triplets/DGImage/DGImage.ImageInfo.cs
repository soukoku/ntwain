using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ImageInfo"/>.
    /// </summary>
    sealed class ImageInfo : TripletBase
    {
        internal ImageInfo(ITwainSessionInternal session) : base(session) { }

        public ReturnCode Get(out TWImageInfo info)
        {
            Session.VerifyState(6, 7, DataGroups.Image, DataArgumentType.ImageInfo, Message.Get);
            info = new TWImageInfo();
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, info);
        }
    }
}