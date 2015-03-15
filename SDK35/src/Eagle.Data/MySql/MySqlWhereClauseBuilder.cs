using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Data.Mapping;
using Eagle.Data.Queries;

namespace Eagle.Data.MySql
{
    public class MySqlWhereClauseBuilder<T> : WhereClauseBuilder<T> where T: class, new()
    {
        public MySqlWhereClauseBuilder(IObjectMappingResolver mappingResolver) : base(mappingResolver) { }

        public MySqlWhereClauseBuilder() : base() { }

        protected internal override char ParameterChar
        {
            get 
            {
                return '?';
            }
        }
    }
}
