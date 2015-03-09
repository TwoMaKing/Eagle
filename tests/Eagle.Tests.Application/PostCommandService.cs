using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Domain.Bus;
using Eagle.Domain.Commands;
using Eagle.Tests.Commands;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Events;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;
using Eagle.Tests.Repositories;
using Eagle.Tests.ServiceContracts;

namespace Eagle.Tests.Application
{
    public class PostCommandService : IPostCommandService
    {
        public void PublishPost(PostDataObject post)
        {
            PostPublishCommand command = new PostPublishCommand();
          
            command.TopicId = post.Topic.Id;
            command.AuthorId = post.Author.Id;
            command.Content = post.Content;

            command.PostDataObject = post;

            this.ExecuteCommand(command);
        }

        private void ExecuteCommand(ICommand command) 
        {
            using (ICommandBus commandBus = ServiceLocator.Instance.GetService<ICommandBus>())
            {
                commandBus.Publish(command);

                commandBus.Commit();
            }
        }
    }
}
