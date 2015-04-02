using Eagle.Domain;
//using NPoco;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Eagle.Tests.Domain.Models
{
    //[TableName("post")]
    //[PrimaryKey("Id", AutoIncrement = true)]
    [Alias("post")]
    [TablePrimaryKey("Id", ColumnName = "post_id", AutoIncrement = true, ReferenceTypes = new Type[] { typeof(PostStatus) })]
    [TableIgnoreColumns(new string[] { "Modified" })]
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

        //[Column("post_topic_id")]
        [Alias("post_topic_id")]
        [References(typeof(Topic))]
        public int TopicId { get; set; }

        //[Column("post_author_id")]
        [Alias("post_author_id")]
        [References(typeof(User))]
        public int AuthorId { get; set; }

        //[Column("post_content")]
        [Alias("post_content")]
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

        //[Column("post_creation_datetime")]
        [Alias("post_creation_datetime")]
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

        [Ignore()]
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

        [Reference()]
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

        [Reference()]
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

        [Reference()]
        public PostStatus Status { get; set; }

        [Reference()]
        public List<Comment> Comments { get; set; }

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
