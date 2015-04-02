using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Domain.Events;

namespace Eagle.Tests.Domain.Events
{
    [AsyncExecution()]
    public class SendEmailEventHandler : IEventHandler<OrderConfirmEvent>
    {
        public void Handle(OrderConfirmEvent t)
        {
            //Send Email
        }
    }
}
