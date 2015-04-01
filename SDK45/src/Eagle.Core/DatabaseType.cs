using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Core
{
    public enum DatabaseType
    {
        /// <summary>
        /// MS SQL Server
        /// </summary>
        SqlServer = 0,

        /// <summary>
        /// Oracle
        /// </summary>
        Oracle = 1,

        /// <summary>
        /// MySql
        /// </summary>
        MySql = 2,

        /// <summary>
        /// Sql Lite
        /// </summary>
        SqlLite = 3
    }
}
