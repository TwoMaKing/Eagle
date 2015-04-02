using Eagle.Domain;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Eagle.Tests.Domain.Models
{
    [TableName("post_status")]
    public class PostStatus : ValueObjectBase<PostStatus>
    {
        [Column("post_internal_id")]
        public Guid InternalId { get; set; }

        [Column("post_internal_status")]
        public string InternalStatus { get; set; }
    }
}
