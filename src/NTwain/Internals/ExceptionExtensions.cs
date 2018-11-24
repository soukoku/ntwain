using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Internals
{
    static class ExceptionExtensions
    {
        /// <summary>
        /// Rethrows the specified excetion while keeping stack trace
        /// if not null.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public static void TryRethrow(this Exception ex)
        {
            if (ex != null)
            {
                typeof(Exception).GetMethod("PrepForRemoting",
                    BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(ex, new object[0]);
                throw ex;
            }
        }

    }
}
