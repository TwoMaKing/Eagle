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
    /// <summary>
    /// Repository interface for an aggregate.
    /// </summary>
    public interface IRepository<TAggregateRoot, TIdentityKey> where TAggregateRoot : class, IAggregateRoot<TIdentityKey>, new()
    {
        /// <summary>
        /// Return a repository context with Unit Of Work.
        /// </summary>
        IRepositoryContext RepositoryContext { get; }

        /// <summary>
        ///  Aadd an aggregate root to Repository.
        /// </summary>
        void Add(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Update an aggregate root to Repository.
        /// </summary>
        void Update(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Delete the specified aggregate root from Repository.
        /// </summary>
        void Delete(TAggregateRoot aggregateRoot);

        /// <summary>
        /// Delete an aggregate root from Repository by item key.
        /// </summary>
        void Delete(TIdentityKey id);

        /// <summary>
        /// Find the specific aggregate root by id or key.
        /// </summary>
        TAggregateRoot FindByKey(TIdentityKey id);

        /// <summary>
        /// Find the specific aggregate root by the specification from repository.
        /// </summary>
        TAggregateRoot Find(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// Find all of aggregate roots from repository.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TAggregateRoot> FindAll();

        /// <summary>
        /// Find all of aggregate roots from repository and then sort by sort predicate.
        /// </summary>
        /// <param name="sortPredicate"></param>
        /// <param name="sorOrder"></param>
        /// <returns></returns>
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder);

        /// <summary>
        /// Find all of aggregate roots matching paging condition from repository and then sort by sort predicate.
        /// </summary>
        IPagingResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                              SortOrder sortOrder, 
                                              int pageNumber, 
                                              int pageSize);

        /// <summary>
        /// Find all of aggregate roots matching query expression from repository.
        /// </summary>
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> expression);

        /// <summary>
        /// Find all of aggregate roots matching query expression from repository and then sort by sort predicate.
        /// </summary>
        IEnumerable<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> expression, 
                                            Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                            SortOrder sortOrder);

        /// <summary>
        /// Find all of aggregate roots matching query expression and paging condition from repository and then sort by sort predicate
        /// </summary>
        IPagingResult<TAggregateRoot> FindAll(Expression<Func<TAggregateRoot, bool>> expression, 
                                              Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                              SortOrder sortOrder, 
                                              int pageNumber, 
                                              int pageSize);

        /// <summary>
        /// Find all of aggregate roots by the specification from repository,
        /// </summary>
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification);

        /// <summary>
        /// Find all of aggregate roots by the specification from repository and then sort by sort predicate.
        /// </summary>
        IEnumerable<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, 
                                            Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                            SortOrder sortOrder);

        /// <summary>
        /// Find all of aggregate roots by the specification and paging condition from repository and then sort by sort predicate.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The number of objects per page.</param>
        IPagingResult<TAggregateRoot> FindAll(ISpecification<TAggregateRoot> specification, 
                                              Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                              SortOrder sortOrder,
                                              int pageNumber, 
                                              int pageSize);

    }

    public interface IRepository<TAggregateRoot> : IRepository<TAggregateRoot, int>
        where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new()
    { 
    
    }
}
