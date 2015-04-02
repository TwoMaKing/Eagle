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
    //[TableName("topic")]
    //[PrimaryKey("Id", AutoIncrement = true)]
    [Alias("topic")]
    [TablePrimaryKey("Id", ColumnName = "topic_id", AutoIncrement = true)]
    [TableIgnoreColumns(new string[] { "Modified" })]
    public class Topic : AggregateRoot
    {
        public Topic() : base() { }

        //[Column("topic_name")]
        [Alias("topic_name")]
        public string Name
        {
            get;
            set;
        }

        //[Column("topic_desc")]
        [Alias("topic_desc")]
        public string Summary { get; set; }

        [Ignore()]
        public DateTime ExpiredDate { get; set; }

        public static Topic Create(string name, string summary, DateTime expiredDate) 
        {
            Topic topic = new Topic();

            topic.Name = name;
            topic.Summary = summary;
            topic.ExpiredDate = expiredDate;

            return topic;
        } 
    }
}
