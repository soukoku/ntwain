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
using System.Runtime.InteropServices;
using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
	static partial class PInvoke
	{
		static partial class NativeMethods
        {
            [DllImport("twaindsm", EntryPoint = "#1")]
            public static extern ReturnCode DsmEntry64(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref IntPtr data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWAudioInfo data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCapability data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCustomDSData data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWDeviceEvent data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCallback data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCallback2 data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWEntryPoint data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWEvent data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWFileSystem data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				IntPtr zero,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWIdentity data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPassThru data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPendingXfers data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWSetupFileXfer data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWSetupMemXfer data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWStatusUtf8 data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWUserInterface data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCieColor data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWExtImageInfo data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWGrayResponse data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageInfo data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageLayout data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageMemXfer data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWJpegCompression data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPalette8 data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWRgbResponse data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWStatus data);

			[DllImport("twaindsm", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry64(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				ref TWMemory data);

		}
	}
}
