using NTwain.Data;
using NTwain.Values;
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
        // Change pinvoke base on where running in 64bit mode.
        // Theoretically [DllImport("twaindsm", EntryPoint = "#1")] 
        // works on both 32 and 64 bit
        // but it's not installed on either system by default.
        // A proper 64 bit twain driver would've installed it so  
        // in essence it only exists in 64 bit systems and thus
        // the 2 sets of identical pinvokes for windows :(

        internal static readonly bool UseNewDSM = CheckIfCanUseNewDSM();

        private static bool CheckIfCanUseNewDSM()
        {
#if DEBUG
            return IntPtr.Size == 8;
#else
            var path = Path.Combine(Environment.SystemDirectory, "twaindsm.dll");
            // if 64bit or the dll exists use it
            return IntPtr.Size == 8 ||
                File.Exists(path);
#endif
        }

        internal static readonly bool IsWin = Environment.OSVersion.Platform == PlatformID.Win32NT;
        static readonly bool IsLinux = Environment.OSVersion.Platform == PlatformID.Unix;

        // define sig for each different data type since "object" doesn't work

        #region wrapped calls

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref IntPtr data)
        {
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, dg, dat, msg, ref data); }
                else { return NativeMethods.DsmWinOld(origin, destination, dg, dat, msg, ref data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, dg, dat, msg, ref data); }
                else { return NativeMethods.DsmWinOld(origin, destination, dg, dat, msg, ref data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
            }
            else if (IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCapability data)
        {
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Capability, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Capability, msg, data); }
            }
            else if (IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.Capability, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCustomDSData data)
        {
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
            }
            else if (IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWEntryPoint data)
        {
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
            }
            else if (IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, DataArgumentType.EntryPoint, msg, data);
            }
            throw new PlatformNotSupportedException();
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWEvent data)
        {
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
            }
            else if (IsLinux)
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
            if (IsWin)
            {
                if (UseNewDSM) { return NativeMethods.DsmWinNew(origin, destination, DataGroups.Control, dat, msg, ref data); }
                else { return NativeMethods.DsmWinOld(origin, destination, DataGroups.Control, dat, msg, ref data); }
            }
            else if (IsLinux)
            {
                return NativeMethods.DsmLinux(origin, destination, DataGroups.Control, dat, msg, ref data);
            }
            throw new PlatformNotSupportedException();
        }


        #endregion
    }
}
