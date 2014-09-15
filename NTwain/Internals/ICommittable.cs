using System;

namespace NTwain.Internals
{
    /// <summary>
    /// For something that is in a pending state until finalized with a Commit() call.
    /// The changes are rolled back if it is disposed without being committed.
    /// </summary>
    interface ICommittable : IDisposable
    {
        /// <summary>
        /// Commits the pending changes.
        /// </summary>
        void Commit();
    }
}
