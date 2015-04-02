using Dappers;
using Eagle.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Eagle.Repositories.Dapper
{
    public interface IDapperRepositoryContext : IRepositoryContext
    {
        IDbConnection CreateConnection();

        IDbTransaction BeginTransaction(IDbConnection dbConnection = null, IsolationLevel il = IsolationLevel.ReadCommitted);

        void CloseConnection(IDbConnection dbConnection);

        IDbTransaction DbTransaction { get; }

        IEnumerable<T> Query<T>(string querySql, object parameters, IDbTransaction dbTransaction = null, CommandType commandType = CommandType.Text);

        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string querySql, 
                                                             Func<TFirst, TSecond, TReturn> map, 
                                                             object parameters, 
                                                             IDbTransaction dbTransaction = null, 
                                                             string splitOnColumns = "Id",
                                                             CommandType commandType = CommandType.Text);
        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string querySql, 
                                                                     Func<TFirst, TSecond, TThird, TReturn> map,
                                                                     object parameters,
                                                                     IDbTransaction dbTransaction = null,
                                                                     string splitOnColumns = "Id",
                                                                     CommandType commandType = CommandType.Text);

        IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string querySql,
                                                                              Func<TFirst, TSecond, TThird, TFourth, TReturn> map,
                                                                              object parameters,
                                                                              IDbTransaction dbTransaction = null,
                                                                              string splitOnColumns = "Id",
                                                                              CommandType commandType = CommandType.Text);
        SqlMapper.GridReader QueryMultiple(string querySql, object parameters, IDbTransaction dbTransaction = null, CommandType commandType = CommandType.Text);

        IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TReturn>(string querySql,
                                                                     Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TReturn>> map,
                                                                     object parameters,
                                                                     IDbTransaction dbTransaction = null,
                                                                     CommandType commandType = CommandType.Text);
        IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TThird, TReturn>(string querySql,
                                                                             Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TReturn>> map,
                                                                             object parameters,
                                                                             IDbTransaction dbTransaction = null,
                                                                             CommandType commandType = CommandType.Text);

        IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TThird, TFourth, TReturn>(string querySql,
                                                                                      Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TReturn>> map,
                                                                                      object parameters,
                                                                                      IDbTransaction dbTransaction = null,
                                                                                      CommandType commandType = CommandType.Text);

        void ExecuteNonQuery(string commandSql, object parameters);

        void ExecuteStoredProcedure(string procedureName, object parameters);
    }
}
