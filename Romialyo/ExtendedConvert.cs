using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo
{
    public static class ExtendedConvert
    {

        public static T ChangeType<T>(object value)
        {
            if (value == null)
            {
                return default(T);
            }
            else
            {
                var conversionType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                return (T)Convert.ChangeType(value, conversionType);
            }
        }

        public static object ChangeType(object value, Type conversionType)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                var realConversionType = Nullable.GetUnderlyingType(conversionType) ?? conversionType;
                return Convert.ChangeType(value, realConversionType);
            }
        }

    }
}
