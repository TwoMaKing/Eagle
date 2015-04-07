using Eagle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eagle.Common.Utils
{
    public static class RegexUtils
    {
        /// <summary>
        /// Gets whether the input is an valid email address.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return IsMatch(pattern, input);
        }

        /// <summary>
        /// Gets whether the input is an valid cell phone no.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhone(string input)
        {
            return IsMatch(@"^13\\d{9}$", input);
        }

        public static bool IsMatch(string pattern, string input)
        {
            if (!input.HasValue())
            {
                return false;
            }

            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }
    }
}