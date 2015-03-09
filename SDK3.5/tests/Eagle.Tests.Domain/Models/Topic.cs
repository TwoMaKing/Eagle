using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Domain;
using NPoco;

namespace Eagle.Tests.Domain.Models
{
    [TableName("topic")]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class Topic : AggregateRoot
    {
        public Topic() : base() { }

        [Column("topic_name")]
        public string Name
        {
            get;
            set;
        }

        [Column("topic_desc")]
        public string Summary { get; set; }

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
