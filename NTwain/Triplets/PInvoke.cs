using System;
using System.Runtime.InteropServices;
using NTwain.Data;
using NTwain.Values;

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
		// There's another way of doing own pinvoke at runtime but
		// that's not easy to understand.
		public static readonly bool Is64Bit = IntPtr.Size == 8;

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
            if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, dg, dat, msg, ref data); }
            else { return NativeMethods.DsmEntry32(origin, destination, dg, dat, msg, ref data); }
        }

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            DataGroups dg,
            DataArgumentType dat,
            Message msg,
            ref uint data)
        {
            if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, dg, dat, msg, ref data); }
            else { return NativeMethods.DsmEntry32(origin, destination, dg, dat, msg, ref data); }
        }

		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWAudioInfo data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Audio, DataArgumentType.AudioInfo, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWCapability data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Capability, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Capability, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWCustomDSData data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.CustomDSData, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWDeviceEvent data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.DeviceEvent, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWCallback data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWCallback2 data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Callback, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWEntryPoint data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.EntryPoint, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWEvent data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Event, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWFileSystem data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.FileSystem, msg, data); }
		}

		public static ReturnCode DsmEntry(
			TWIdentity origin,
			Message msg,
			TWIdentity data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, IntPtr.Zero, DataGroups.Control, DataArgumentType.Identity, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWPassThru data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.PassThru, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWPendingXfers data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.PendingXfers, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWSetupFileXfer data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.SetupFileXfer, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWSetupMemXfer data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.SetupMemXfer, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWStatusUtf8 data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.StatusUtf8, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWUserInterface data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.UserInterface, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWCieColor data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.CieColor, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWExtImageInfo data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ExtImageInfo, msg, data); }
		}

        public static ReturnCode DsmEntry(
            TWIdentity origin,
            TWIdentity destination,
            Message msg,
            TWFilter data)
        {
            if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
            else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.Filter, msg, data); }
        }

		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWGrayResponse data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.GrayResponse, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWImageInfo data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ImageInfo, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWImageLayout data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ImageLayout, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWImageMemXfer data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.ImageMemXfer, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWJpegCompression data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.JpegCompression, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWPalette8 data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.Palette8, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWRgbResponse data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Image, DataArgumentType.RgbResponse, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			Message msg,
			TWStatus data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, DataArgumentType.Status, msg, data); }
		}


		public static ReturnCode DsmEntry(
			TWIdentity origin,
			TWIdentity destination,
			DataArgumentType dat,
			Message msg,
			ref TWMemory data)
		{
			if (Is64Bit) { return NativeMethods.DsmEntry64(origin, destination, DataGroups.Control, dat, msg, ref data); }
			else { return NativeMethods.DsmEntry32(origin, destination, DataGroups.Control, dat, msg, ref data); }
		}


		#endregion
	}
}
