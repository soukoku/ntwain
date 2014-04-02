using NTwain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NTwain.Values;
using System.Runtime.Serialization;

namespace NTwain.Tests
{
    [TestClass]
    public class DataTransferredEventArgsTests
    {
        [TestMethod]
        public void Constructor_Sets_Correct_Properties()
        {
            // just some non-default values to test
            IntPtr data = new IntPtr(10);
            string file = "THIS IS A TEST.";

            DataTransferredEventArgs target = new DataTransferredEventArgs(data, file);

            Assert.AreEqual(data, target.Data, "Data mismatch.");
            Assert.AreEqual(file, target.FilePath, "File mismatch.");
        }

    }
}
