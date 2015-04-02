using Dappers;
using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Domain;
using Eagle.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Eagle.Repositories.Dapper
{
    public abstract class DapperRepository<TAggregateRoot> : Repository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new()
    {
        private IDapperRepositoryContext dapperRepositoryContext;

        public DapperRepository(IRepositoryContext repositoryContext) :base(repositoryContext)
        {
            if (repositoryContext is IDapperRepositoryContext)
            {
                this.dapperRepositoryContext = (IDapperRepositoryContext)repositoryContext;
            }
            else
            {
                throw new ArgumentException("The provided repository context type is invalid. DapperRepository requires an instance of DapperRepositoryContext to be initialized.");
            }
        }

        protected IDapperRepositoryContext DapperRepositoryContext 
        {
            get
            {
                return this.dapperRepositoryContext;
            }
        }

        #region Find the aggregate root by Id or specification using Dapper

        protected override TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification)
        {
            return this.DapperRepositoryContext.Query<TAggregateRoot>("", null).SingleOrDefault();
        }

        #endregion

        #region Aggregate roots queries using Dapper
        protected override IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder)
        {
            throw new NotImplementedException();
        }

        protected override IPagingResult<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, object>> sortPredicate, SortOrder sortOrder, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Aggregate root Creation/Update/Deletion using Dapper

        protected override void DoAdd(TAggregateRoot aggregateRoot)
        {
            string insertSqlStatement = this.GetAggregateRootInsertSqlStatement();

            object insertParameters = this.GetAggregateRootInsertParameters(aggregateRoot);

            this.DapperRepositoryContext.RegisterAdded(new CommandSqlParameters { CommandSql = insertSqlStatement, Parameters = insertParameters });
        }

        protected override void DoUpdate(TAggregateRoot aggregateRoot)
        {
            string updateSqlStatement = this.GetAggregateRootUpdateSqlStatement();

            object updateParameters = this.GetAggregateRootUpdateParameters(aggregateRoot);

            this.DapperRepositoryContext.RegisterModified(new CommandSqlParameters { CommandSql = updateSqlStatement, Parameters = updateParameters });
        }

        protected override void DoDelete(TAggregateRoot aggregateRoot)
        {
            string deleteSqlStatement = this.GetAggregateRootDeleteSqlStatement();

            object deleteParameters = this.GetAggregateRootDeleteParameters(aggregateRoot);

            this.DapperRepositoryContext.RegisterDeleted(new CommandSqlParameters { CommandSql = deleteSqlStatement, Parameters = deleteParameters });
        }

        protected override void DoDelete(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected methods

        protected abstract string GetAggregateRootQuerySqlStatementById();

        protected abstract string GetAggregateRootInsertSqlStatement();

        protected abstract object GetAggregateRootInsertParameters(TAggregateRoot aggregateRoot);

        protected abstract string GetAggregateRootUpdateSqlStatement();

        protected abstract object GetAggregateRootUpdateParameters(TAggregateRoot aggregateRoot);

        protected abstract string GetAggregateRootDeleteSqlStatement();

        protected abstract object GetAggregateRootDeleteParameters(TAggregateRoot aggregateRoot);

        #endregion
    }
}
