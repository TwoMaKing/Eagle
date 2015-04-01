using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Domain;
using Eagle.Domain.Repositories;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Eagle.Repositories.Lite
{
    public class LiteRepository<TAggregateRoot> : Repository<TAggregateRoot> 
        where TAggregateRoot: class, IAggregateRoot<int>, IAggregateRoot, new()
    {
        private ILiteRepositoryContext liteRepositoryContext;

        public LiteRepository(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
            if (repositoryContext is ILiteRepositoryContext)
            {
                this.liteRepositoryContext = (ILiteRepositoryContext)repositoryContext;
            }
            else
            {
                throw new ArgumentException("The provided repository context type is invalid. LiteRepository requires an instance of LiteRepositoryContext to be initialized.");
            }
        }

        protected ILiteRepositoryContext LiteRepositoryContext 
        {
            get 
            {
                return this.liteRepositoryContext;
            }
        }

        protected override void DoAdd(TAggregateRoot aggregateRoot)
        {
            this.LiteRepositoryContext.RegisterAdded(aggregateRoot);
        }

        protected override void DoUpdate(TAggregateRoot aggregateRoot)
        {
            this.LiteRepositoryContext.RegisterModified(aggregateRoot);
        }

        protected override void DoDelete(TAggregateRoot aggregateRoot)
        {
            this.LiteRepositoryContext.RegisterDeleted(aggregateRoot);
        }

        protected override void DoDelete(int id)
        {
            TAggregateRoot aggregateRootToBeDeleted = this.DoFind(new ExpressionSpecification<TAggregateRoot>(a=>a.Id == id));

            this.LiteRepositoryContext.RegisterDeleted(aggregateRootToBeDeleted);
        }

        protected override TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification)
        {
            using (IDbConnection dbConnection = this.LiteRepositoryContext.LiteConnectionFactory.CreateDbConnection())
            {
                TAggregateRoot aggregateRoot = dbConnection.Single<TAggregateRoot>(specification.GetExpression());

                return aggregateRoot;
            }
        }

        protected override IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, 
                                                                 Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                                                 SortOrder sortOrder)
        {
            using (IDbConnection dbConnection = this.LiteRepositoryContext.LiteConnectionFactory.CreateDbConnection())
            {
                SqlExpression<TAggregateRoot> sqlExpression = dbConnection.From<TAggregateRoot>().Where(expression);

                if (sortOrder == SortOrder.Descending)
                {
                    sqlExpression = sqlExpression.OrderByDescending(sortPredicate);
                }
                else
                {
                    sqlExpression = sqlExpression.OrderBy(sortPredicate);
                }

                List<TAggregateRoot> aggregateRoots = dbConnection.Select(sqlExpression);

                return aggregateRoots;
            }
        }

        protected override IPagingResult<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, 
                                                                   Expression<Func<TAggregateRoot, object>> sortPredicate, 
                                                                   SortOrder sortOrder, 
                                                                   int pageNumber, 
                                                                   int pageSize)
        {
            throw new NotImplementedException();
        }
    }

}
