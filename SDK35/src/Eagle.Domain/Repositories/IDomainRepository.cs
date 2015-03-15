using Eagle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    /// <summary>
    /// epresents that the implemented classes are domain repository used for CQRS event sourcing and event storage.
    /// </summary>
    public interface IDomainRepository : IUnitOfWork, IDisposable
    {
        TAggregateRoot Get<TAggregateRoot>(int Id) where TAggregateRoot : class, IEventSourceAggregateRoot;

        void Save<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : class, IEventSourceAggregateRoot;
    }
}
