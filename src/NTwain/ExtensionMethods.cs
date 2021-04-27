using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Some utility extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Checks if the returned <see cref="STS"/> contains a particular 
        /// <see cref="STS"/> value since normal TWAIN 
        /// return code is merged with condition code into <see cref="STS"/>.
        /// This is only useful for non-success values.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="checkFor">Non-success code to check for.</param>
        /// <returns></returns>
        public static bool Is(this STS status, STS checkFor)
        {
            return (status & checkFor) == checkFor;
        }
    }
}
