﻿using Eagle.Common.Cache;
using Eagle.Common.Utils;
using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Data;
using Eagle.Domain;
using Eagle.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Eagle.Repositories.Sql
{
    /// <summary>
    /// Repository used for Sql Server.
    /// </summary>
    public abstract class SqlRepository<TAggregateRoot> : Repository<TAggregateRoot> 
        where TAggregateRoot : class, IAggregateRoot<int>, IAggregateRoot, new()
    {
        protected delegate void AppendChildToAggregateRoot(TAggregateRoot aggregateRoot, int childEntityId);

        private ISqlRepositoryContext sqlRepositoryContext;

        private ICacheManager cacheManager = CacheFactory.GetCacheManager();

        public SqlRepository(IRepositoryContext repositoryContext) : base(repositoryContext) 
        {
            if (repositoryContext is ISqlRepositoryContext)
            {
                this.sqlRepositoryContext = (ISqlRepositoryContext)repositoryContext;
            }
            else
            {
                throw new ArgumentException("The specified Repository context is not the instance of ISqlRepositoryContext.");
            }
        }

        protected ISqlRepositoryContext SqlRepositoryContext 
        {
            get 
            {
                return this.sqlRepositoryContext;
            }
        }

        protected override void DoDelete(int id)
        {
            TAggregateRoot aggregateRoot = DoFindByKey(id);

            this.DoDelete(aggregateRoot);
        }

        protected override TAggregateRoot DoFindByKey(int id)
        {
            string cacheAggregateRootId = typeof(TAggregateRoot).Name + "_" + id.ToString();

            if (this.cacheManager.ContainsKey(cacheAggregateRootId))
            {
                return this.cacheManager.GetItem<TAggregateRoot>(cacheAggregateRootId);
            }

            TAggregateRoot currentAggregateRoot = default(TAggregateRoot);

            string querySqlById = this.GetAggregateRootQuerySqlById();

            using (IDataReader reader = DbGateway.Default.ExecuteReader(querySqlById, new object[] { id }))
            {
                var currentAggregateRoots = this.BuildAggregateRootsFromDataReader(reader);

                if (currentAggregateRoots != null &&
                    currentAggregateRoots.Count() > 0)
                {
                    currentAggregateRoot = currentAggregateRoots.FirstOrDefault();

                    Dictionary<string, AppendChildToAggregateRoot> childCallbacks = this.BuildChildCallbacks();

                    if (childCallbacks != null &&
                        childCallbacks.Count > 0)
                    {
                        foreach (KeyValuePair<string, AppendChildToAggregateRoot> callbackItem in childCallbacks)
                        {
                            string childEntityForeignKey = reader[callbackItem.Key].ToString();

                            int childEntityId = Convertor.ConvertToInteger(childEntityForeignKey).Value;

                            callbackItem.Value(currentAggregateRoot, childEntityId);
                        }
                    }
                }

                if (!reader.IsClosed)
                {
                    reader.Close();
                }
            }

            this.cacheManager.AddItem<TAggregateRoot>(cacheAggregateRootId, currentAggregateRoot);

            return currentAggregateRoot;
        }

        protected override TAggregateRoot DoFind(ISpecification<TAggregateRoot> specification)
        {
            return null;
        }

        protected override IEnumerable<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, Expression<Func<TAggregateRoot, dynamic>> sortPredicate, SortOrder sortOrder)
        {
            string fromTable = this.GetFromTableSqlByFindAll();

            string[] selectColumns = this.GetSelectColumnsByFindAll();

            var whereBuilderResult = this.SqlRepositoryContext.GetWhereClauseSql<TAggregateRoot>(expression);

            string orderBy = this.SqlRepositoryContext.GetOrderByClauseSql<TAggregateRoot>(sortPredicate) +
                             (sortOrder == SortOrder.Descending ? " DESC " : " ASC ");

            IDataReader dataReader = this.SqlRepositoryContext.Select(fromTable,
                                                                      selectColumns,
                                                                      whereBuilderResult.WhereClause,
                                                                      whereBuilderResult.ParameterValues.Values.ToArray(),
                                                                      orderBy);

            return this.BuildAggregateRootsFromDataReader(dataReader);
        }

        protected override IPagingResult<TAggregateRoot> DoFindAll(Expression<Func<TAggregateRoot, bool>> expression, 
                                                                   Expression<Func<TAggregateRoot, dynamic>> sortPredicate, 
                                                                   SortOrder sortOrder, 
                                                                   int pageNumber, 
                                                                   int pageSize)
        {
            string fromTable = this.GetFromTableSqlByFindAll();

            string[] selectColumns = this.GetSelectColumnsByFindAll();

            var whereBuilderResult = this.SqlRepositoryContext.GetWhereClauseSql<TAggregateRoot>(expression);

            string orderBy = this.SqlRepositoryContext.GetOrderByClauseSql<TAggregateRoot>(sortPredicate) +
                             (sortOrder == SortOrder.Descending ? " DESC " : " ASC ");

            IDataReader dataReader = this.SqlRepositoryContext.Select(fromTable,
                                                                      selectColumns,
                                                                      whereBuilderResult.WhereClause,
                                                                      whereBuilderResult.ParameterValues.Values.ToArray(),
                                                                      orderBy,
                                                                      pageNumber,
                                                                      pageSize,
                                                                      orderBy,
                                                                      true);

            IEnumerable<TAggregateRoot> aggregateRoots = this.BuildAggregateRootsFromDataReader(dataReader);

            return new PagingResult<TAggregateRoot>(aggregateRoots.Count(), null, pageNumber, pageSize, aggregateRoots.ToList());
        }

        #region Protected methods for finding the specified aggregate root

        protected abstract string GetFromTableSqlByFindAll();

        protected abstract string[] GetSelectColumnsByFindAll();

        protected abstract string GetAggregateRootQuerySqlById();

        protected virtual IEnumerable<TAggregateRoot> BuildAggregateRootsFromDataReader(IDataReader dataReader)
        {
            if (dataReader == null ||
                dataReader.IsClosed)
            {
                return null;
            }

            List<TAggregateRoot> aggregateRoots = new List<TAggregateRoot>();

            while (dataReader.Read())
            {
                TAggregateRoot aggregateRoot = this.BuildAggregateRootFromDataReader(dataReader);

                aggregateRoots.Add(aggregateRoot);
            }

            if (!dataReader.IsClosed)
            {
                dataReader.Close();
            }

            return aggregateRoots;
        }

        protected abstract TAggregateRoot BuildAggregateRootFromDataReader(IDataReader dataReader);

        protected abstract Dictionary<string, AppendChildToAggregateRoot> BuildChildCallbacks();

        #endregion

    }
}
