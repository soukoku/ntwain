using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.FileSystem"/>.
    /// </summary>
	public sealed class FileSystem : BaseTriplet
	{
		internal FileSystem(TwainSession session) : base(session) { }

        ReturnCode DoIt(Message msg, ref TW_FILESYSTEM fileSystem)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.FileSystem, msg, ref fileSystem);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.FileSystem, msg, ref fileSystem);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.FileSystem, msg, ref fileSystem);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.FileSystem, msg, ref fileSystem);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.FileSystem, msg, ref fileSystem);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.FileSystem, msg, ref fileSystem);

            return ReturnCode.Failure;
        }

        /// <summary>
        /// This operation selects the destination directory within the Source (camera, storage, etc), where
        /// images captured using CapAutomaticCapture will be stored. This command only selects
        /// the destination directory (a file of type Directory). The directory must exist and be
        /// accessible to the Source. The creation of images within the directory is at the discretion of the
        /// Source, and may result in the creation of additional sub-directories.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <returns></returns>
        public ReturnCode AutomaticCaptureDirectory(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.AutomaticCaptureDirectory, ref fileSystem);
		}

		/// <summary>
		/// This operation selects the current device within the Source (camera, storage, etc). If the device is
		/// a <see cref="FileType.Domain"/>, then this command enters a directory that can contain <see cref="FileType.Host"/> files. If the
		/// device is a <see cref="FileType.Host"/>, then this command enters a directory that can contain
		/// <see cref="FileType.Directory"/> files. If the device is a <see cref="FileType.Directory"/>, then this command enters a
		/// directory that can contain <see cref="FileType.Directory"/> or <see cref="FileType.Image"/> files.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode ChangeDirectory(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.ChangeDirectory, ref fileSystem);
        }

		/// <summary>
		/// This operation copies a file or directory. Absolute and relative pathnames are supported. A file
		/// may not be overwritten with this command. If an Application wishes to do this, it must first
		/// delete the unwanted file and then reissue the Copy command.
		/// The Application specifies the path and name of the entry to be copied in InputName. The
		/// Application specifies the new patch and name in OutputName.
		/// It is not permitted to copy files into the root directory.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode Copy(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.Copy, ref fileSystem);
        }

		/// <summary>
		/// This operation creates a new directory within the current directory. Pathnames are not allowed,
		/// only the name of the new directory can be specified.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode CreateDirectory(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.CreateDirectory, ref fileSystem);
		}

		/// <summary>
		/// This operation deletes a file or directory on the device. Pathnames are not allowed, only the
		/// name of the file or directory to be deleted can be specified. Recursive deletion can be specified
		/// by setting the Recursive to TRUE.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode Delete(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.Delete, ref fileSystem);
        }

		/// <summary>
		/// This operation formats the specified storage. This operation destroys all images and subdirectories
		/// under the selected device. Use with caution.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode FormatMedia(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.FormatMedia, ref fileSystem);
        }

		/// <summary>
		/// The operation frees the Context field in fileSystem.
		/// Every call to GetFirstFile must be matched by
		/// a call to GetClose to release the Context field.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetClose(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.GetClose, ref fileSystem);
        }

		/// <summary>
		/// This operation gets the first filename in a directory, and returns information about that file (the
		/// same information that can be retrieved with GetInfo).
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetFirstFile(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.GetFirstFile, ref fileSystem);
        }

		/// <summary>
		/// This operation fills the information fields in fileSystem.
		/// InputName contains the absolute or relative path and filename of the requested file.
		/// OutputName returns the absolute path to the file.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetInfo(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.GetInfo, ref fileSystem);
        }

		/// <summary>
		/// This operation gets the next filename in a directory, and returns information about that file (the
		/// same information that can be retrieved with GetInfo).
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetNextFile(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.GetNextFile, ref fileSystem);
        }

		/// <summary>
		/// This operation renames (and optionally moves) a file or directory. Absolute and relative path
		/// names are supported. A file may not be overwritten with this command. If an Application
		/// wishes to do this it must first delete the unwanted file, then issue the rename command.
		/// The Application specifies the path and name of the entry to be renamed in InputName. The
		/// Application specifies the new path and name in OutputName.
		/// Filenames in the root directory cannot be moved or renamed.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode Rename(ref TW_FILESYSTEM fileSystem)
        {
            return DoIt(Message.Rename, ref fileSystem);
		}
	}
}