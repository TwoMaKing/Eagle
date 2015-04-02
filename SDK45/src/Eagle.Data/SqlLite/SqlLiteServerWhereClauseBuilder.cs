using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Data.Mapping;
using Eagle.Data.Queries;

namespace Eagle.Data.SqlLite
{
    public class SqlLiteServerWhereClauseBuilder<T> : WhereClauseBuilder<T> where T : class, new()
    {
        public SqlLiteServerWhereClauseBuilder(IObjectMappingResolver mappingResolver) : base(mappingResolver) { }

        public SqlLiteServerWhereClauseBuilder() : base() { }

        protected internal override char ParameterChar
        {
            get 
            {
                return '?'; 
            }
        }
    }
}
