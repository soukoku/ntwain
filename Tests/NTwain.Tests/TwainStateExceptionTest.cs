using NTwain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NTwain.Values;
using System.Runtime.Serialization;

namespace NTwain.Tests
{
	/// <summary>
	///This is a test class for TwainStateException and is intended
	///to contain all TwainStateExceptionTest Unit Tests
	///</summary>
	[TestClass]
	public class TwainStateExceptionTest
	{
		/// <summary>
		///A test for TwainStateException Constructor
		///</summary>
		[TestMethod]
		public void Constructor_Sets_Correct_Properties()
		{
            // just some non-default values to test
			int state = 3;
            int minState = 4;
            int maxState = 5;
            DataGroups dataGroup = DataGroups.Control;
            DataArgumentType argumentType = DataArgumentType.AudioNativeXfer;
            Message twainMessage = Message.Copy;
            string message = "THIS IS A TEST.";

			TwainStateException target = new TwainStateException(state, minState, maxState, dataGroup, argumentType, twainMessage, message);

            Assert.AreEqual(state, target.ActualState, "State mismatch.");
            Assert.AreEqual(minState, target.MinStateExpected, "Minimum mismatch.");
            Assert.AreEqual(maxState, target.MaxStateExpected, "Maximum mismatch.");
            Assert.AreEqual(dataGroup, target.DataGroup, "DataGroup mismatch.");
            Assert.AreEqual(argumentType, target.ArgumentType, "ArgumentType mismatch.");
            Assert.AreEqual(twainMessage, target.TwainMessage, "TwainMessage mismatch.");
            Assert.AreEqual(message, target.Message, "Message mismatch.");
		}

    }
}
