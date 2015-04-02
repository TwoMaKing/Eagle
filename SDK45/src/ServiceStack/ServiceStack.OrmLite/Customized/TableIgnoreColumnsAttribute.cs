using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStack.OrmLite
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TableIgnoreColumnsAttribute : AttributeBase
    {
        public TableIgnoreColumnsAttribute(string[] columns)
        {
            this.Columns = columns;
        }

        public string[] Columns
        {
            get;
            private set;
        }
    }
}
