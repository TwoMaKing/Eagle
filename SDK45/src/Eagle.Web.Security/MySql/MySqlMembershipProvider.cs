using Dappers;
using Eagle.Common.Utils;
using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using System.Web.WebPages;

namespace Eagle.Web.Security
{
    public class MySqlMembershipProvider : RdbmsMembershipProvider 
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            if (name.IsEmpty())
            {
                name = "MySqlMembershipProvider";
            }

            base.Initialize(name, config);
        }

        public override AccountDbProvider CreateAccountDbProvider()
        {
            return new MySqlAccountDbHelper(this.ConnectionString);
        }
    }

}
