using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Events
{
    public abstract class DomainEvent : IDomainEvent
    {
        private DateTime timestamp = DateTime.UtcNow;

        private IEntity source;

        public DomainEvent(IEntity source)
        {
            this.source = source;
        }

        public int Id
        {
            get;
            set;
        }

        public IEntity Source
        {
            get 
            {
                return this.source;
            }
        }

        public DateTime TimeStamp
        {
            get 
            {
                return this.timestamp;
            }
        }
    }
}
