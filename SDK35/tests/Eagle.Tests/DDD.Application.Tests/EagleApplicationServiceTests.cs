using Eagle.Common.Query;
using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Query;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Dapper;
using Eagle.Repositories.Sql;
using Eagle.Tests.Commands;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Events;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;
using Eagle.Tests.ServiceContracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;

namespace Eagle.Tests
{
    [TestFixture]
    public class EagleApplicationServiceTests : TestBase
    {
        public EagleApplicationServiceTests() : base() { }

        [Test]
        public void Test_PublishPost()
        {
            IPostService postService = ServiceLocator.Instance.GetService<IPostService>();

            PostDataObject post = new PostDataObject();

            post.Content = "Add By Application Service";
            post.Topic.Id = 1000;
            post.Author.Id = 1000;
            post.InternalId = Guid.NewGuid();
            post.InternalStatus = "NEW";

            postService.PublishPost(post);
        }

        [Test]
        public void Test_QueryPostsByPaging()
        {

        }

    }
}
