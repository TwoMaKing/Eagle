using ServiceStack.OrmLite;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Repositories.Lite
{
    public class SqlServerOrmLiteDialectProviderFactory : IOrmLiteDialectProviderFactory
    {
        public IOrmLiteDialectProvider DialectProvider
        {
            get 
            {
                return SqlServerOrmLiteDialectProvider.Instance;
            }
        }
    }
}
