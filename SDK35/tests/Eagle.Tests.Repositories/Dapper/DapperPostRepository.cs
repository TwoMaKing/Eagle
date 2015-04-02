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
    public class DapperPostRepository : DapperRepository<Post>, IPostRepository
    {
        public DapperPostRepository(IRepositoryContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Post> GetPostsPublishedByUser(User user)
        {
            throw new NotImplementedException();
        }

        protected override Post DoFindByKey(int id)
        {
            string querySqlStatement = this.GetAggregateRootQuerySqlStatementById();

            Func<IEnumerable<Post>, IEnumerable<PostStatus>, IEnumerable<Post>> map = (postList, statusList) => 
            {
                Post post = postList.SingleOrDefault();
                PostStatus postStatus = statusList.SingleOrDefault();
                post.Status = postStatus;

                return new Post[] { post };
            };

            return this.DapperRepositoryContext.QueryMultiple<Post, PostStatus, Post>(querySqlStatement, map, new { Id = id }).SingleOrDefault();
        }

        protected override string GetAggregateRootQuerySqlStatementById()
        {
            return @"SELECT post_id AS Id, 
                            post_topic_id AS TopicId,
                            post_author_id AS AuthorId,
                            post_content AS Content,
                            post_creation_datetime AS CreationDateTime
                     FROM [post] WHERE post_id = @Id;

                     SELECT post_internal_id AS InternalId,
                            post_internal_status AS InternalStatus
                     FROM [post_status] WHERE post_id = @Id";
        }

        protected override string GetAggregateRootInsertSqlStatement()
        {
            return @"INSERT INTO [post] VALUES (@TopicId, @AuthorId, @Content, GETDATE());
                     DECLARE @PostId INT;
                     SELECT @PostId = SCOPE_IDENTITY(); 
                     INSERT INTO [post_status] VALUES (@PostId, @InternalId, @InternalStatus)";
        }

        protected override object GetAggregateRootInsertParameters(Post post)
        {
            return new { post.TopicId, post.AuthorId, post.Content, post.Status.InternalId, post.Status.InternalStatus };
        }

        protected override string GetAggregateRootUpdateSqlStatement()
        {
            return @"UPDATE [post] SET post_topic_id = @TopicId, post_author_id = @AuthorId, post_content = @Content WHERE post_id = @Id;
                     UPDATE [post_status] SET post_internal_status = @InternalStatus WHERE post_id = @Id";
        }

        protected override object GetAggregateRootUpdateParameters(Post post)
        {
            return new { post.TopicId, post.AuthorId, post.Content, post.Status.InternalStatus, post.Id };
        }

        protected override string GetAggregateRootDeleteSqlStatement()
        {
            return @"DELETE FROM [post] WHERE post_id = @Id;
                     DELETE FROM [post_status] WHERE post_id = @Id";
        }

        protected override object GetAggregateRootDeleteParameters(Post post)
        {
            return new { post.Id };
        }
    }
}
