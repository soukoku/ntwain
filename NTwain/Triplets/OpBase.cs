using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTwain.Values;

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
        internal OpBase(ITwainSessionInternal session)
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
        internal ITwainSessionInternal Session { get; private set; }
	}
}
