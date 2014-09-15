using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ExtImageInfo"/>.
    /// </summary>
    public sealed class ExtImageInfo : TripletBase
    {
        internal ExtImageInfo(ITwainSessionInternal session) : base(session) { }

        /// <summary>
        /// This operation is used by the application to query the data source for extended image attributes.
        /// The application is responsible for creating and disiposing the info object.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        public ReturnCode Get(TWExtImageInfo info)
        {
            Session.VerifyState(7, 7, DataGroups.Image, DataArgumentType.ExtImageInfo, Message.Get);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, info);
        }
    }
}