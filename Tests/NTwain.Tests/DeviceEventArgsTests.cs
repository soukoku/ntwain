using NTwain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NTwain.Values;
using System.Runtime.Serialization;
using NTwain.Data;

namespace NTwain.Tests
{
    [TestClass]
    public class DeviceEventArgsTests
    {
        [TestMethod]
        public void Constructor_Sets_Correct_Properties()
        {
            // just some non-default values to test
            TWDeviceEvent evt = new TWDeviceEvent();

            DeviceEventArgs target = new DeviceEventArgs(evt);

            Assert.AreEqual(evt, target.DeviceEvent, "DeviceEvent mismatch.");
        }

    }
}
