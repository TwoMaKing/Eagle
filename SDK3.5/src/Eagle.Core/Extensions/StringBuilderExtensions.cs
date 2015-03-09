using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void Clear(this StringBuilder stringBuilder)
        {
            if (stringBuilder != null)
            {
                stringBuilder.Remove(0, stringBuilder.Length);
            }
            else
            {
                throw new ArgumentNullException("The instance of StringBuilder cannot be null.");
            }
        }
    }
}
