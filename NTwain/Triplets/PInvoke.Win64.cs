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
                ref uint data);

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
                [In, Out]TWFilter data);

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
