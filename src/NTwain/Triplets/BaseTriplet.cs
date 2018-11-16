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

            // windows can always use 32bit structs even in 64bit app
            Use32BitData = session.Config.Platform == System.PlatformID.Win32NT ||
                !session.Config.Is64Bit;
        }

        /// <summary>
        /// Whether to use 32bit data structures.
        /// </summary>
        protected readonly bool Use32BitData;
    }
}