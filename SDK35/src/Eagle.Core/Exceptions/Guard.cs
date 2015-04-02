using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Eagle.Core.Exceptions
{
    public static class Guard
    {
        [DebuggerStepThrough]
        public static void NotNull(object argumentValue,
                                   string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void NotNullOrEmpty(string argumentValue,
                                          string argumentName)
        {
            if (argumentValue == null || argumentValue.Length == 0)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        [DebuggerStepThrough]
        public static void IsFalse(bool condition)
        {
            if (condition)
            {
                Fail(null);
            }
        }

        [DebuggerStepThrough]
        public static void IsTrue(bool condition)
        {
            if (!condition)
            {
                Fail(null);
            }
        }

        [DebuggerStepThrough]
        public static void IsTrue(bool condition, [Localizable(false)]string message)
        {
            if (!condition)
            {
                Fail(message);
            }
        }

        [DebuggerStepThrough]
        public static void Fail([Localizable(false)]string message)
        {
            throw new Exception(message);
        }
    }
}
