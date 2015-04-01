using Dappers;
using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using Eagle.Core.Extensions;
using Eagle.Core.IoC;
using Eagle.Domain.Repositories;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Eagle.Repositories.Dapper
{
    internal class CommandSqlParameters
    {
        public string CommandSql { get; set; }

        public object Parameters { get; set; }
    }

    public class DapperRepositoryContext : RepositoryContext, IDapperRepositoryContext
    {
        private string providerName;

        private string connectionString;

        private IDbTransaction dbTransaction = null;

        private List<CommandDefinition> commandDefinitionList = new List<CommandDefinition>();

        private static readonly object lockObject = new object();

        public DapperRepositoryContext() : this(ConfigurationManager.ConnectionStrings[0].Name) { }

        public DapperRepositoryContext(string connectionStringSectionName)
        {
            ConnectionStringSettings connectionStringSetting = ConfigurationManager.ConnectionStrings[connectionStringSectionName];

            if (connectionStringSetting == null)
            {
                throw new ConfigException("The connection string section {0} has not been configured yet.", connectionStringSectionName);
            }

            if (!connectionStringSetting.ProviderName.HasValue())
            {
                throw new ConfigException("The provider name has not been specified in the connection string settings section yet.");
            }

            if (!connectionStringSetting.ConnectionString.HasValue())
            {
                throw new ConfigException("The connection string has not been specified in the connection string settings section yet.");
            }

            this.providerName = connectionStringSetting.ProviderName;

            this.connectionString = connectionStringSetting.ConnectionString;
        }

        public DapperRepositoryContext(string providerName, string connectionString)
        {
            if (!providerName.HasValue())
            {
                throw new ConfigException("The provider name has not been specified in the connection string settings section yet.");
            }

            if (!connectionString.HasValue())
            {
                throw new ConfigException("The connection string has not been specified in the connection string settings section yet.");
            }

            this.providerName = providerName;

            this.connectionString = connectionString;
        }

        public override void RegisterAdded(object obj)
        {
            AddCommandDefinition((CommandSqlParameters)obj);
        }

        public override void RegisterModified(object obj)
        {
            AddCommandDefinition((CommandSqlParameters)obj);
        }

        public override void RegisterDeleted(object obj)
        {
            AddCommandDefinition((CommandSqlParameters)obj);
        }

        protected override void DoCommit()
        {
            lock (lockObject)
            {
                using (IDbConnection dbConnection = (this.dbTransaction != null &&
                                                     this.dbTransaction.Connection != null &&
                                                     this.dbTransaction.Connection.State == ConnectionState.Open) ?
                                                     this.dbTransaction.Connection :
                                                     this.CreateConnection())
                {
                    using (this.dbTransaction = this.BeginTransaction(dbConnection))
                    {
                        try
                        {
                            if (this.commandDefinitionList != null &&
                                this.commandDefinitionList.Count > 0)
                            {
                                for (int commandIndex = 0; commandIndex < this.commandDefinitionList.Count; commandIndex++)
                                {
                                    CommandDefinition commandDefinition = this.commandDefinitionList[commandIndex];
                                    dbConnection.Execute(commandDefinition);
                                }
                            }

                            this.dbTransaction.Commit();
                        }
                        catch (Exception e)
                        {
                            this.dbTransaction.Rollback();

                            throw e;
                        }
                        finally
                        {
                            this.commandDefinitionList.Clear();
                            this.CloseConnection(dbConnection);
                        }
                    }
                }
            }
        }

        protected override void DoRollback()
        {
            this.commandDefinitionList.Clear();
            this.Committed = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.commandDefinitionList.Clear();
                this.dbTransaction = null;
            }

            base.Dispose(disposing);
        }

        protected override IRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            IEnumerable<Type> repositoryTypesMapTo =
                AppRuntime.Instance.CurrentApplication.ObjectContainer.TypesMapTo.Where(t => typeof(DapperRepository<TAggregateRoot>).IsAssignableFrom(t));

            Type repositoryType = repositoryTypesMapTo.FirstOrDefault();

            ConstructorInfo constructorInfo = repositoryType.GetConstructor(new Type[] { typeof(IRepositoryContext) });
            
            if (constructorInfo == null)
            {
                throw new InfrastructureException("The parameter of the Repository constructor must be an instance of IRepositoryContext.");
            }

            return (IRepository<TAggregateRoot>)
                    AppRuntime.Instance.CurrentApplication.
                    ObjectContainer.Resolve(repositoryType,
                                            new ResolverParameter(constructorInfo.GetParameters()[0].Name, this));
        }

        private void AddCommandDefinition(CommandSqlParameters commandSqlParams)
        {
            lock (lockObject)
            {
                CommandDefinition commandDefinition = new CommandDefinition(commandSqlParams.CommandSql,
                                                                            commandSqlParams.Parameters,
                                                                            this.DbTransaction,
                                                                            null,
                                                                            CommandType.Text,
                                                                            CommandFlags.Buffered);

                this.commandDefinitionList.Add(commandDefinition);
            }
        }

        #region IDapperRepositoryContext members

        public IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = DbProviderFactories.GetFactory(this.providerName).CreateConnection();
            dbConnection.ConnectionString = this.connectionString;
            
            return dbConnection;
        }

        public IDbTransaction BeginTransaction(IDbConnection dbConnection = null, IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            if (this.dbTransaction != null &&
                this.dbTransaction.Connection != null &&
                this.dbTransaction.Connection.State == ConnectionState.Open)
            {
                return this.dbTransaction;
            }

            if (dbConnection == null)
            {
                dbConnection = this.CreateConnection();
            }

            try
            {
                if (dbConnection.State != ConnectionState.Open)
                {
                    dbConnection.Open();
                }
            }
            catch
            {
                try
                {
                    dbConnection.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return dbConnection.BeginTransaction(il);
        }

        public void CloseConnection(IDbConnection dbConnection)
        {
            if (dbConnection != null &&
                dbConnection.State != System.Data.ConnectionState.Closed)
            {
                dbConnection.Close();
            }
        }

        public IDbTransaction DbTransaction
        {
            get
            {
                this.dbTransaction = this.BeginTransaction();

                return this.dbTransaction;
            }
        }

        public IEnumerable<T> Query<T>(string querySql, object parameters, IDbTransaction dbTransaction = null, CommandType commandType = CommandType.Text)
        {
            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    IEnumerable<T> objects = dbConnection.Query<T>(querySql, parameters, commandType);

                    this.CloseConnection(dbConnection);

                    return objects;
                }
            }
            else
            {
                return dbTransaction.Connection.Query<T>(querySql, parameters, dbTransaction, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string querySql, 
                                                                    Func<TFirst, TSecond, TReturn> map, 
                                                                    object parameters, 
                                                                    IDbTransaction dbTransaction = null, 
                                                                    string splitOnColumns = "Id", 
                                                                    CommandType commandType = CommandType.Text)
        {
            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    IEnumerable<TReturn> objects = dbConnection.Query<TFirst, TSecond, TReturn>(querySql, map, parameters, null, true, splitOnColumns, null, commandType);

                    this.CloseConnection(dbConnection);

                    return objects;
                }
            }
            else
            {
                return dbTransaction.Connection.Query<TFirst, TSecond, TReturn>(querySql, map, parameters, dbTransaction, true, splitOnColumns, null, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string querySql, 
                                                                            Func<TFirst, TSecond, TThird, TReturn> map, 
                                                                            object parameters, 
                                                                            IDbTransaction dbTransaction = null, 
                                                                            string splitOnColumns = "Id", 
                                                                            CommandType commandType = CommandType.Text)
        {
            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    IEnumerable<TReturn> objects = dbConnection.Query<TFirst, TSecond, TThird, TReturn>(querySql, map, parameters, null, true, splitOnColumns, null, commandType);

                    this.CloseConnection(dbConnection);

                    return objects;
                }
            }
            else
            {
                return dbTransaction.Connection.Query<TFirst, TSecond, TThird, TReturn>(querySql, map, parameters, dbTransaction, true, splitOnColumns, null, commandType);
            }
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string querySql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object parameters, IDbTransaction dbTransaction = null, string splitOnColumns = "Id", CommandType commandType = CommandType.Text)
        {
            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    IEnumerable<TReturn> objects = dbConnection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(querySql, map, parameters, null, true, splitOnColumns, null, commandType);

                    this.CloseConnection(dbConnection);

                    return objects;
                }
            }
            else
            {
                return dbTransaction.Connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(querySql, map, parameters, dbTransaction, true, splitOnColumns, null, commandType);
            }
        }

        public SqlMapper.GridReader QueryMultiple(string querySql, object parameters, IDbTransaction dbTransaction = null, CommandType commandType = CommandType.Text)
        {
            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    SqlMapper.GridReader gridReader = dbConnection.QueryMultiple(querySql, parameters, commandType);

                    return gridReader;
                }
            }
            else
            {
                return dbTransaction.Connection.QueryMultiple(querySql, parameters, dbTransaction, commandType);
            }
        }

        public IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TReturn>(string querySql, 
                                                                            Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TReturn>> map,
                                                                            object parameters, 
                                                                            IDbTransaction dbTransaction = null, 
                                                                            CommandType commandType = CommandType.Text)
        {
            SqlMapper.GridReader gridReader;

            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    gridReader = dbConnection.QueryMultiple(querySql, parameters, commandType);

                    IEnumerable<TReturn> returnObjects = map(gridReader.Read<TFirst>(), gridReader.Read<TSecond>());

                    this.CloseConnection(dbConnection);

                    return returnObjects;
                }
            }
            else
            {
                gridReader = dbTransaction.Connection.QueryMultiple(querySql, parameters, dbTransaction, commandType);
                
                return map(gridReader.Read<TFirst>(), gridReader.Read<TSecond>());
            }
        }

        public IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TThird, TReturn>(string querySql,
                                                                                    Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TReturn>> map, 
                                                                                    object parameters, 
                                                                                    IDbTransaction dbTransaction = null, 
                                                                                    CommandType commandType = CommandType.Text)
        {
            SqlMapper.GridReader gridReader;

            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    gridReader = dbConnection.QueryMultiple(querySql, parameters, commandType);

                    IEnumerable<TReturn> returnObjects = map(gridReader.Read<TFirst>(), gridReader.Read<TSecond>(), gridReader.Read<TThird>());

                    this.CloseConnection(dbConnection);

                    return returnObjects;
                }
            }
            else
            {
                gridReader = dbTransaction.Connection.QueryMultiple(querySql, parameters, dbTransaction, commandType);

                return map(gridReader.Read<TFirst>(), gridReader.Read<TSecond>(), gridReader.Read<TThird>());
            }
        }

        public IEnumerable<TReturn> QueryMultiple<TFirst, TSecond, TThird, TFourth, TReturn>(string querySql,
                                                                                             Func<IEnumerable<TFirst>, IEnumerable<TSecond>, IEnumerable<TThird>, IEnumerable<TFourth>, IEnumerable<TReturn>> map,  
                                                                                             object parameters, 
                                                                                             IDbTransaction dbTransaction = null, 
                                                                                             CommandType commandType = CommandType.Text)
        {
            SqlMapper.GridReader gridReader;

            if (dbTransaction == null)
            {
                using (IDbConnection dbConnection = this.CreateConnection())
                {
                    gridReader = dbConnection.QueryMultiple(querySql, parameters, commandType);

                    IEnumerable<TReturn> returnObjects = map(gridReader.Read<TFirst>(), gridReader.Read<TSecond>(), gridReader.Read<TThird>(), gridReader.Read<TFourth>());

                    this.CloseConnection(dbConnection);

                    return returnObjects;
                }
            }
            else
            {
                gridReader = dbTransaction.Connection.QueryMultiple(querySql, parameters, dbTransaction, commandType);

                return map(gridReader.Read<TFirst>(), gridReader.Read<TSecond>(), gridReader.Read<TThird>(), gridReader.Read<TFourth>());
            }
        }

        public void ExecuteNonQuery(string commandSql, object parameters)
        {
            lock (lockObject)
            {
                CommandDefinition commandDefinition = new CommandDefinition(commandSql, 
                                                                            parameters, 
                                                                            this.DbTransaction, 
                                                                            null, 
                                                                            CommandType.Text, 
                                                                            CommandFlags.Buffered);

                this.commandDefinitionList.Add(commandDefinition);
            }
        }

        public void ExecuteStoredProcedure(string storedProcedureName, object parameters)
        {
            lock (lockObject)
            {
                CommandDefinition commandDefinition = new CommandDefinition(storedProcedureName, 
                                                                            parameters, 
                                                                            this.DbTransaction, 
                                                                            null, 
                                                                            CommandType.StoredProcedure, 
                                                                            CommandFlags.Buffered);

                this.commandDefinitionList.Add(commandDefinition);
            }
        }

        #endregion
    }
}
