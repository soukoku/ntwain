using System;

namespace NTwain
{
	/// <summary>
	/// Contains event data after whatever data from the source has been transferred.
	/// </summary>
	public class DataTransferredEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DataTransferredEventArgs" /> class.
		/// </summary>
		/// <param name="data">The data pointer.</param>
		/// <param name="filePath">The file.</param>
		internal DataTransferredEventArgs(IntPtr data, string filePath)
		{
			Data = data;
			FilePath = filePath;
		}
		/// <summary>
		/// Gets pointer to the image data if applicable.
		/// The data will be freed once the event handler ends
		/// so consumers must complete whatever processing
		/// required by then.
		/// </summary>
		/// <value>The data pointer.</value>
		public IntPtr Data { get; private set; }

		/// <summary>
		/// Gets the filepath to the transferrerd data if applicable.
		/// </summary>
		/// <value>
		/// The file.
		/// </value>
		public string FilePath { get; private set; }
	}
}