using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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
#if NET40
                typeof(Exception).GetMethod("PrepForRemoting",
                    BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(ex, new object[0]);
                throw ex;
#else
                ExceptionDispatchInfo.Capture(ex).Throw();
#endif
            }
        }

    }
}
