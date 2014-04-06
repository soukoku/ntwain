using System;
using NTwain.Data;
using NTwain.Values;
using System.Collections.Generic;
using System.Linq;

namespace NTwain
{
    /// <summary>
    /// Contains event data when a data transfer is ready to be processed.
    /// </summary>
    public class TransferReadyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the current transfer should be canceled
        /// and continue next transfer if there are more data.
        /// </summary>
        /// <value><c>true</c> to cancel current transfer; otherwise, <c>false</c>.</value>
        public bool CancelCurrent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all transfers should be canceled.
        /// </summary>
        /// <value><c>true</c> to cancel all transfers; otherwise, <c>false</c>.</value>
        public bool CancelAll { get; set; }

        /// <summary>
        /// Gets a value indicating whether current transfer signifies an end of job in TWAIN world.
        /// </summary>
        /// <value><c>true</c> if transfer is end of job; otherwise, <c>false</c>.</value>
        public bool EndOfJob { get; internal set; }

        /// <summary>
        /// Gets the known pending transfer count. This may not be appilicable 
        /// for certain scanning modes.
        /// </summary>
        /// <value>The pending count.</value>
        public int PendingTransferCount { get; internal set; }

        #region image use

        /// <summary>
        /// Gets the tentative image information for the current transfer if applicable.
        /// This may differ from the final image depending on the transfer mode used.
        /// </summary>
        /// <value>
        /// The image info.
        /// </value>
        public TWImageInfo PendingImageInfo { get; internal set; }

        ///// <summary>
        ///// Gets or sets a value indicating whether file transfer is supported.
        ///// </summary>
        ///// <value>
        ///// 	<c>true</c> if this instance can do file transfer; otherwise, <c>false</c>.
        ///// </value>
        //public bool CanDoFileXfer { get; private set; }

        ///// <summary>
        ///// Gets or sets the desired output file path if file transfer is supported.
        ///// Note that not all sources will support the specified image type and compression
        ///// when file transfer is used.
        ///// </summary>
        ///// <value>
        ///// The output file.
        ///// </value>
        //public string OutputFile { get; set; }

        ///// <summary>
        ///// Gets the supported compression for image xfer.
        ///// </summary>
        ///// <value>
        ///// The supported compressions.
        ///// </value>
        //public IList<Compression> SupportedImageCompressions { get; internal set; }

        //private Compression _imageCompression;

        ///// <summary>
        ///// Gets or sets the image compression for image xfer.
        ///// </summary>
        ///// <value>
        ///// The image compression.
        ///// </value>
        ///// <exception cref="System.NotSupportedException"></exception>
        //public Compression ImageCompression
        //{
        //    get { return _imageCompression; }
        //    set
        //    {
        //        if (SupportedImageCompressions.Contains(value))
        //        {
        //            _imageCompression = value;
        //        }
        //        else
        //        {
        //            throw new NotSupportedException(string.Format("{0} is not supported.", value));
        //        }
        //    }
        //}


        ///// <summary>
        ///// Gets the supported file formats for image file xfer.
        ///// </summary>
        ///// <value>
        ///// The supported formats.
        ///// </value>
        //public IList<FileFormat> SupportedImageFormats { get; internal set; }

        //private FileFormat _imageFormat;
        ///// <summary>
        ///// Gets or sets the image format for image xfer.
        ///// </summary>
        ///// <value>
        ///// The image format.
        ///// </value>
        ///// <exception cref="System.NotSupportedException"></exception>
        //public FileFormat ImageFormat
        //{
        //    get { return _imageFormat; }
        //    set
        //    {
        //        if (SupportedImageFormats.Contains(value))
        //        {
        //            _imageFormat = value;
        //        }
        //        else
        //        {
        //            throw new NotSupportedException(string.Format("{0} is not supported.", value));
        //        }
        //    }
        //}

        #endregion

        #region audio use

        /// <summary>
        /// Gets the audio information for the current transfer if applicable.
        /// </summary>
        /// <value>
        /// The audio information.
        /// </value>
        public TWAudioInfo AudioInfo { get; internal set; }


        #endregion
    }
}
