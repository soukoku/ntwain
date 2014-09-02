using NTwain.Data;
using System;
using System.Runtime.InteropServices;

namespace NTwain.Triplets
{
	static partial class Dsm
	{
        internal const string LINUX_DSM_PATH = "/usr/local/lib/libtwaindsm.so";

        static partial class NativeMethods
        {
            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref IntPtr data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref DataGroups data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWAudioInfo data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWCapability data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWCustomDSData data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWDeviceEvent data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWCallback data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWCallback2 data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWEntryPoint data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWEvent data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWFileSystem data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                IntPtr zero,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWIdentity data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWPassThru data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWPendingXfers data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWSetupFileXfer data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWSetupMemXfer data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWStatusUtf8 data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWUserInterface data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWCieColor data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWExtImageInfo data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWFilter data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWGrayResponse data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWImageInfo data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWImageLayout data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWImageMemXfer data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWJpegCompression data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWPalette8 data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWRgbResponse data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                [In, Out]TWStatus data);

            [DllImport(LINUX_DSM_PATH, EntryPoint = "DSM_Entry")]
            public static extern ReturnCode DsmLinux(
                [In, Out]TWIdentity origin,
                [In, Out]TWIdentity destination,
                DataGroups dg,
                DataArgumentType dat,
                Message msg,
                ref TWMemory data);
        }
	}
}
