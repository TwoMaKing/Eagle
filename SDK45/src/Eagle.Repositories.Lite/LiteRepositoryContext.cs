using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using Eagle.Core.Extensions;
using Eagle.Core.IoC;
using Eagle.Domain.Repositories;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.MySql;
using ServiceStack.OrmLite.SqlServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Repositories.Lite
{
    public class LiteRepositoryContext : RepositoryContext, ILiteRepositoryContext
    {
        private OrmLiteConnectionFactory liteConnectionFactory;

        public LiteRepositoryContext() : this(ConfigurationManager.ConnectionStrings[0].Name) { }

        public LiteRepositoryContext(string connectionStringSectionName)
        {
            var connectionStringSetting = ConfigurationManager.ConnectionStrings[connectionStringSectionName];

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

            string providerName = connectionStringSetting.ProviderName;

            IOrmLiteDialectProviderFactory providerFactory = OrmLiteDialectProviderFactories.CreateOrmLiteDialectProviderFactory(providerName);

            if (providerFactory == null)
            {
                throw new DatabaseException("The instance of IOrmLiteDialectProviderFactory is null. Please specify an valid OrmLiteDialectProviderFactory");
            }

            string connectionString = connectionStringSetting.ConnectionString;

            this.liteConnectionFactory = new OrmLiteConnectionFactory(connectionString, providerFactory.DialectProvider);
        }

        public LiteRepositoryContext(string connectionString, DatabaseType databaseType)
        {
            IOrmLiteDialectProvider dialectProvider = null;

            if (databaseType == DatabaseType.SqlServer)
            {
                dialectProvider = SqlServerOrmLiteDialectProvider.Instance;
            }
            else if (databaseType == DatabaseType.MySql)
            {
                dialectProvider = MySqlDialectProvider.Instance;
            }

            if (dialectProvider == null)
            {
                throw new DatabaseException("Please provide an valid instance of IOrmLiteDialectProvider.");
            }

            this.liteConnectionFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider);
        }

        public OrmLiteConnectionFactory LiteConnectionFactory
        {
            get 
            {
                return this.liteConnectionFactory;
            }
        }

        protected override void DoCommit()
        {
            using (IDbConnection dbConnection = this.LiteConnectionFactory.CreateDbConnection())
            {
                IDbTransaction dbTransaction = dbConnection.BeginTransaction();

                try
                {
                    if (this.AddedNewCollection != null &&
                        this.AddedNewCollection.Count() > 0)
                    {
                        foreach(object addedObj in this.AddedNewCollection)
                        {
                            dbConnection.Insert(addedObj);
                        }
                    }

                    if (this.ModifiedCollection != null &&
                        this.ModifiedCollection.Count() > 0)
                    {
                        foreach (object modifiedObj in this.ModifiedCollection)
                        {
                            dbConnection.Update(modifiedObj);
                        }
                    }

                    if (this.DeletedCollection != null &&
                        this.DeletedCollection.Count() > 0)
                    {
                        foreach (object deletedObj in this.DeletedCollection)
                        {
                            dbConnection.Update(deletedObj);
                        }
                    }

                    dbTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbTransaction.Rollback();

                    throw e;
                }
                finally
                {
                    if (dbConnection.State != ConnectionState.Closed)
                    {
                        dbConnection.Close();
                    }
                }
            }
        }

        protected override void DoRollback()
        {
            return;
        }

        protected override IRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            IEnumerable<Type> repositoryTypes = 
                AppRuntime.Instance.CurrentApplication.ObjectContainer.TypesMapTo.Where(t=> typeof(LiteRepository<TAggregateRoot>).IsAssignableFrom(t));

            Type repositoryType = repositoryTypes.FirstOrDefault();

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

    }
}
