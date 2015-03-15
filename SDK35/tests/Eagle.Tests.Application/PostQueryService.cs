using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;
using Eagle.Common.Query;
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

namespace Eagle.Tests.Application
{
    public class PostQueryService : DisposableObject, IQueryService
    {
        public IEnumerable<PostDataObject> GetPosts()
        {
            using (IRepositoryContext repositoryContext = ServiceLocator.Instance.GetService<IRepositoryContext>())
            {
                IRepository<Post> postRepository = repositoryContext.GetRepository<Post>();

                IEnumerable<Post> posts = postRepository.FindAll();

                IList<PostDataObject> postDataObjects = new List<PostDataObject>();

                foreach (Post post in posts)
                {
                    PostDataObject postDataObject = new PostDataObject();

                    postDataObject.MapFrom(post);

                    postDataObjects.Add(postDataObject);
                }

                return postDataObjects;
            }
        }

        public IEnumerable<PostDataObject> GetPostsByQueryRequest(PostQueryRequest request)
        {
            using (IRepositoryContext repositoryContext = ServiceLocator.Instance.GetService<IRepositoryContext>())
            {
                IRepository<Post> postRepository = repositoryContext.GetRepository<Post>();

                Expression<Func<Post, bool>> dateTimeExpression = (p) => true;

                switch (request.CreationDateTimeParam.CreationDateTimeOperator)
                {
                    case Operator.LessThanEqual:
                        dateTimeExpression = p => p.CreationDateTime <= request.CreationDateTimeParam.CreationDateTime;
                        break;
                    case Operator.GreaterThanEqual:
                        dateTimeExpression = p => p.CreationDateTime >= request.CreationDateTimeParam.CreationDateTime;
                        break;
                    case Operator.Equal:
                    default:
                        dateTimeExpression = p => p.CreationDateTime.Equals(request.CreationDateTimeParam.CreationDateTime);
                        break;
                }

                QueryBuilder<Post> postQueryBuilder = new QueryBuilder<Post>();

                postQueryBuilder.And(p => p.TopicId == request.TopicId).And(dateTimeExpression);

                IEnumerable<Post> posts = postRepository.FindAll(postQueryBuilder.QueryPredicate);

                IList<PostDataObject> postDataObjects = new List<PostDataObject>();

                foreach (Post post in posts)
                {
                    var postDataObject = new PostDataObject();
                    postDataObject.MapFrom(post);

                    postDataObjects.Add(postDataObject);
                }

                return postDataObjects;
            }
        }

        protected override void Dispose(bool disposing)
        {
            //
        }
    }
}
