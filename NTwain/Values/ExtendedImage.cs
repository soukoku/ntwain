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

namespace NTwain.Values
{
	/// <summary>
	/// Contains various values for extended image codes.
	/// </summary>
	public static class ExtendedImage
	{
		public enum Codes
		{
			Invalid = 0,
			BarcodeX = 0x1200,
			BarcodeY = 0x1201,
			BarcodeText = 0x1202,
			BarcodeType = 0x1203,
			DeshadeTop = 0x1204,
			DeshadeLeft = 0x1205,
			DeshadeHeight = 0x1206,
			DeshadeWidth = 0x1207,
			DeshadeSize = 0x1208,
			SpecklesRemoved = 0x1209,
			HorzLineXCoord = 0x120A,
			HorzLineYCoord = 0x120B,
			HorzLineLength = 0x120C,
			HorzLineThickness = 0x120D,
			VertLineXCoord = 0x120E,
			VertLineYCoord = 0x120F,
			VertLineLength = 0x1210,
			VertLineThickness = 0x1211,
			PatchCode = 0x1212,
			EndorsedText = 0x1213,
			FormConfidence = 0x1214,
			FormTemplateMatch = 0x1215,
			FormTemplatePageMatch = 0x1216,
			FormHorzDocOffset = 0x1217,
			FormVertDocOffset = 0x1218,
			BarcodeCount = 0x1219,
			BarcodeConfidence = 0x121A,
			BarcodeRotation = 0x121B,
			BarcodeTextLength = 0x121C,
			DeshadeCount = 0x121D,
			DeshadeBlackCountOld = 0x121E,
			DeshadeBlackCountNew = 0x121F,
			DeshadeBlackRLMin = 0x1220,
			DeshadeBlackRLMax = 0x1221,
			DeshadeWhiteCountOld = 0x1222,
			DeshadeWhiteCountNew = 0x1223,
			DeshadeWhiteRLMin = 0x1224,
			DeshadeWhiteRLAve = 0x1225,
			DeshadeWhiteRLMax = 0x1226,
			BlackSpecklesRemoved = 0x1227,
			WhiteSpecklesRemoved = 0x1228,
			HorzLineCount = 0x1229,
			VertLineCount = 0x122A,
			DeskewStatus = 0x122B,
			SkewOriginalAngle = 0x122C,
			SkewFinalAngle = 0x122D,
			SkewConfidence = 0x122E,
			SkewWindowX1 = 0x122F,
			SkewWindowY1 = 0x1230,
			SkewWindowX2 = 0x1231,
			SkewWindowY2 = 0x1232,
			SkewWindowX3 = 0x1233,
			SkewWindowY3 = 0x1234,
			SkewWindowX4 = 0x1235,
			SkewWindowY4 = 0x1236,
			BookName = 0x1238, /* added 1.9 */
			ChapterNumber = 0x1239,/* added 1.9 */
			DocumentNumber = 0x123A,  /* added 1.9 */
			PageNumber = 0x123B,  /* added 1.9 */
			Camera = 0x123C,  /* added 1.9 */
			FrameNumber = 0x123D,  /* added 1.9 */
			Frame = 0x123E,  /* added 1.9 */
			PixelFlavor = 0x123F,  /* added 1.9 */
			IccProfile = 0x1240,  /* added 1.91 */
			LastSegment = 0x1241,  /* added 1.91 */
			SegmentNumber = 0x1242,  /* added 1.91 */
			MagData = 0x1243,  /* added 2.0 */
			MagType = 0x1244,  /* added 2.0 */
			PageSide = 0x1245,  /* added 2.0 */
			FileSystemSource = 0x1246,  /* added 2.0 */
			ImageMerged = 0x1247,  /* added 2.1 */
			MagDataLength = 0x1248,  /* added 2.1 */

		}

		/// <summary>
		/// The bar code’s orientation on the scanned image is described in
		/// reference to a Western-style interpretation of the image.
		/// </summary>
		public enum BarcodeRotation : uint
		{
			/// <summary>
			/// Normal reading orientation.
			/// </summary>
			Rot0 = 0,
			/// <summary>
			/// Rotated 90 degrees clockwise.
			/// </summary>
			Rot90 = 1,
			/// <summary>
			/// Rotated 180 degrees clockwise.
			/// </summary>
			Rot180 = 2,
			/// <summary>
			/// Rotated 270 degrees clockwise.
			/// </summary>
			Rot270 = 3,
			/// <summary>
			/// The orientation is not known.
			/// </summary>
			RotX = 4
		}


		public enum DeskewStatus
		{
			Success = 0,
			ReportOnly = 1,
			Fail = 2,
			Disabled = 3
		}


		public enum MagType
		{
			Micr = 0,
			Raw = 1,
			Invalid = 2
		}
	}
}
