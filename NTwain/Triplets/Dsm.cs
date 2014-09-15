using NTwain.Data;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Contains the full TWAIN entry signatures through pinvoke.
    /// These are called by all the defined triplets methods in 
    /// this namespace.
    /// </summary>
    static partial class Dsm
    {
        internal const string DSM_ENTRY = "DSM_Entry";
        internal const string LINUX_DSM_PATH = "/usr/local/lib/libtwaindsm.so";
        internal const string WIN_NEW_DSM_NAME = "twaindsm.dll";
        internal const string WIN_OLD_DSM_NAME = "twain_32.dll";

        #region wrapped calls

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref IntPtr data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, dg, dat, msg, ref data); }
                else { return NativeMethods.DsmWinOld(origin, destination, dg, dat, msg, ref data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, dg, dat, msg, ref data);
            }
            throw new PlatformNotSupportedException();
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref DataGroups data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, dg, dat, msg, ref data); }
                else { return NativeMethods.DsmWinOld(origin, destination, dg, dat, msg, ref data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, dg, dat, msg, ref data);
            }
            throw new PlatformNotSupportedException();
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWAudioInfo data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataArgumentType dat,
            Message msg,
            TWCapability data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, dat, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, dat, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, dat, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCustomDSData data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWDeviceEvent data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCallback data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCallback2 data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            Message msg,
            TWEntryPoint data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, null, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, null, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, null, DataGroups.Control, DataArgumentType.EntryPoint, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWEvent data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWFileSystem data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data);
            }
            throw new PlatformNotSupportedException();
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            Message msg,
            TWIdentity data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWPassThru data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWPendingXfers data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWSetupFileXfer data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWSetupMemXfer data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWStatusUtf8 data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWUserInterface data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCieColor data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWExtImageInfo data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data);
            }
            throw new PlatformNotSupportedException();
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWFilter data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data);
            }
            throw new PlatformNotSupportedException();
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWGrayResponse data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWImageInfo data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWImageLayout data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWImageMemXfer data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWJpegCompression data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWPalette8 data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWRgbResponse data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWStatus data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataArgumentType dat,
            Message msg,
            ref TWMemory data)
        {
            if (PlatformInfo.__global.IsWindows)
            {
                if (PlatformInfo.__global.UseNewWinDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, dat, msg, ref data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, dat, msg, ref data); }
            }
            else if (PlatformInfo.__global.IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, dat, msg, ref data);
            }
            throw new PlatformNotSupportedException();
        }


        #endregion
    }
}
