using NTwain.Data;
using System.ComponentModel;
namespace NTwain
{
    /// <summary>
    /// Interface for keeping track of current TWAIN state with current app and source ids.
    /// </summary>
    public interface ITwainState : INotifyPropertyChanged
    {   
        /// <summary>
        /// Gets the source id used for the session.
        /// </summary>
        /// <value>The source id.</value>
        TWIdentity SourceId { get; }

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>The state.</value>
        int State { get; }
    }
}
