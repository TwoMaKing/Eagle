using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Domain;
using Eagle.Domain.Events;

namespace Eagle.Tests.Domain.Events
{
    public class OrderConfirmEvent : DomainEvent
    {
        public OrderConfirmEvent(IEntity source) : base(source) { }
    }
}
