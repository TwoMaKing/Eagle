﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Eagle.Domain.Application;
using Eagle.Tests.Domain.Models;

namespace Eagle.Tests.DataObjects
{
    [DataContract()]
    public class TopicDataObject : DataTransferObjectBase<Topic>
    {
        public TopicDataObject() { }

        [DataMember()]
        public int Id { get; set; }

        [DataMember()]
        public string Name { get; set; }

        [DataMember()]
        public string Summary { get; set; }

        protected override void DoMapFrom(Topic domainModel)
        {
            this.Id = domainModel.Id;
            this.Name = domainModel.Name;
            this.Summary = domainModel.Summary;
        }

        protected override Topic DoMapTo()
        {
            Topic topic = new Topic();

            topic.Id = this.Id;
            topic.Name = this.Name;
            topic.Summary = this.Summary;

            return topic;
        }
    }
}
