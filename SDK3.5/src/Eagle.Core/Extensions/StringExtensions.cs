using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value.Trim(' '));
        }

        public static bool HasValue(this string @string)
        {
            return !string.IsNullOrEmpty(@string) &&
                   !@string.IsNullOrWhiteSpace();
        }
    }
}
