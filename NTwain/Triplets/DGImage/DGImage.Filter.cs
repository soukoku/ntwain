using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.Filter"/>.
    /// </summary>
	public sealed class Filter : OpBase
	{
        internal Filter(ITwainSessionInternal session) : base(session) { }


        /// <summary>
        /// Causes the Source to return the filter parameters that will be used during the next image
        /// acquisition.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Get(out TWFilter filter)
        {
            Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.Filter, Message.Get);
            filter = new TWFilter();
            return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.Get, filter);
        }

        /// <summary>
        /// Causes the Source to return the power-on default values applied to the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode GetDefault(out TWFilter filter)
        {
            Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.Filter, Message.GetDefault);
            filter = new TWFilter();
            return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.GetDefault, filter);
        }

        /// <summary>
        /// Allows the Application to configure the filter parameters that will be used during the next image
        /// acquisition.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public ReturnCode Set(TWFilter filter)
        {
            Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.Filter, Message.Set);
            return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.Set, filter);
        }

        /// <summary>
        /// Return the Source to using its power-on default values when it is applying the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Reset(out TWFilter filter)
        {
            Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.Filter, Message.Reset);
            filter = new TWFilter();
            return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.Reset, filter);
        }
	}
}