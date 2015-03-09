using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Eagle.Common.Utils;
using Eagle.Core;
using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Data;
using Eagle.Data.Queries;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Sql;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;

namespace Eagle.Tests.Repositories
{
    public class PostRepository : SqlRepository<Post>, IPostRepository
    {
        public PostRepository(IRepositoryContext repositoryContext)
            : base(repositoryContext)
        { 

        }

        public IEnumerable<Post> GetPostsPublishedByUser(User user)
        {
            throw new NotImplementedException();
        }

        protected override Post DoFind(ISpecification<Post> specification)
        {
            throw new NotImplementedException();
        }

        protected override string GetAggregateRootQuerySqlById()
        {
            using (ISqlQuery sqlQuery = new SqlQuery())
            {
                string queryByIdSql = sqlQuery.From("post")
                                              .InnerJoin("topic", "post_topic_id", "topic_id")
                                              .InnerJoin("[user]", "post_author_id", "user_id")
                                              .Equals("post_id", "?")
                                              .Select("post_id",
                                                      "post_topic_id",
                                                      "post_author_id",
                                                      "post_content",
                                                      "post_creation_datetime",
                                                      "topic_name",
                                                      "user_name")
                                              .SqlBuilder
                                              .GetQuerySql();

                return queryByIdSql;
            }
        }

        protected override Post BuildAggregateRootFromDataReader(IDataReader dataReader)
        { 
            Post post = new Post();

            post.Id = Convertor.ConvertToInteger(dataReader["post_id"]).Value;
            post.Content = dataReader["post_content"].ToString().Trim();
            post.CreationDateTime = Convertor.ConvertToDateTime(dataReader["post_creation_datetime"]).Value;

            post.Topic = new Topic();
            post.Topic.Id = Convertor.ConvertToInteger(dataReader["post_topic_id"]).Value;
            post.Topic.Name = dataReader["topic_name"].ToString().Trim();

            post.Author = new User();
            post.Author.Id = Convertor.ConvertToInteger(dataReader["post_topic_id"]).Value;
            post.Author.Name = dataReader["user_name"].ToString();

            return post;
        }

        protected override Dictionary<string, AppendChildToAggregateRoot> BuildChildCallbacks()
        {
            Dictionary<string, AppendChildToAggregateRoot> childCallbacks = new Dictionary<string, AppendChildToAggregateRoot>();

            childCallbacks.Add("post_topic_id", AppendTopicToPost);
            childCallbacks.Add("post_author_id", AppendAuthorToPost);

            return childCallbacks;
        }

        private void AppendTopicToPost(Post post, int topicId)
        {
            ITopicRepository topicReposiotry = ServiceLocator.Instance.GetService<ITopicRepository>();

            Topic topic = topicReposiotry.FindByKey(topicId);

            post.Topic = topic;
        }

        private void AppendAuthorToPost(Post post, int authorId)
        {
            IUserRepository userReposiotry = ServiceLocator.Instance.GetService<IUserRepository>();

            User user = userReposiotry.FindByKey(authorId);

            post.Author = user;
        }

        protected override void DoPersistAddedItems(IEnumerable<Post> aggregateRoots)
        {
            foreach (Post post in aggregateRoots)
            {
                this.SqlRepositoryContext.Insert("post",
                                                 new object[] { post.Topic.Id, 
                                                                post.Author.Id, 
                                                                post.Content, 
                                                                post.CreationDateTime });
            }
        }

        protected override void DoPersistModifiedItems(IEnumerable<Post> aggregateRoots)
        {
            foreach (Post post in aggregateRoots)
            {
                this.SqlRepositoryContext.Update("post",
                                                 new string[] { "post_topic_id", "post_author_id", "content" },
                                                 new object[] { post.Topic.Id, 
                                                                post.Author.Id, 
                                                                post.Content },
                                                 "post_id=@post_id",
                                                 new object[] { post.Id });
            }
        }

        protected override void DoPersistDeletedItems(IEnumerable<Post> aggregateRoots)
        {
            foreach (Post post in aggregateRoots)
            {
                this.SqlRepositoryContext.Delete("post",
                                                 "post_id=@post_id",
                                                 new object[] { post.Id });
            }
        }

        protected override string GetFromTableSqlByFindAll()
        {
            return @"post
                     inner join topic on post_topic_id = topic_id
                     inner join [user] on post_author_id = user_id";
        }

        protected override string[] GetSelectColumnsByFindAll()
        {
            return new string[] { "post_id", "post_topic_id", "post_author_id", "post_content", "post_creation_datetime", "topic_name", "user_name" };
        }
    }
}
