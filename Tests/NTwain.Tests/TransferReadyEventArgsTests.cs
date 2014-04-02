using NTwain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NTwain.Values;
using System.Runtime.Serialization;
using NTwain.Data;
using NTwain.Values.Cap;
using System.Collections.Generic;

namespace NTwain.Tests
{
    [TestClass]
    public class TransferReadyEventArgsTests
    {
        [TestMethod]
        public void Constructor_Sets_Correct_Properties()
        {
            // just some non-default values to test
            TWPendingXfers pending = new TWPendingXfers();
            List<ImageFileFormat> formats = new List<ImageFileFormat> { ImageFileFormat.Bmp, ImageFileFormat.Tiff };
            ImageFileFormat curFormat = ImageFileFormat.Tiff;
            List<Compression> compressions = new List<Compression> { Compression.None, Compression.Group4 };
            Compression curCompress = Compression.None;
            bool fileXfer = true;
            TWImageInfo info = new TWImageInfo();

            TransferReadyEventArgs target = new TransferReadyEventArgs(pending, formats, curFormat, compressions, curCompress, fileXfer, info);

            Assert.AreEqual(pending.Count, target.PendingCount, "PendingCount mismatch.");
            Assert.AreEqual(formats, target.SupportedFormats, "SupportedFormats mismatch.");
            Assert.AreEqual(curFormat, target.ImageFormat, "ImageFormat mismatch.");
            Assert.AreEqual(compressions, target.SupportedCompressions, "SupportedCompressions mismatch.");
            Assert.AreEqual(curCompress, target.ImageCompression, "ImageCompression mismatch.");
            Assert.AreEqual(fileXfer, target.CanDoFileXfer, "CanDoFileXfer mismatch.");
            Assert.AreEqual(info, target.ImageInfo, "ImageInfo mismatch.");
        }

    }
}
