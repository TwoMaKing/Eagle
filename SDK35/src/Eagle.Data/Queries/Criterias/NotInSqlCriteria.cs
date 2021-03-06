﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Data.Queries.Criterias
{
    public class NotInSqlCriteria : OperatorSqlCriteria
    {
        public NotInSqlCriteria(DbProvider dbProvider, string column) : base(dbProvider, column) { }

        protected override string GetOperatorChar()
        {
            return "NOT IN";
        }
    }
}
