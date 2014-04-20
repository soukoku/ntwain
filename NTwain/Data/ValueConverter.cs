using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace NTwain.Data
{
    /// <summary>
    /// Utility on converting (possibly bad) integer values to enum values.
    /// </summary>
    public static class ValueConverter
    {
        public static IList<T> CastToEnum<T>(this IEnumerable<object> list) where T : struct,IConvertible
        {
            return list.CastToEnum<T>(true);
        }
        public static IList<T> CastToEnum<T>(this IEnumerable<object> list, bool tryUpperWord) where T : struct,IConvertible
        {
            return list.Select(o => o.ConvertToEnum<T>(tryUpperWord)).ToList();
        }

        public static T ConvertToEnum<T>(this object value) where T : struct,IConvertible
        {
            return ConvertToEnum<T>(value, true);
        }
        public static T ConvertToEnum<T>(this object value, bool tryUpperWord) where T : struct,IConvertible
        {
            var returnType = typeof(T);

            // standard int values
            if (returnType.IsEnum)
            {
                if (tryUpperWord)
                {
                    // small routine to work with bad sources that may put
                    // 16bit value in the upper word instead of lower word (as per the twain spec).
                    var rawType = Enum.GetUnderlyingType(returnType);
                    if (typeof(ushort).IsAssignableFrom(rawType))
                    {
                        var intVal = Convert.ToUInt32(value, CultureInfo.InvariantCulture);
                        var enumVal = GetLowerWord(intVal);
                        if (!Enum.IsDefined(returnType, enumVal))
                        {
                            return (T)Enum.ToObject(returnType, GetUpperWord(intVal));
                        }
                    }
                }
                // this may work better?
                return (T)Enum.ToObject(returnType, value);
                //// cast to underlying type first then to the enum
                //return (T)Convert.ChangeType(value, rawType);
            }
            else if (typeof(IConvertible).IsAssignableFrom(returnType))
            {
                // for regular integers and whatnot
                return (T)Convert.ChangeType(value, returnType, CultureInfo.InvariantCulture);
            }
            // return as-is from cap. if caller made a mistake then there should be exceptions
            return (T)value;
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
            if (value is TWFix32)
            {
                return (TWFix32)value;
            }
            return (TWFix32)Convert.ToSingle(value, CultureInfo.InvariantCulture);
        }
    }
}
