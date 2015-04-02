using Eagle.Domain.Bus;
using Eagle.Domain.Events.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    public class EventSourceDomainRepository : EventPublisherDomainRepository
    {
        private IDomainEventStorage domainEventStorage;

        public EventSourceDomainRepository(IDomainEventStorage domainEventStorage, IEventBus eventBus) : base(eventBus)
        {
            this.domainEventStorage = domainEventStorage;
        }

        protected override void DoCommit()
        {

        }

        protected override void DoRollback()
        {
            base.DoRollback();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public IDomainEventStorage DomainEventStorage 
        {
            get 
            {
                return this.domainEventStorage;
            }
        }

        public override bool DistributedTransactionSupported
        {
            get
            {
                return base.DistributedTransactionSupported;
            }
        }

        public override TAggregateRoot Get<TAggregateRoot>(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
