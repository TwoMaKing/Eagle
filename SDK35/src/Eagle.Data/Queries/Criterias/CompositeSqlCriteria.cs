﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Data.Queries.Criterias
{
    public abstract class CompositeSqlCriteria : OperatorSqlCriteria, ICompositeSqlCriteria
    {
        public CompositeSqlCriteria(ISqlCriteria left, ISqlCriteria right) 
        {
            this.Left = left;
            this.Right = right;
        }

        public ISqlCriteria Left
        {
            get;
            set;
        }

        public ISqlCriteria Right
        {
            get;
            set;
        }
    }
}
