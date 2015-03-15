using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string @string)
        {
            return !string.IsNullOrEmpty(@string) &&
                   !string.IsNullOrWhiteSpace(@string);
        }
    }
}
