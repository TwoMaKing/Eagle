using Dappers;
using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Data;
using Eagle.Data.Queries;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Dapper;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;


namespace Eagle.Tests.Repositories
{
    public class DapperCommentRepository : DapperRepository<Comment>, ICommentRepository
    {
        public DapperCommentRepository(IRepositoryContext repositoryContext) : base(repositoryContext) { }

        protected override string GetAggregateRootQuerySqlStatementById()
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootInsertSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootInsertParameters(Comment aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootUpdateSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootUpdateParameters(Comment aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootDeleteSqlStatement()
        {
            throw new NotImplementedException();
        }

        protected override object GetAggregateRootDeleteParameters(Comment aggregateRoot)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetCommentsByPost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
