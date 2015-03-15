using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Data.Mapping;
using Eagle.Data.Queries;

namespace Eagle.Data.Oracle
{
    public class OracleWhereClauseBuilder<T> : WhereClauseBuilder<T> where T: class, new()
    {
        public OracleWhereClauseBuilder(IObjectMappingResolver mappingResolver) : base(mappingResolver) { }

        public OracleWhereClauseBuilder() : base() { }

        protected internal override char ParameterChar
        {
            get 
            {
                return ':'; 
            }
        }
    }
}
