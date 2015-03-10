using Eagle.Domain.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    public abstract class EventPublisherDomainRepository : DomainRepository
    {
        private IEventBus eventBus;

        public EventPublisherDomainRepository(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public IEventBus EventBus 
        {
            get 
            {
                return this.eventBus;
            }
        }

        protected override void DoRollback()
        {
            this.eventBus.Rollback();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.eventBus.Dispose();
            }

            base.Dispose(disposing);
        }

        public override bool DistributedTransactionSupported
        {
            get 
            { 
                return this.eventBus.DistributedTransactionSupported; 
            }
        }
    }
}
