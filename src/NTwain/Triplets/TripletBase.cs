using System;

namespace NTwain.Triplets
{
  /// <summary>
  /// Base class for grouping triplet operations messages.
  /// </summary>
  public abstract class TripletBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="TripletBase" /> class.
    /// </summary>
    /// <param name="session">The session.</param>
    /// <exception cref="System.ArgumentNullException"></exception>
    public TripletBase(TwainSession session)
    {
      Session = session ?? throw new ArgumentNullException(nameof(session));
    }

    /// <summary>
    /// Gets the twain session.
    /// </summary>
    public TwainSession Session { get; }
  }
}
