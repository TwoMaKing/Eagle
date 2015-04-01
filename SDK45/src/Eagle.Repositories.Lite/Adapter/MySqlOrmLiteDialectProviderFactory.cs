using ServiceStack.OrmLite;
using ServiceStack.OrmLite.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Repositories.Lite
{
    public class MySqlOrmLiteDialectProviderFactory : IOrmLiteDialectProviderFactory
    {
        public IOrmLiteDialectProvider DialectProvider
        {
            get 
            {
                return MySqlDialectProvider.Instance;
            }
        }
    }
}
