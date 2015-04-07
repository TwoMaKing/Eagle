using Dappers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace Eagle.Web.Security
{
    internal sealed class MySqlAccountDbHelper : AccountDbProvider
    {
        public MySqlAccountDbHelper(string connectionString) : base(connectionString) { }

        public override string SelectLastInsertedRowAutoIDStatement
        {
            get 
            {
                return "LAST_INSERT_ID()";
            }
        }

        public override char ParameterPrefix
        {
            get 
            {
                return '?';
            }
        }

        public override char ParameterLeftToken
        {
            get 
            {
                return '`';
            }
        }

        public override char ParameterRightToken
        {
            get 
            {
                return '`';
            }
        }

        public override char WildCharToken
        {
            get 
            {
                return '%';
            }
        }

        public override char WildSingleCharToken
        {
            get 
            {
                return '_';
            }
        }

        public override string CurrentDateSqlStatement
        {
            get 
            {
                return "NOW()";
            }
        }

        protected override IDbConnection CreateDbConnection()
        {
            return new MySqlConnection(this.ConnectionString);
        }
    }
}
