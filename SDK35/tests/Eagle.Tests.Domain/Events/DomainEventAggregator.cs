using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Domain.Events;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;

namespace Eagle.Tests.Domain.Events
{
    public class DomainEventAggregator : EventAggregator
    {
        private readonly static DomainEventAggregator instance = new DomainEventAggregator();

        public DomainEventAggregator()
        {
            IDomainEventHandler<PostDomainEvent>  postDomainEventHandler = 
                ServiceLocator.Instance.GetService<IDomainEventHandler<PostDomainEvent>>();

            this.Subscribe<PostDomainEvent>(postDomainEventHandler);
        }

        public static DomainEventAggregator Instance 
        {
            get 
            {
                return instance;
            }
        }
    }
}
