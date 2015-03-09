using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Attributes
{
    /// <summary>
    /// Description of IgnoreAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class IgnoreAttribute : Attribute
    {
    }
}
