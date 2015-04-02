﻿using Eagle.Domain;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Tests.Domain.Models
{

    [TableName("post")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class Post : AggregateRoot
    {
        private Topic topic;

        private User author;

        private string content = string.Empty;

        private DateTime creationDateTime = DateTime.Now;

        private int praiseCount;

        public Post() : base() { }

        public Post(Topic topic, User author, string content)
        {
            this.topic = topic;
            this.author = author;
            this.content = content;
        }

        [Column("post_topic_id")]
        public int TopicId { get; set; }

        [Column("post_author_id")]
        public int AuthorId { get; set; }

        [Column("post_content")]
        public string Content
        {
            get 
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }

        [Column("post_creation_datetime")]
        public DateTime CreationDateTime
        {
            get 
            {
                return this.creationDateTime;
            }
            set
            {
                this.creationDateTime = value;
            }
        }

        public int PraiseCount 
        { 
            get 
            { 
                return praiseCount; 
            }
            private set
            {
                this.praiseCount = value;
            }
        }

        public Topic Topic
        {
            get
            {
                return this.topic;
            }
            set
            {
                this.topic = value;
            }
        }

        public User Author
        {
            get
            {
                return this.author;
            }
            set
            {
                this.author = value;
            }
        }

        
        public PostStatus Status { get; set; }

        #region Domain Business
        
        public void Publish() 
        { 

        }

        public void Forward() 
        { 
            
        }

        public void Praise()
        {

        }

        public void Collect() 
        { 
            
        }

        public static Post Create(Topic topic, User author, string content) 
        {
            return new Post(topic, author, content);
        }

        #endregion
    }

}
