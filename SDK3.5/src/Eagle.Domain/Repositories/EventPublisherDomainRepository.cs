using Eagle.Domain.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    public abstract class EventPublisherDomainRepository : IDomainRepository
    {
        private IEventBus eventBus;

        public EventPublisherDomainRepository(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        protected IEventBus EventBus
        {
            get 
            {
                return this.eventBus;
            }
        }

        public TAggregateRoot Get<TAggregateRoot>(int Id) where TAggregateRoot : class, IEventSourceAggregateRoot
        {
            throw new NotImplementedException();
        }

        public void Save<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : class, IEventSourceAggregateRoot
        {
            throw new NotImplementedException();
        }

        public bool DistributedTransactionSupported
        {
            get { throw new NotImplementedException(); }
        }

        public bool Committed
        {
            get { throw new NotImplementedException(); }
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
