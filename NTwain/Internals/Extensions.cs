using NTwain.Data;
using NTwain.Properties;
using System;

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
        public static void VerifyState(this ITwainStateInternal session, int allowedMinimum, int allowedMaximum, DataGroups group, DataArgumentType dataArgumentType, Message message)
        {
            if (session.EnforceState && (session.State < allowedMinimum || session.State > allowedMaximum))
            {
                throw new TwainStateException(session.State, allowedMinimum, allowedMaximum, group, dataArgumentType, message,
                    string.Format("TWAIN state {0} does not match required range {1}-{2} for operation {3}-{4}-{5}.",
                    session.State, allowedMinimum, allowedMaximum, group, dataArgumentType, message));
            }
        }
    }
}
