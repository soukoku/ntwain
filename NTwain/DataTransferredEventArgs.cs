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
		/// Gets pointer to the complete data if the transfer was native.
		/// The data will be freed once the event handler ends
		/// so consumers must complete whatever processing before then.
        /// For image type this data is DIB (Windows), PICT (old Mac), and TIFF (Linux/OSX).
		/// </summary>
		/// <value>The data pointer.</value>
		public IntPtr NativeData { get; internal set; }

		/// <summary>
		/// Gets the file path to the complete data if the transfer was file or memory-file.
		/// </summary>
		/// <value>
		/// The file.
		/// </value>
        public string FilePath { get; internal set; }

        /// <summary>
        /// Gets the memory data if the transfer was memory.
        /// </summary>
        /// <value>
        /// The memory data.
        /// </value>
        public byte[] MemData { get; internal set; }

        /// <summary>
        /// Gets the final image information if applicable.
        /// </summary>
        /// <value>
        /// The final image information.
        /// </value>
        public TWImageInfo FinalImageInfo { get; internal set; }
	}
}