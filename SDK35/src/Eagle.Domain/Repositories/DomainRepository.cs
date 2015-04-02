using Eagle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Domain.Repositories
{
    public abstract class DomainRepository : DisposableObject,  IDomainRepository
    {
        private bool committed;

        private readonly HashSet<IEventSourceAggregateRoot> savedAggregateRoots = new HashSet<IEventSourceAggregateRoot>();

        protected HashSet<IEventSourceAggregateRoot> SavedAggregateRoots 
        {
            get 
            {
                return this.savedAggregateRoots;
            }
        }

        protected abstract void DoCommit();

        protected abstract void DoRollback();

        protected override void Dispose(bool disposing) { }

        public abstract TAggregateRoot Get<TAggregateRoot>(int Id) where TAggregateRoot : class, IEventSourceAggregateRoot;

        public void Save<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : class, IEventSourceAggregateRoot
        {
            if (!this.savedAggregateRoots.Contains(aggregateRoot))
            {
                this.savedAggregateRoots.Add(aggregateRoot);
            }

            this.committed = false;
        }

        public abstract bool DistributedTransactionSupported { get; }

        public bool Committed
        {
            get 
            {
                return this.committed;
            }
        }

        public void Commit() 
        {
            this.DoCommit();
            this.SavedAggregateRoots.Clear();
            this.committed = true;
        }

        public void Rollback() 
        {
            this.DoRollback();
            this.committed = false;
        }
    }
}
