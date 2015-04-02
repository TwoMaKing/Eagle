using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Repositories.Lite
{
    public static class OrmLiteDialectProviderFactories
    {
        private static Dictionary<string, IOrmLiteDialectProviderFactory> providerFactoryCache =
            new Dictionary<string, IOrmLiteDialectProviderFactory>();

        private static IOrmLiteDialectProviderFactory defaultDialectProviderFactory;

        private static readonly object lockObject = new object();

        public static IOrmLiteDialectProviderFactory Default 
        {
            get 
            {
                if (defaultDialectProviderFactory == null)
                {
                    defaultDialectProviderFactory = CreateDefaultOrmLiteDialectProviderFactory();
                }

                return defaultDialectProviderFactory;
            }
        }

        public static IOrmLiteDialectProviderFactory CreateOrmLiteDialectProviderFactory(string providerName, params object[] args)
        {
            lock (lockObject)
            {
                if (providerFactoryCache.ContainsKey(providerName))
                {
                    return providerFactoryCache[providerName];
                }
                else
                {
                    Type dialectProviderFactoryType = Type.GetType(providerName);

                    if (dialectProviderFactoryType == null)
                    {
                        throw new InfrastructureException("The OrmLiteDialectProviderFactory defined by type {0} doesn't exist.", dialectProviderFactoryType.Name);
                    }

                    IOrmLiteDialectProviderFactory providerFactory = 
                        (IOrmLiteDialectProviderFactory)Activator.CreateInstance(dialectProviderFactoryType, args);

                    if (!providerFactoryCache.ContainsKey(providerName))
                    {
                        providerFactoryCache.Add(providerName, providerFactory);
                    }

                    return providerFactory;
                }
            }
        }

        private static IOrmLiteDialectProviderFactory CreateDefaultOrmLiteDialectProviderFactory()
        {
            if (ConfigurationManager.ConnectionStrings.Count == 0)
            {
                throw new ConfigException(@"There is no connection string configured in the current 
                                            application's connection string configuration. 
                                            please provide a connection string at least.");
            }

            string providerName = ConfigurationManager.ConnectionStrings[0].ProviderName;

            return CreateOrmLiteDialectProviderFactory(providerName);
        }
    }
}
