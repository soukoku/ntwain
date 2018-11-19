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
            
            Is32Bit = session.Config.Is32Bit;
            IsWin = session.Config.Platform == System.PlatformID.Win32NT;
            IsLinux = session.Config.Platform == System.PlatformID.Unix;
            IsMac = session.Config.Platform == System.PlatformID.MacOSX;
        }

        /// <summary>
        /// Whether to use 32bit data structures.
        /// </summary>
        protected readonly bool Is32Bit;

        /// <summary>
        /// Whether platform is Windows.
        /// </summary>
        protected readonly bool IsWin;
        /// <summary>
        /// Whether platform is Linux.
        /// </summary>
        protected readonly bool IsLinux;
        /// Whether platform is MacOS.
        protected readonly bool IsMac;
    }
}