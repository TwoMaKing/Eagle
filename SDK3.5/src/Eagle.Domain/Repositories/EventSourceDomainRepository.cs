using Eagle.Domain.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    public class EventSourceDomainRepository : EventPublisherDomainRepository
    {
        public EventSourceDomainRepository(IEventBus eventBus) : base(eventBus)
        {
        
        }
    }
}
