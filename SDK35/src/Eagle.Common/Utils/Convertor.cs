using Eagle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Eagle.Common.Util
{
    public static class Convertor
    {
        public static T ChangeType<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static int? ConvertToInteger(object value) 
        {
            if (value == null || 
                value == DBNull.Value)
            {
                return null;
            }

            try
            {
                int intValue = Convert.ToInt32(value);

                return intValue;
            }
            catch
            {
                return null;
            }
        }

        public static int? ConvertToInteger(string value)
        {
            if (string.IsNullOrEmpty(value) ||
                value.IsNullOrWhiteSpace())
            {
                return null;
            }

            int intValue;

            bool parsed = int.TryParse(value, out intValue);

            if (!parsed)
            {
                return null;
            }

            return intValue;
        }

        public static DateTime? ConvertToDateTime(object value)
        {
            return Convertor.ConvertToDateTime(value, CultureInfo.CurrentCulture.DateTimeFormat);
        }

        public static DateTime? ConvertToDateTime(object value, IFormatProvider formatProvider) 
        {
            if (value == null ||
                value == DBNull.Value)
            {
                return null;
            }

            DateTime resultDateTime;

            bool parsed = DateTime.TryParse(value.ToString(),
                                            formatProvider, 
                                            DateTimeStyles.None, 
                                            out resultDateTime);

            if (!parsed)
            {
                return null;
            }

            return DateTimeUtils.ToDateTime(resultDateTime);
        }

    }
}
