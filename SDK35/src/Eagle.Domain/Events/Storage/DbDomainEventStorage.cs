using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Events.Storage
{
    public class DbDomainEventStorage : IDomainEventStorage
    {
        public void SaveEvent(IDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDomainEvent> LoadEvents(Type aggregateRootType, Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDomainEvent> LoadEvents(Type aggregateRootType, Guid id, long version)
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
