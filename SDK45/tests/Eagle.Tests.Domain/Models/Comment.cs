using Eagle.Domain;
using ServiceStack.DataAnnotations;
//using NPoco;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Tests.Domain.Models
{
    [Alias("comment")]
    [TablePrimaryKey("Id", ColumnName = "comment_id", AutoIncrement = true)]
    [TableIgnoreColumns(new string[] { "Modified" })]
    public class Comment : AggregateRoot
    {
        private Post post;

        private User author;

        private string content = string.Empty;

        private DateTime creationDateTime = DateTime.Now;

        public Comment() : base() { }

        public Comment(Post post, User author, string content) : base()
        {
            this.post = post;
            this.author = author;
            this.content = content;
        }

        [Alias("comment_post_id")]
        //[References(typeof(Post))]
        public int PostId { get; set; }

        [Alias("comment_author_id")]
        [References(typeof(User))]
        public int AuthorId { get; set; }

        [Alias("comment_content")]
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

        [Alias("comment_creation_datetime")]
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

        //[Reference()]
        //public Post Post
        //{
        //    get
        //    {
        //        return this.post;
        //    }
        //    set
        //    {
        //        this.post = value;
        //    }
        //}

        public static Comment Create(Post post, User author, string content) 
        {
            return new Comment(post, author, content);
        }
    }
}
