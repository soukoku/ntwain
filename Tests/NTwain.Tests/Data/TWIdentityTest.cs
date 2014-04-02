using NTwain.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NTwain.Values;

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
    }
}
