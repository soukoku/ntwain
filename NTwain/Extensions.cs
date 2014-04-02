using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Media.Imaging;
using NTwain.Properties;
using NTwain.Values;
using Microsoft.Win32.SafeHandles;

namespace NTwain
{
	/// <summary>
	/// Some utility stuff.
	/// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Verifies the string length is under the maximum length
        /// and throws <see cref="ArgumentException"/> if violated.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="maxLength">The maximum length.</param>
        internal static void VerifyLengthUnder(this string value, int maxLength)
        {
            if (value != null && value.Length > maxLength)
                throw new ArgumentException(Resources.MaxStringLengthExceeded);
        }

    }
}
