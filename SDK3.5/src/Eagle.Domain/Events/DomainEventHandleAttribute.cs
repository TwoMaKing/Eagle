using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Events.Storage
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class DomainEventHandleAttribute : Attribute
    {
    }
}
