using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Data;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Sql;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;

namespace Eagle.Tests.Repositories
{
    public class CommentRepository : SqlRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IRepositoryContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Comment> GetCommentsByPost(Post post)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootQuerySqlById()
        {
            throw new NotImplementedException();
        }

        protected override Comment BuildAggregateRootFromDataReader(IDataReader dataReader)
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<string, AppendChildToAggregateRoot> BuildChildCallbacks()
        {
            throw new NotImplementedException();
        }

        protected override string GetFromTableSqlByFindAll()
        {
            throw new NotImplementedException();
        }

        protected override string[] GetSelectColumnsByFindAll()
        {
            throw new NotImplementedException();
        }

        protected override void DoAdd(Comment comment)
        {
            throw new NotImplementedException();
        }

        protected override void DoUpdate(Comment comment)
        {
            throw new NotImplementedException();
        }

        protected override void DoDelete(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
