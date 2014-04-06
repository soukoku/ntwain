using System;
using System.Runtime.InteropServices;
using NTwain.Data;
using NTwain.Values;
using System.IO;

namespace NTwain.Triplets
{
    /// <summary>
    /// Contains the full TWAIN entry signatures through pinvoke.
    /// These are called by all the defined triplets methods in 
    /// this namespace.
    /// </summary>
    static partial class PInvoke
    {
        // Change pinvoke base on where running in 64bit mode.
        // Theoretically [DllImport("twaindsm", EntryPoint = "#1")] 
        // works on both 32 and 64 bit
        // but it's not installed on either system by default.
        // A proper 64 bit twain driver would've installed it so  
        // in essence it only exists in 64 bit systems and thus
        // the 2 sets of identical pinvokes :(

        public static readonly bool CanUseTwainDSM = CheckIfCanUseNewDSM();

        private static bool CheckIfCanUseNewDSM()
        {
            var path = Path.Combine(Environment.SystemDirectory, "twaindsm.dll");
            // if 64bit or the dll exists use it
            return IntPtr.Size == 8 ||
                File.Exists(path);
        }

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
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, dg, dat, msg, ref data); }
            else { return NativeMethods.DsmEntry32(origin, destination, dg, dat, msg, ref data); }
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref DataGroups data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, dg, dat, msg, ref data); }
            else { return NativeMethods.DsmEntry32(origin, destination, dg, dat, msg, ref data); }
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWAudioInfo data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCapability data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Capability, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Capability, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCustomDSData data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWDeviceEvent data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCallback data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCallback2 data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWEntryPoint data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWEvent data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWFileSystem data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            Message msg,
            TWIdentity data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWPassThru data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWPendingXfers data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWSetupFileXfer data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWSetupMemXfer data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWStatusUtf8 data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWUserInterface data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWCieColor data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWExtImageInfo data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWFilter data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWGrayResponse data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWImageInfo data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWImageLayout data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWImageMemXfer data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWJpegCompression data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWPalette8 data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWRgbResponse data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWStatus data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
        }


        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataArgumentType dat,
            Message msg,
            ref TWMemory data)
        {
            if (CanUseTwainDSM) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, dat, msg, ref data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, dat, msg, ref data); }
        }


        #endregion
    }
}
