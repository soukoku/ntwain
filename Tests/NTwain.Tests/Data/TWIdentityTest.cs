using Microsoft.VisualStudio.TestTools.UnitTesting;
using NTwain.Data;
using System;

namespace NTwain.Tests.Data
{
    /// <summary>
    ///This is a test class for TWIdentity and is intended
    ///to contain all TWIdentityTest Unit Tests
    ///</summary>
    [TestClass]
    public class TWIdentityTest
    {
        // the maxlength expects null terminator so at maxlength it's over the limit

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "String length not enforced.")]
        public void Enforce_Manufacturer_String_Length()
        {
            TWIdentity target = new TWIdentity();

            var overLength = TwainConst.String32;

            string badString = new String('a', overLength);

            target.Manufacturer = badString;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "String length not enforced.")]
        public void Enforce_ProductFamily_String_Length()
        {
            TWIdentity target = new TWIdentity();

            var overLength = TwainConst.String32;

            string badString = new String('a', overLength);
            target.ProductFamily = badString;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "String length not enforced.")]
        public void Enforce_ProductName_String_Length()
        {
            TWIdentity target = new TWIdentity();

            var overLength = TwainConst.String32;

            string badString = new String('a', overLength);
            target.ProductName = badString;
        }

        [TestMethod]
        public void Setting_and_Unsetting_the_Shared_Flag_Properties_Works()
        {
            TWIdentity id = new TWIdentity();

            // these 2 flag properties are store in same int value so needs to test together

            id.DataFunctionalities = DataFunctionalities.App2;
            id.DataGroup = DataGroups.Audio;
            Assert.AreEqual(DataFunctionalities.App2, id.DataFunctionalities);
            Assert.AreEqual(DataGroups.Audio, id.DataGroup);

            // clear needs to be tested 
            id.DataFunctionalities = DataFunctionalities.None;
            Assert.AreEqual(DataFunctionalities.None, id.DataFunctionalities);
            Assert.AreEqual(DataGroups.Audio, id.DataGroup, "Cleared incorrectly.");

            id.DataGroup = DataGroups.None;
            Assert.AreEqual(DataGroups.None, id.DataGroup);
        }
        
    }
}
