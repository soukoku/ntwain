using System;
using System.Runtime.InteropServices;
using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
	static partial class PInvoke
	{
        // not sure if the signatures are correct yet
        // so just a placeholder for now

        //static partial class NativeMethods
        //{
        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        ref IntPtr data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        ref DataGroups data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWAudioInfo data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWCapability data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWCustomDSData data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWDeviceEvent data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWCallback data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWCallback2 data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWEntryPoint data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWEvent data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWFileSystem data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        IntPtr zero,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWIdentity data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWPassThru data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWPendingXfers data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWSetupFileXfer data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWSetupMemXfer data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWStatusUtf8 data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWUserInterface data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWCieColor data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWExtImageInfo data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWFilter data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWGrayResponse data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWImageInfo data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWImageLayout data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWImageMemXfer data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWJpegCompression data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWPalette8 data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWRgbResponse data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        [In, Out]TWStatus data);

        //    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "#1")]
        //    public static extern ReturnCode DsmEntryLinux(
        //        [In, Out]TWIdentity origin,
        //        [In, Out]TWIdentity destination,
        //        DataGroups dg,
        //        DataArgumentType dat,
        //        Message msg,
        //        ref TWMemory data);
        //}
	}
}
