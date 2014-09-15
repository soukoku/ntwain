using NTwain.Data;
using System;

namespace NTwain
{
    /// <summary>
    /// Contains event data after whatever data from the source has been transferred.
    /// </summary>
    public class DataTransferredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferredEventArgs"/> class.
        /// </summary>
        /// <param name="nativeData">The native data.</param>
        /// <param name="imageInfo">The image information.</param>
        public DataTransferredEventArgs(IntPtr nativeData, TWImageInfo imageInfo)
        {
            NativeData = nativeData;
            ImageInfo = imageInfo;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferredEventArgs"/> class.
        /// </summary>
        /// <param name="fileDataPath">The file data path.</param>
        /// <param name="imageInfo">The image information.</param>
        public DataTransferredEventArgs(string fileDataPath, TWImageInfo imageInfo)
        {
            FileDataPath = fileDataPath;
            ImageInfo = imageInfo;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataTransferredEventArgs"/> class.
        /// </summary>
        /// <param name="memoryData">The memory data.</param>
        /// <param name="imageInfo">The image information.</param>
        public DataTransferredEventArgs(byte[] memoryData, TWImageInfo imageInfo)
        {
            MemoryData = memoryData;
            ImageInfo = imageInfo;
        }

        /// <summary>
        /// Gets pointer to the complete data if the transfer was native.
        /// The data will be freed once the event handler ends
        /// so consumers must complete whatever processing before then.
        /// For image type this data is DIB (Windows) or TIFF (Linux).
        /// This pointer is already locked for the duration of this event.
        /// </summary>
        /// <value>The data pointer.</value>
        public IntPtr NativeData { get; private set; }

        /// <summary>
        /// Gets the file path to the complete data if the transfer was file or memory-file.
        /// </summary>
        /// <value>
        /// The file path.
        /// </value>
        public string FileDataPath { get; private set; }

        /// <summary>
        /// Gets the raw memory data if the transfer was memory.
        /// Consumer application will need to do the parsing based on the values
        /// from <see cref="ImageInfo"/>.
        /// </summary>
        /// <value>
        /// The memory data.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public byte[] MemoryData { get; private set; }

        /// <summary>
        /// Gets the final image information if applicable.
        /// </summary>
        /// <value>
        /// The final image information.
        /// </value>
        public TWImageInfo ImageInfo { get; private set; }

        ///// <summary>
        ///// Gets the extended image information if applicable.
        ///// </summary>
        ///// <value>
        ///// The extended image information.
        ///// </value>
        //public TWExtImageInfo ExImageInfo { get; internal set; }
    }
}