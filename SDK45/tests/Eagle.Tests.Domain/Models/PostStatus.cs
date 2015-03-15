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
    //[TableName("post_status")]
    [Alias("post_status")]
    public class PostStatus : ValueObjectBase<PostStatus>
    {
        [Alias("post_status_post_id")]
        public int PostId { get; set; }

        //[Column("post_internal_id")]
        [Alias("post_internal_id")]
        public Guid InternalId { get; set; }

        //[Column("post_internal_status")]
        [Alias("post_internal_status")]
        public string InternalStatus { get; set; }
    }
}
