using Eagle.Common.Query;
using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Query;
using Eagle.Domain.Repositories;
using Eagle.Tests.Commands;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Events;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;
using Eagle.Tests.ServiceContracts;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Eagle.Tests
{
    [TestFixture()]
    public class ORMLiteTests
    {

        [Test()]
        public void Test_Query()
        {
            string connectionString = @"server=localhost\OSPTTEST;database=EXPRESSLIFE;User ID=sa;Password=3Edcvfr4567ujmnb";

            var dbFactory = new OrmLiteConnectionFactory(connectionString, SqlServerOrmLiteDialectProvider.Instance);

            using (IDbConnection connection = dbFactory.OpenDbConnection())
            {
                var post = connection.Single<Post>(p => p.Id == 1016);
                connection.LoadReferences<Post>(post);

                SqlExpression<Post> ex = connection.From<Post>();
                ex.Join<Topic>((p, t) => p.TopicId == t.Id).Join<User>((p, u) => p.AuthorId == u.Id).Where(p => p.Id == 1016);
                var postsByExp = connection.LoadSelect<Post>(ex);

                var postBySql = connection.Single<Post>(@"select post.*, topic.* from post inner join topic on post.post_topic_id = topic.topic_id where post.post_id = @post_id", new { Id = 1016 });

                Assert.AreEqual(postBySql.Id, 1016);
            }

        }

        

    }
}
