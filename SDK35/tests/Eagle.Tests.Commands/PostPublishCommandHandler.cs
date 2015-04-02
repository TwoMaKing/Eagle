using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Domain.Commands;
using Eagle.Domain.Repositories;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;

namespace Eagle.Tests.Commands
{
    public class PostPublishCommandHandler : ICommandHandler<PostPublishCommand>
    {
        public void Handle(PostPublishCommand message)
        {
            //Post post = message.PostDataObject.MapTo();
            //post.Publish();

            using (IRepositoryContext repositoryContext = ServiceLocator.Instance.GetService<IRepositoryContext>())
            {
                IPostRepository postRepository = (IPostRepository)repositoryContext.GetRepository<Post>();

                Post post = message.PostDataObject.MapTo();

                postRepository.Add(post);

                repositoryContext.Commit();
            }
        }
    }
}
