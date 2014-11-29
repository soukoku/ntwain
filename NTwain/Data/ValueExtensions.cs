using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NTwain.Data
{
    /// <summary>
    /// Utility on converting (possibly bad) integer values to enum values.
    /// </summary>
    public static class ValueExtensions
    {
        /// <summary>
        /// Casts a list of objects to a list of specified enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        public static IList<TEnum> CastToEnum<TEnum>(this IEnumerable<object> list) where TEnum : struct,IConvertible
        {
            return list.CastToEnum<TEnum>(true);
        }
        /// <summary>
        /// Casts a list of objects to a list of specified enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="tryUpperWord">set to <c>true</c> for working with bad values.</param>
        /// <returns></returns>
        public static IList<TEnum> CastToEnum<TEnum>(this IEnumerable<object> list, bool tryUpperWord) where TEnum : struct,IConvertible
        {
            return list.Select(o => o.ConvertToEnum<TEnum>(tryUpperWord)).ToList();
        }

        /// <summary>
        /// Casts an objects to the specified enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TEnum ConvertToEnum<TEnum>(this object value) where TEnum : struct,IConvertible
        {
            return ConvertToEnum<TEnum>(value, true);
        }
        /// <summary>
        /// Casts an objects to the specified enum.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="tryUpperWord">if set to <c>true</c> [try upper word].</param>
        /// <returns></returns>
        public static TEnum ConvertToEnum<TEnum>(this object value, bool tryUpperWord) where TEnum : struct,IConvertible
        {
            if (value != null)
            {
                var returnType = typeof(TEnum);

                // standard int values
                if (returnType.IsEnum)
                {
                    var rawType = Enum.GetUnderlyingType(returnType);
                        
                    if (tryUpperWord)
                    {
                        // small routine to work with bad sources that may put
                        // 16bit value in the upper word instead of lower word (as per the twain spec).
                        if (typeof(ushort).IsAssignableFrom(rawType))
                        {
                            var intVal = Convert.ToUInt32(value, CultureInfo.InvariantCulture);
                            var enumVal = GetLowerWord(intVal);
                            if (!Enum.IsDefined(returnType, enumVal))
                            {
                                return (TEnum)Enum.ToObject(returnType, GetUpperWord(intVal));
                            }
                        }
                    }
                    // old method:
                    // return (TEnum)Enum.ToObject(returnType, value);
                    
                    // new method:
                    // try to convert to enum's underlying type first then cast to the enum
                    return (TEnum)Convert.ChangeType(value, rawType);
                }
                else if (typeof(IConvertible).IsAssignableFrom(returnType))
                {
                    // for regular integers and whatnot
                    return (TEnum)Convert.ChangeType(value, returnType, CultureInfo.InvariantCulture);
                }
                // return as-is from cap. if caller made a mistake then there should be exceptions
                return (TEnum)value;
            }
            return default(TEnum);
        }

        static ushort GetLowerWord(uint value)
        {
            return (ushort)(value & 0xffff);
        }
        static uint GetUpperWord(uint value)
        {
            return (ushort)(value >> 16);
        }

        /// <summary>
        /// Tries to convert to a value to <see cref="TWFix32"/> if possible.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static TWFix32 ConvertToFix32(this object value)
        {
            if (value != null)
            {
                if (value is TWFix32)
                {
                    return (TWFix32)value;
                }
                return (TWFix32)Convert.ToSingle(value, CultureInfo.InvariantCulture);
            }
            return default(TWFix32);
        }


        ///// <summary>
        ///// Routine that does nothing.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public static object NoConvertRoutine(object value)
        //{
        //    return value;
        //}

        ///// <summary>
        ///// Predefined routine for <see cref="string"/>
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns></returns>
        //public static string ConvertToString(object value)
        //{
        //    if (value != null)
        //    {
        //        return value.ToString();
        //    }
        //    return default(string);
        //}
    }
}
