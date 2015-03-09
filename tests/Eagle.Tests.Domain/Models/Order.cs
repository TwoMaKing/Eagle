using Eagle.Domain;
using Eagle.Domain.Events;
using Eagle.Tests.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Tests.Domain.Models
{
    public class Order : AggregateRoot
    {
        public void Confirm() 
        {
            // confirmation logic

            OrderConfirmEvent @event = new OrderConfirmEvent(this);

            this.RaiseEvent<OrderConfirmEvent>(@event);
        }
    }
}
