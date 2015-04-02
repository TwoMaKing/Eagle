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
    public class PostDataObject : DataTransferObjectBase<Post>
    {
        public PostDataObject()
        {
            this.Topic = new TopicDataObject();
            this.Author = new UserDataObject();
        }

        [DataMember()]
        public int Id { get; set; }

        [DataMember()]
        public TopicDataObject Topic { get; set; }

        [DataMember()]
        public UserDataObject Author { get; set; }

        [DataMember()]
        public string Content { get; set; }

        [DataMember()]
        public DateTime CreationDateTime { get; set; }

        [DataMember()]
        public Guid InternalId { get; set; }

        [DataMember()]
        public string InternalStatus { get; set; }
        
        protected override void DoMapFrom(Post domainModel)
        {
            this.Id = domainModel.Id;
            
            TopicDataObject topic = new TopicDataObject();
            topic.Id = domainModel.Topic.Id;
            topic.Name = domainModel.Topic.Name;
            this.Topic = topic;

            UserDataObject author = new UserDataObject();
            author.Id = domainModel.Author.Id;
            author.Name = domainModel.Author.Name;
            author.NickName = domainModel.Author.NickName;
            this.Author = author;

            this.Content = domainModel.Content;
            this.CreationDateTime = domainModel.CreationDateTime;
        }

        protected override Post DoMapTo()
        {
            Post post = new Post();
            post.Id = this.Id;
            
            Topic topic = new Topic();
            topic.Id = this.Topic.Id;
            post.Topic = topic;

            User author = new User();
            author.Id = this.Author.Id;
            post.Author = author;

            post.TopicId = this.Topic.Id;
            post.AuthorId = this.Author.Id;
            post.Content = this.Content;

            post.Status = new PostStatus();
            post.Status.InternalId = this.InternalId;
            post.Status.InternalStatus = this.InternalStatus;

            return post;
        }

    }
}
