using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Eagle.Domain.Repositories
{
    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot, int>, IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new()
    {
        private IRepositoryContext repositoryContext;

        public Repository(IRepositoryContext repositoryContext) 
        {
            this.repositoryContext = repositoryContext;
        }

        public IRepositoryContext RepositoryContext
        {
            get 
            {
                return this.repositoryContext; 
            }
        }

        #region Aggregate root Creation/Update/Deletion

        public void Add(TAggregateRoot aggregateRoot)
        {
            this.DoAdd(aggregateRoot);
        }

        public void Add(IEnumerable<TAggregateRoot> aggregateRoots)
        {
            this.DoAdd(aggregateRoots);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            this.DoUpdate(aggregateRoot);
        }

        public void Update(IEnumerable<TAggregateRoot> aggregateRoots)
        {
            this.DoUpdate(aggregateRoots);
        }

        public void Delete(TAggregateRoot aggregateRoot)
        {
            this.DoDelete(aggregateRoot);
        }

        public void Delete(IEnumerable<TAggregateRoot> aggregateRoots)
        {
            this.DoDelete(aggregateRoots);
        }

        public void Delete(int id)
        {
            this.DoDelete(id);
        }

        #endregion

        #region Find the aggregate root by Id or specification

        public TAggregateRoot FindByKey(int id)
        {
            return this.DoFindByKey(id);
        }

        public TAggregateRoot Find(ISpecification<TAggregateRoot> specification)
        {
            return this.DoFind(specification);
        }

        #endregion

        #region Aggregate roots queries

        public IEnumerable<TAggregateRoot> FindAll()
        {
            return this.DoFindAll(new AnySepcification<TAggregateRoot>().GetExpression(), TAggregateRoot => TAggregateRoot.Id, SortOrder.None);
        }

        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder)
        {
            return this.DoFindAll(new AnySepcification<TAggregateRoot>().GetExpression(), sortPredicate, sortOrder);
        }

        public IPagingResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoFindAll(new AnySepcification<TAggregateRoot>().GetExpression(), sortPredicate, sortOrder, pageNumber, pageSize);
        }

        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> expression)
        {
            return this.DoFindAll(expression, TAggregateRoot => TAggregateRoot.Id, SortOrder.None);
        }

        public IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder)
        {
            return this.DoFindAll(expression, sortPredicate, sortOrder);
        }

        public IPagingResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            return this.DoFindAll(expression, sortPredicate, sortOrder, pageNumber, pageSize);
        }

        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("Query spcification is null. Please specify a specification.");
            }

            return this.DoFindAll(specification.GetExpression(), TAggregateRoot => TAggregateRoot.Id, SortOrder.None);
        }

        public IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("Query spcification is null. Please specify a specification.");
            }

            return this.DoFindAll(specification.GetExpression(), sortPredicate, sortOrder);
        }

        public IPagingResult<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            if (specification == null)
            {
                throw new ArgumentNullException("Query spcification is null. Please specify a specification.");
            }

            return this.DoFindAll(specification.GetExpression(), sortPredicate, sortOrder, pageNumber, pageSize);
        }

        #endregion

        #region Protected methods

        #region Aggregate root Creation/Update/Deletion

        protected virtual void DoAdd(TAggregateRoot aggregateRoot)
        {
            this.DoAdd(new TAggregateRoot[] { aggregateRoot });
        }

        protected abstract void DoAdd(IEnumerable<TAggregateRoot> aggregateRoots);

        protected virtual void DoUpdate(TAggregateRoot aggregateRoot)
        {
            this.DoUpdate(new TAggregateRoot[] { aggregateRoot });
        }

        protected abstract void DoUpdate(IEnumerable<TAggregateRoot> aggregateRoots);

        protected virtual void DoDelete(TAggregateRoot aggregateRoot) 
        {
            this.DoDelete(new TAggregateRoot[] { aggregateRoot });
        }

        protected abstract void DoDelete(IEnumerable<TAggregateRoot> aggregateRoots);

        protected abstract void DoDelete(int id);

        #endregion

        #region Find the aggregate root by Id or specification

        protected virtual TAggregateRoot DoFindByKey(int id) 
        {
            return this.DoFind(new ExpressionSpecification<TAggregateRoot>(aggregateRoot => aggregateRoot.Id.Equals(id)));
        }

        protected abstract TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification);

        #endregion

        #region Aggregate roots Queries

        protected abstract IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, 
                                                                 Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                                                 SortOrder sortOrder);

        protected abstract IPagingResult<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, 
                                                                   Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                                                   SortOrder sortOrder, 
                                                                   int pageNumber, 
                                                                   int pageSize);

        #endregion

        #endregion
    }
}
