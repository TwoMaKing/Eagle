using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStack.OrmLite
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TablePrimaryKeyAttribute : AttributeBase
    {
        public TablePrimaryKeyAttribute(string primaryKey)
        {
            this.PrimaryKey = primaryKey;
            this.ColumnName = primaryKey;
            this.AutoIncrement = true;
        }

        public string PrimaryKey { get; private set; }

        public string ColumnName { get; set; }

        public bool AutoIncrement { get; set; }
        
        public string SequenceName { get; set; }

        public Type[] ReferenceTypes { get; set; }
    }
}
