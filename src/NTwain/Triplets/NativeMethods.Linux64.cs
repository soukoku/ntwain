using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Triplets
{
    static partial class NativeMethods
    {
        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref IntPtr data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref DataGroups data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_AUDIOINFO data);

        //[DllImport(DSM.WinDsmDll, EntryPoint = DSM.EntryName)]
        //public static extern ReturnCode DsmLinux64(
        //    [In, Out]TW_IDENTITY origin,
        //    [In, Out]TW_IDENTITY destination,
        //    DataGroups dg,
        //    DataArgumentType dat,
        //    Message msg,
        //    ref TWCapability data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_CUSTOMDSDATA data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_DEVICEEVENT data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_CALLBACK data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_CALLBACK2 data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            IntPtr zero,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            [In, Out]TW_ENTRYPOINT data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_EVENT data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_FILESYSTEM data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            IntPtr zero,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            [In, Out]TW_IDENTITY data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_PASSTHRU data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_PENDINGXFERS data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_SETUPFILEXFER data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_SETUPMEMXFER data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_STATUSUTF8 data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_USERINTERFACE data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_CIECOLOR data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_EXTIMAGEINFO data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_FILTER data);

        //[DllImport(DSM.WinDsmDll, EntryPoint = DSM.EntryName)]
        //public static extern ReturnCode DsmLinux64(
        //    [In, Out]TW_IDENTITY origin,
        //    [In, Out]TW_IDENTITY destination,
        //    DataGroups dg,
        //    DataArgumentType dat,
        //    Message msg,
        //    ref TWGrayResponse data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_IMAGEINFO data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_IMAGELAYOUT data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_IMAGEMEMXFER data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_JPEGCOMPRESSION data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_PALETTE8 data);

        //[DllImport(DSM.WinDsmDll, EntryPoint = DSM.EntryName)]
        //public static extern ReturnCode DsmLinux64(
        //    [In, Out]TW_IDENTITY origin,
        //    [In, Out]TW_IDENTITY destination,
        //    DataGroups dg,
        //    DataArgumentType dat,
        //    Message msg,
        //    ref TWRgbResponse data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_STATUS data);

        [DllImport(LinuxDll, EntryPoint = EntryName)]
        public static extern ReturnCode DsmLinux64(
            [In, Out]TW_IDENTITY origin,
            [In, Out]TW_IDENTITY destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref TW_MEMORY data);

    }
}
