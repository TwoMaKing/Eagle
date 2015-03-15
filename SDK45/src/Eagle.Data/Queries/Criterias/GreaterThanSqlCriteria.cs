﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Data.Queries.Criterias
{
    public class GreaterThanSqlCriteria : OperatorSqlCriteria
    {
        public GreaterThanSqlCriteria(DbProvider dbProvider, string column) : base(dbProvider, column) { }

        protected override string GetOperatorChar()
        {
            return ">";
        }
    }
}
