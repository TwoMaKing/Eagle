using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Events
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
    public class DomainEventHandleAttribute : Attribute
    {
        private Type domainEventType;
        public DomainEventHandleAttribute(Type domainEventType)
        {
            this.domainEventType = domainEventType;
        }

        public Type DomainEventType 
        { 
            get 
            {
                return this.domainEventType;
            } 
        }
    }
}
