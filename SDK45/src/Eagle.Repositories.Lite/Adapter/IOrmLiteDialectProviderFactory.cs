using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Repositories.Lite
{
    public interface IOrmLiteDialectProviderFactory
    {
        IOrmLiteDialectProvider DialectProvider { get; }
    }
}
