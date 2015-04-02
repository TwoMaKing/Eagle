using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Domain.Commands;
using Eagle.Tests.DataObjects;

namespace Eagle.Tests.Commands
{
    public class PostPublishCommand : Command
    {
        public int TopicId { get; set; }

        public int AuthorId { get; set; }

        public string Content { get; set; }

        public PostDataObject PostDataObject { get; set; }
    }
}
