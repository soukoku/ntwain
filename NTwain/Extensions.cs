using NTwain.Properties;
using System;

namespace NTwain
{
    static class Extensions
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
