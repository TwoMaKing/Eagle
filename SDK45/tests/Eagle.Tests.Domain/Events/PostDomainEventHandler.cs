using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Domain.Events;
using Eagle.Domain.Repositories;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;

namespace Eagle.Tests.Domain.Events
{
    public class PostDomainEventHandler : IDomainEventHandler<PostDomainEvent>
    {
        private IRepositoryContext repositoryContext;

        private IPostRepository postRepository;

        public PostDomainEventHandler(
            IRepositoryContext repositoryContext,
            IPostRepository postRepository) 
        {
            this.repositoryContext = repositoryContext;
            this.postRepository = postRepository;
        }

        public void Handle(PostDomainEvent t)
        {
            if (t != null &&
                t.Post != null)
            {
                this.postRepository.Add(t.Post);

                this.repositoryContext.Commit();

                this.repositoryContext.Dispose();
            }
        }
    }
}
