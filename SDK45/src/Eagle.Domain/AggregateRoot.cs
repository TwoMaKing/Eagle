using Eagle.Core;
using Eagle.Domain;
using Eagle.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain
{
    public class AggregateRoot : EntityBase, IAggregateRoot
    {
        protected void RaiseEvent<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : class, IEvent
        {
            IEnumerable<IEventHandler<TDomainEvent>> eventHandlers = ServiceLocator.Instance.ResolveAll<IEventHandler<TDomainEvent>>();

            foreach (IEventHandler<TDomainEvent> eventHandler in eventHandlers)
            {
                if (eventHandler.GetType().IsDefined(typeof(AsyncExecutionAttribute), false))
                {
                
                }
                else
                {
                    eventHandler.Handle(domainEvent);
                }
            }
        }

    }
}
