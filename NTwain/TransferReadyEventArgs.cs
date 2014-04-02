// The MIT License (MIT)
// Copyright (c) 2013 Yin-Chun Wang
//
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF 
// OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using NTwain.Data;
using NTwain.Values;
using NTwain.Values.Cap;
using System.Collections.Generic;

namespace NTwain
{
    /// <summary>
    /// Contains event data when a data transfer is ready to be processed.
    /// </summary>
    public class TransferReadyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReadyEventArgs" /> class.
        /// </summary>
        /// <param name="pending">The pending data.</param>
        /// <param name="supportedFormats">The formats.</param>
        /// <param name="currentFormat">The current format.</param>
        /// <param name="supportedCompressions">The compressions.</param>
        /// <param name="currentCompression">The current compression.</param>
        /// <param name="canDoFileXfer">if set to <c>true</c> then allow file xfer properties.</param>
        /// <param name="imageInfo">The image info.</param>
        internal TransferReadyEventArgs(TWPendingXfers pending, IList<ImageFileFormat> supportedFormats, ImageFileFormat currentFormat,
            IList<Compression> supportedCompressions, Compression currentCompression, bool canDoFileXfer, TWImageInfo imageInfo)
        {
            PendingCount = pending.Count;
            EndOfJob = pending.EndOfJob;
            _imageCompression = currentCompression;
            SupportedCompressions = supportedCompressions;
            _imageFormat = currentFormat;
            SupportedFormats = supportedFormats;
            CanDoFileXfer = canDoFileXfer;
            ImageInfo = imageInfo;
        }

        /// <summary>
        /// Gets the image info for the current transfer.
        /// </summary>
        /// <value>
        /// The image info.
        /// </value>
        public TWImageInfo ImageInfo { get; private set; }

        /// <summary>
        /// Gets the known pending transfer count. This may not be appilicable 
        /// for certain scanning modes.
        /// </summary>
        /// <value>The pending count.</value>
        public int PendingCount { get; private set; }

        /// <summary>
        /// Gets a value indicating whether current transfer signifies an end of job.
        /// </summary>
        /// <value><c>true</c> if transfer is end of job; otherwise, <c>false</c>.</value>
        public bool EndOfJob { get; private set; }

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
        /// Gets or sets a value indicating whether file transfer is supported.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can do file transfer; otherwise, <c>false</c>.
        /// </value>
        public bool CanDoFileXfer { get; private set; }

        /// <summary>
        /// Gets or sets the desired output file path if file transfer is supported.
        /// Note that not all sources will support the specified image type and compression
        /// when file transfer is used.
        /// </summary>
        /// <value>
        /// The output file.
        /// </value>
        public string OutputFile { get; set; }

        /// <summary>
        /// Gets the supported compression for image xfer.
        /// </summary>
        /// <value>
        /// The supported compressions.
        /// </value>
        public IList<Compression> SupportedCompressions { get; private set; }

        private Compression _imageCompression;

        /// <summary>
        /// Gets or sets the image compression for image xfer.
        /// </summary>
        /// <value>
        /// The image compression.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        public Compression ImageCompression
        {
            get { return _imageCompression; }
            set
            {
                if (SupportedCompressions.Contains(value))
                {
                    _imageCompression = value;
                }
                else
                {
                    throw new NotSupportedException(string.Format("{0} is not supported.", value));
                }
            }
        }


        /// <summary>
        /// Gets the supported file formats for image file xfer.
        /// </summary>
        /// <value>
        /// The supported formats.
        /// </value>
        public IList<ImageFileFormat> SupportedFormats { get; private set; }

        private ImageFileFormat _imageFormat;
        /// <summary>
        /// Gets or sets the image format for image xfer.
        /// </summary>
        /// <value>
        /// The image format.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        public ImageFileFormat ImageFormat
        {
            get { return _imageFormat; }
            set
            {
                if (SupportedFormats.Contains(value))
                {
                    _imageFormat = value;
                }
                else
                {
                    throw new NotSupportedException(string.Format("{0} is not supported.", value));
                }
            }
        }


        ///// <summary>
        ///// Gets or sets the audio file format if <see cref="OutputFile"/> is specified
        ///// and the data to be transferred is audio.
        ///// </summary>
        ///// <value>
        ///// The audio file format.
        ///// </value>
        //public AudioFileFormat AudioFileFormat { get; set; }
    }
}
