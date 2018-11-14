using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Base class for grouping triplet operations.
    /// </summary>
    public abstract class BaseTriplet
    {
        /// <summary>
        /// Gets the associated <see cref="TwainSession"/>.
        /// </summary>
        protected readonly TwainSession Session;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTriplet" /> class.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        protected BaseTriplet(TwainSession session)
        {
            this.Session = session ?? throw new ArgumentNullException(nameof(session));
        }
    }
}