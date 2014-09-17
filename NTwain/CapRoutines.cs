using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Contains re-usable routines for cap use.
    /// </summary>
    public static class CapRoutines
    {
        #region handy conversions

        /// <summary>
        /// Routine that does nothing.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static object NoConvertRoutine(object value)
        {
            return value;
        }

        /// <summary>
        /// Predefined routine for <see cref="BoolType"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TEnum EnumRoutine<TEnum>(object value) where TEnum : struct, IConvertible
        {
            if (value != null)
            {
                return value.ConvertToEnum<TEnum>();
            }
            return default(TEnum);
        }

        /// <summary>
        /// Predefined routine for <see cref="TWFix32"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TWFix32 TWFix32Routine(object value)
        {
            if (value != null)
            {
                if (value is TWFix32)
                {
                    return (TWFix32)value;
                }
                return Convert.ToSingle(value);
            }
            return default(TWFix32);
        }

        /// <summary>
        /// Predefined routine for <see cref="string"/>
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string StringRoutine(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }
            return default(string);
        }

        #endregion
    }
}
