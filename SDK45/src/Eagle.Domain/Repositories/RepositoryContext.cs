using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Eagle.Domain.Repositories
{
    public abstract class RepositoryContext : DisposableObject, IRepositoryContext
    {
        private Guid id = Guid.NewGuid();

        private List<object> addedNewCollection = new List<object>();

        private List<object> modifiedCollection = new List<object>();

        private List<object> deletedCollection = new List<object>();

        private bool committed;

        private static Dictionary<Type, object> repositoryCaches = new Dictionary<Type, object>();

        private static readonly object lockObject = new object();

        public Guid Id
        {
            get 
            {
                return this.id;
            }
        }

        public virtual bool DistributedTransactionSupported
        {
            get 
            { 
                return false; 
            }
        }

        public bool Committed
        {
            get 
            {
                return this.committed;
            }
            protected set
            {
                this.committed = value;
            }
        }

        public IRepository<TAggregateRoot> GetRepository<TAggregateRoot>() where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new()
        {
            lock (lockObject)
            { 
                Type entityType = typeof(TAggregateRoot);

                if (!repositoryCaches.ContainsKey(entityType))
                {
                    IRepository<TAggregateRoot> repository = this.CreateRepository<TAggregateRoot>();

                    repositoryCaches.Add(entityType, repository);
                }

                return (IRepository<TAggregateRoot>)repositoryCaches[entityType];
            }
        }

        public virtual void RegisterAdded(object obj)
        {
            lock (lockObject)
            {
                this.addedNewCollection.Add(obj);

                this.committed = false;
            }
        }

        public virtual void RegisterModified(object obj)
        {
            lock (lockObject)
            {
                if (!this.modifiedCollection.Contains(obj) &&
                    !this.deletedCollection.Contains(obj))
                {
                    this.modifiedCollection.Add(obj);

                    this.committed = false;
                }
            }
        }

        public virtual void RegisterDeleted(object obj)
        {
            lock (lockObject)
            {
                if (this.addedNewCollection.Contains(obj))
                {
                    this.addedNewCollection.Remove(obj);

                    return;
                }

                if (this.modifiedCollection.Contains(obj))
                {
                    this.modifiedCollection.Remove(obj);
                }

                if (!this.deletedCollection.Contains(obj))
                {
                    this.deletedCollection.Add(obj);

                    this.committed = false;
                }
            }
        }

        public void Commit() 
        {
            this.DoCommit();

            this.committed = true;
        }

        public void Rollback() 
        {
            this.DoRollback();

            this.committed = false;
        }

        protected IEnumerable<object> AddedNewCollection
        {
            get
            {
                return this.addedNewCollection;
            }
        }

        protected IEnumerable<object> ModifiedCollection
        {
            get
            {
                return this.modifiedCollection;
            }
        }

        protected IEnumerable<object> DeletedCollection
        {
            get
            {
                return this.deletedCollection;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.addedNewCollection.Clear();
                this.modifiedCollection.Clear();
                this.deletedCollection.Clear();
            }
        }

        protected abstract void DoCommit();

        protected abstract void DoRollback();

        protected abstract IRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
            where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new();
    }

}
