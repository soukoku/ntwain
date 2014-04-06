using System;
using System.Runtime.InteropServices;
using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
	static partial class PInvoke
	{
		static partial class WinNativeMethods
        {
            [DllImport("twain_32", EntryPoint = "#1")]
            public static extern ReturnCode DsmEntry32(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref IntPtr data);

            [DllImport("twain_32", EntryPoint = "#1")]
            public static extern ReturnCode DsmEntry32(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref uint data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWAudioInfo data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCapability data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCustomDSData data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWDeviceEvent data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCallback data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCallback2 data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWEntryPoint data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWEvent data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWFileSystem data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				IntPtr zero,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWIdentity data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPassThru data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPendingXfers data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWSetupFileXfer data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWSetupMemXfer data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWStatusUtf8 data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWUserInterface data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCieColor data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
                [In, Out]TWExtImageInfo data);

            [DllImport("twain_32", EntryPoint = "#1")]
            public static extern ReturnCode DsmEntry32(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWFilter data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWGrayResponse data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageInfo data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageLayout data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageMemXfer data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWJpegCompression data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPalette8 data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWRgbResponse data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWStatus data);

			[DllImport("twain_32", EntryPoint = "#1")]
			public static extern ReturnCode DsmEntry32(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				ref TWMemory data);
		}
	}
}
