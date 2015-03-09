using Eagle.Core.Application;
using Eagle.Domain.Repositories;
using Microsoft.Practices.Unity;
using MongoDB;
using MongoDB.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Repositories.MongoDB
{
    public class MongoDBRepositoryContext : RepositoryContext, IMongoDBRepositoryContext
    {
        private Mongo mongo;

        private string databaseName;

        public MongoDBRepositoryContext(string connectionString, string databaseName) 
        {
            this.databaseName = databaseName;

            this.mongo = new Mongo(connectionString);

            this.mongo.Connect();
        }

        public IMongoDatabase MongoDatabase
        {
            get 
            {
                return this.mongo[this.databaseName];
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.mongo != null)
                {
                    this.mongo.Disconnect();
                    this.mongo.Dispose();
                }
            }
        }

        protected override void DoCommit()
        {
            this.mongo.Disconnect();

            this.mongo.Dispose();
        }

        protected override void DoRollback()
        {
            base.Rollback();
        }

        protected override IRepository<TAggregateRoot> CreateRepository<TAggregateRoot>()
        {
            IEnumerable<Type> repositoryTypesMapTo =
                AppRuntime.Instance.CurrentApp.ObjectContainer.TypesMapTo.Where(t => typeof(MongoDBRepository<TAggregateRoot>).IsAssignableFrom(t));

            Type repositoryType = repositoryTypesMapTo.FirstOrDefault();

            IUnityContainer unityContainer = AppRuntime.Instance.CurrentApp.ObjectContainer.GetWrapperContainer<IUnityContainer>();

            return (IRepository<TAggregateRoot>)unityContainer.Resolve(repositoryType, new DependencyOverride<IRepositoryContext>(this));
        }
    }
}
