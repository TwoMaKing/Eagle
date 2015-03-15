using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Data.Mapping;
using Eagle.Data.Queries;

namespace Eagle.Data.SqlServer
{
    public class SqlServerWhereClauseBuilder<T> : WhereClauseBuilder<T> where T : class, new()
    {
        public SqlServerWhereClauseBuilder(IObjectMappingResolver mappingResolver) : base(mappingResolver) { }

        public SqlServerWhereClauseBuilder() : base() { }

        protected internal override char ParameterChar
        {
            get 
            {
                return '@';
            }
        }
    }
}
