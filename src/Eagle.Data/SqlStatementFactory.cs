﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Data
{
    public abstract class SqlStatementFactory : ISqlStatementFactory
    {
        private DbProvider dbProvider;

        public SqlStatementFactory(DbProvider dbProvider) 
        {
            this.dbProvider = dbProvider;
        }

        protected DbProvider DbProvider 
        {
            get 
            {
                return this.dbProvider;
            }
        }

        public abstract string CreateInsertStatement(string tableName, string[] includedColumns);

        public abstract string CreateUpdateStatement(string tableName, string where, string[] includedColumns);

        public abstract string CreateDeleteStatement(string tableName, string where);

        public abstract string CreateSelectStatement(string tableName, string where, string orderBy, params string[] includedColumns);

        public virtual string CreateSelectRangeStatement(string tableName, 
                                                         string where, 
                                                         string orderBy, 
                                                         int topCount, 
                                                         int skipCount, 
                                                         string identityColumn, 
                                                         bool identityColumnIsNumber = true,
                                                         string groupBy = null,
                                                         params string[] includedColumns)
        {
            if (includedColumns == null)
            {
                includedColumns = new string[] { "*" };
            }

            if (topCount == int.MaxValue && 
                skipCount == 0)
            {
                return CreateSelectStatement(tableName, where, orderBy, includedColumns);
            }
            else if (skipCount == 0)
            {
                return CreateSelectTopStatement(tableName, where, includedColumns, orderBy,  groupBy, topCount);
            }
            else
            {
                if (identityColumnIsNumber && 
                    SqlQueryUtils.OrderByStartsWith(orderBy, identityColumn) && 
                   (string.IsNullOrEmpty(groupBy) || 
                    groupBy.Equals(identityColumn, StringComparison.InvariantCultureIgnoreCase)))
                {
                    return CreateSelectRangeStatementForSortedRows(tableName, where, includedColumns, orderBy, groupBy, 
                        topCount, skipCount, identityColumn, SqlQueryUtils.OrderByStartsWith(orderBy, identityColumn + " DESC"));
                }
                else
                {
                    return CreateSelectRangeStatementForUnsortedRows(tableName, where, includedColumns, orderBy, groupBy, topCount, skipCount, identityColumn);
                }
            }
        }

        protected abstract string CreateSelectTopStatement(string tableName, string where, string[] columns, string orderBy, string groupBy, int topCount);

        protected abstract string CreateSelectRangeStatementForSortedRows(string tableName, string where, string[] columns, string orderBy, string groupBy, int topCount, int skipCount, string identityColumn, bool isIdentityColumnDesc);

        protected abstract string CreateSelectRangeStatementForUnsortedRows(string tableName, string where, string[] columns, string orderBy, string groupBy, int topCount, int skipCount, string identyColumn);

    }
}
