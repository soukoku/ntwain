using System;

namespace NTwain.Threading
{
    /// <summary>
    /// Interface for running some work on an associated thread.
    /// </summary>
    interface IThreadContext
    {
        /// <summary>
        /// Runs the action synchronously on the associated thread.
        /// </summary>
        /// <param name="action"></param>
        void Invoke(Action action);

        /// <summary>
        /// Runs the action asynchronously on the associated thread.
        /// </summary>
        /// <param name="action"></param>
        void BeginInvoke(Action action);
    }
}
