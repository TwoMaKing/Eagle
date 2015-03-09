using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Events
{
    /// <summary>
    /// Represents this is an asynchronization event/command hanlder
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false)]
    public class HandleAsynchronizationAttribute : Attribute
    {

    }
}
