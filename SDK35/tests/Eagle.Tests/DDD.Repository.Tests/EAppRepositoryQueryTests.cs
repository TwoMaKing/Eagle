using Eagle.Common.Query;
using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Query;
using Eagle.Domain.Repositories;
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
    [TestFixture()]
    public class EAppRepositoryTests : TestBase
    {
        public EAppRepositoryTests() : base() 
        { 
            
        }

        [Test()]
        public void Test_PostRepository_FindAllOrderByWithoutPaging() 
        {
            PostQueryRequest request = new PostQueryRequest();
            request.TopicId = 1000;
            request.CreationDateTimeParam.CreationDateTimeOperator = Operator.LessThanEqual;
            request.CreationDateTimeParam.CreationDateTime = DateTimeUtils.ToDateTime("2015-1-22").Value;

            using (IRepositoryContext repositoryContext = ServiceLocator.Instance.GetService<IRepositoryContext>())
            {
                IRepository<Post> postRepository = repositoryContext.GetRepository<Post>();

                Expression<Func<Post, bool>> dateTimeExpression = (p) => true;

                //DateTime dt = request.CreationDateTimeParam.CreationDateTime;

                switch (request.CreationDateTimeParam.CreationDateTimeOperator)
                {
                    case Operator.LessThanEqual:
                        dateTimeExpression = p => p.CreationDateTime <= request.CreationDateTimeParam.CreationDateTime;
                        break;
                    case Operator.GreaterThanEqual:
                        dateTimeExpression = p => p.CreationDateTime >= request.CreationDateTimeParam.CreationDateTime;
                        break;
                    case Operator.Equal:
                        dateTimeExpression = p => p.CreationDateTime.Equals(request.CreationDateTimeParam.CreationDateTime);
                        break;
                }

                QueryBuilder<Post> postQueryBuilder = new QueryBuilder<Post>();

                //int topicId = request.TopicId;

                postQueryBuilder.And(p => p.TopicId == request.TopicId).And(dateTimeExpression);

                IEnumerable<Post> posts = postRepository.FindAll(postQueryBuilder.QueryPredicate);

                IList<PostDataObject> postDataObjects = new List<PostDataObject>();

                foreach (Post post in posts)
                {
                    var postDataObject = new PostDataObject();
                    postDataObject.MapFrom(post);

                    postDataObjects.Add(postDataObject);
                }

                Assert.AreEqual(5, postDataObjects.Count);
            }
        
        }

    }
}
