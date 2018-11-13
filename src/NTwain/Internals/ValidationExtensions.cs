using NTwain.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Internals
{
    static class ValidationExtensions
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
                throw new ArgumentException(string.Format(MsgText.MaxStringLengthExceeded, maxLength));
        }

    }
}
