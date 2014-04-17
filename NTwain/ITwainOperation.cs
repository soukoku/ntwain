using NTwain.Triplets;

namespace NTwain
{
    /// <summary>
    /// Interface for providing TWAIN triplet operations. 
    /// </summary>
    public interface ITwainOperation
    {
        /// <summary>
        /// Gets the triplet operations defined for audio data group.
        /// </summary>
        DGAudio DGAudio { get; }

        /// <summary>
        /// Gets the triplet operations defined for control data group.
        /// </summary>
        DGControl DGControl { get; }

        /// <summary>
        /// Gets the triplet operations defined for image data group.
        /// </summary>
        DGImage DGImage { get; }
    }
}
