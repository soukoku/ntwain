using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.FileSystem"/>.
    /// </summary>
	public sealed class FileSystem : OpBase
	{
		internal FileSystem(ITwainStateInternal session) : base(session) { }
		/// <summary>
		/// This operation selects the destination directory within the Source (camera, storage, etc), where
		/// images captured using CapAutomaticCapture will be stored. This command only selects
		/// the destination directory (a file of type Directory). The directory must exist and be
		/// accessible to the Source. The creation of images within the directory is at the discretion of the
		/// Source, and may result in the creation of additional sub-directories.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode AutomaticCaptureDirectory(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.FileSystem, Message.AutomaticCaptureDirectory);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.AutomaticCaptureDirectory, fileSystem);
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
		public ReturnCode ChangeDirectory(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.FileSystem, Message.ChangeDirectory);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.ChangeDirectory, fileSystem);
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
		public ReturnCode Copy(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.FileSystem, Message.Copy);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Copy, fileSystem);
		}

		/// <summary>
		/// This operation creates a new directory within the current directory. Pathnames are not allowed,
		/// only the name of the new directory can be specified.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode CreateDirectory(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.FileSystem, Message.CreateDirectory);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.CreateDirectory, fileSystem);
		}

		/// <summary>
		/// This operation deletes a file or directory on the device. Pathnames are not allowed, only the
		/// name of the file or directory to be deleted can be specified. Recursive deletion can be specified
		/// by setting the Recursive to TRUE.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode Delete(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.FileSystem, Message.Delete);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Delete, fileSystem);
		}

		/// <summary>
		/// This operation formats the specified storage. This operation destroys all images and subdirectories
		/// under the selected device. Use with caution.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode FormatMedia(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.FileSystem, Message.FormatMedia);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.FormatMedia, fileSystem);
		}

		/// <summary>
		/// The operation frees the Context field in fileSystem.
		/// Every call to GetFirstFile must be matched by
		/// a call to GetClose to release the Context field.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetClose(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.FileSystem, Message.GetClose);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.GetClose, fileSystem);
		}

		/// <summary>
		/// This operation gets the first filename in a directory, and returns information about that file (the
		/// same information that can be retrieved with GetInfo).
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetFirstFile(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.FileSystem, Message.GetFirstFile);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.GetFirstFile, fileSystem);
		}

		/// <summary>
		/// This operation fills the information fields in fileSystem.
		/// InputName contains the absolute or relative path and filename of the requested file.
		/// OutputName returns the absolute path to the file.
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetInfo(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.FileSystem, Message.GetInfo);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.GetInfo, fileSystem);
		}

		/// <summary>
		/// This operation gets the next filename in a directory, and returns information about that file (the
		/// same information that can be retrieved with GetInfo).
		/// </summary>
		/// <param name="fileSystem">The file system.</param>
		/// <returns></returns>
		public ReturnCode GetNextFile(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.FileSystem, Message.GetNextFile);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.GetNextFile, fileSystem);
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
		public ReturnCode Rename(TWFileSystem fileSystem)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.FileSystem, Message.Rename);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Rename, fileSystem);
		}
	}
}