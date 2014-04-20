using NTwain.Internals;
using System;

namespace NTwain.Triplets
{
	/// <summary>
	/// Base class for grouping triplet operations messages.
	/// </summary>
	public abstract class OpBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OpBase" /> class.
		/// </summary>
		/// <param name="session">The session.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
        internal OpBase(ITwainStateInternal session)
		{
			if (session == null) { throw new ArgumentNullException("session"); }
			Session = session;
		}

		/// <summary>
		/// Gets the twain session.
		/// </summary>
		/// <value>
		/// The session.
		/// </value>
        internal ITwainStateInternal Session { get; private set; }
	}
}
