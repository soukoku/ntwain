using NTwain.Data;
using NTwain.Properties;
using System;
using System.Globalization;
using System.IO;

namespace NTwain.Internals
{
    static class Extensions
    {
        /// <summary>
        /// Verifies the string length is under the maximum length
        /// and throws <see cref="ArgumentException"/> if violated.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        public static void VerifyLengthUnder(this string value, int maxLength)
        {
            if (value != null && value.Length > maxLength)
                throw new ArgumentException(Resources.MaxStringLengthExceeded);
        }


        /// <summary>
        /// Verifies the session is within the specified state range (inclusive). Throws
        /// <see cref="TwainStateException" /> if violated.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="allowedMinimum">The allowed minimum.</param>
        /// <param name="allowedMaximum">The allowed maximum.</param>
        /// <param name="group">The triplet data group.</param>
        /// <param name="dataArgumentType">The triplet data argument type.</param>
        /// <param name="message">The triplet message.</param>
        /// <exception cref="TwainStateException"></exception>
        public static void VerifyState(this ITwainSessionInternal session, int allowedMinimum, int allowedMaximum, DataGroups group, DataArgumentType dataArgumentType, Message message)
        {
            if (session.EnforceState && (session.State < allowedMinimum || session.State > allowedMaximum))
            {
                throw new TwainStateException(session.State, allowedMinimum, allowedMaximum, group, dataArgumentType, message,
                    string.Format(CultureInfo.InvariantCulture, "TWAIN state {0} does not match required range {1}-{2} for operation {3}-{4}-{5}.",
                    session.State, allowedMinimum, allowedMaximum, group, dataArgumentType, message));
            }
        }


        public static string ChangeExtensionByFormat(this TWSetupFileXfer fileInfo, string currentFilePath)
        {
            string finalFile = null;
            switch (fileInfo.Format)
            {
                case FileFormat.Bmp:
                    finalFile = Path.ChangeExtension(currentFilePath, ".bmp");
                    break;
                case FileFormat.Dejavu:
                    finalFile = Path.ChangeExtension(currentFilePath, ".dejavu");
                    break;
                case FileFormat.Exif:
                    finalFile = Path.ChangeExtension(currentFilePath, ".exit");
                    break;
                case FileFormat.Fpx:
                    finalFile = Path.ChangeExtension(currentFilePath, ".fpx");
                    break;
                case FileFormat.Jfif:
                    finalFile = Path.ChangeExtension(currentFilePath, ".jpg");
                    break;
                case FileFormat.Jp2:
                    finalFile = Path.ChangeExtension(currentFilePath, ".jp2");
                    break;
                case FileFormat.Jpx:
                    finalFile = Path.ChangeExtension(currentFilePath, ".jpx");
                    break;
                case FileFormat.Pdf:
                case FileFormat.PdfA:
                case FileFormat.PdfA2:
                    finalFile = Path.ChangeExtension(currentFilePath, ".pdf");
                    break;
                case FileFormat.Pict:
                    finalFile = Path.ChangeExtension(currentFilePath, ".pict");
                    break;
                case FileFormat.Png:
                    finalFile = Path.ChangeExtension(currentFilePath, ".png");
                    break;
                case FileFormat.Spiff:
                    finalFile = Path.ChangeExtension(currentFilePath, ".spiff");
                    break;
                case FileFormat.Tiff:
                case FileFormat.TiffMulti:
                    finalFile = Path.ChangeExtension(currentFilePath, ".tif");
                    break;
                case FileFormat.Xbm:
                    finalFile = Path.ChangeExtension(currentFilePath, ".xbm");
                    break;
                default:
                    finalFile = Path.ChangeExtension(currentFilePath, ".unknown");
                    break;
            }
            return finalFile;
        }

    }
}
