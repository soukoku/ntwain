using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Interface for providing TWAIN triplet access.
    /// </summary>
    public interface ITripletControl
    {
        /// <summary>
        /// Gets the triplet operations defined for control data group.
        /// </summary>
        DGControl DGControl { get; }

        /// <summary>
        /// Gets the triplet operations defined for image data group.
        /// </summary>
        DGImage DGImage { get; }

        /// <summary>
        /// Gets the direct triplet operation entry for custom values.
        /// </summary>
        DGCustom DGCustom { get; }
    }
}
