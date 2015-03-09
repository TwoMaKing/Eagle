﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Eagle.Tests.DataObjects
{

    [DataContract()]
    public class CommentDataObject
    {
        [DataMember()]
        public int AuthorId { get; set; }

        [DataMember()]
        public string AuthorName { get; set; }

        [DataMember()]
        public string Content { get; set; }

        [DataMember()]
        public DateTime CreationDateTime { get; set; }
    }
}
