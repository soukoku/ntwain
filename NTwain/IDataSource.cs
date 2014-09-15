using System;
namespace NTwain
{
    /// <summary>
    /// Represents a TWAIN data source.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Gets the source's product name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        string Name { get; }
    }
}
