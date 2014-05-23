using NTwain.Data;
using NTwain.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    /// <summary>
    /// Provides direct access to the triplet call.
    /// </summary>
    public class DGCustom
    {
        ITwainSessionInternal _session;
        internal DGCustom(ITwainSessionInternal session)
        {
            if (session == null) { throw new ArgumentNullException("session"); }
            _session = session;
        }


        /// <summary>
        /// Direct DSM_Entry call with full arguments for custom values.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="dat">The dat.</param>
        /// <param name="message">The message.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public ReturnCode DsmEntry(
            DataGroups group,
            DataArgumentType dat,
            Message message,
            ref IntPtr data)
        {
            _session.VerifyState(3, 7, group, dat, message);
            return Dsm.DsmEntry(_session.AppId, _session.CurrentSource.Identity, group, dat, message, ref data);
        }

        // todo: add other data value types?
    }
}
