﻿using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Dapper
{

    [Serializable]
    public class MappingException : DatabaseException
    {
        public MappingException() : base() { }

        public MappingException(string message, Exception innerException) : base(message, innerException) { }

        public MappingException(string message) : base(message) { }
    }
}
