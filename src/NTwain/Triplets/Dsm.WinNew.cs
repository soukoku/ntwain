using NTwain.Data;
using System;
using System.Runtime.InteropServices;

namespace NTwain.Triplets
{
	static partial class Dsm
	{
		static partial class NativeMethods
        {
            [DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
            public static extern ReturnCode DsmWinNew(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref IntPtr data);

            [DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
            public static extern ReturnCode DsmWinNew(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref DataGroups data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWAudioInfo data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCapability data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCustomDSData data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWDeviceEvent data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCallback data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCallback2 data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWEntryPoint data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWEvent data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWFileSystem data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				IntPtr zero,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWIdentity data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPassThru data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPendingXfers data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWSetupFileXfer data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWSetupMemXfer data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWStatusUtf8 data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWUserInterface data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWCieColor data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWExtImageInfo data);

            [DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
            public static extern ReturnCode DsmWinNew(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWFilter data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWGrayResponse data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageInfo data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageLayout data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWImageMemXfer data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWJpegCompression data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWPalette8 data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWRgbResponse data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				[In, Out]TWStatus data);

			[DllImport(WIN_NEW_DSM_NAME, EntryPoint = DSM_ENTRY)]
			public static extern ReturnCode DsmWinNew(
				[In, Out]TWIdentity origin,
				[In, Out]TWIdentity destination,
				DataGroups dg,
				DataArgumentType dat,
				Message msg,
				ref TWMemory data);

		}
	}
}
