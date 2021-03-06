﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using Eagle.Core.Exceptions;


namespace Eagle.Data
{

    /// <summary>
    /// The db provider factory.
    /// </summary>
    /// <remarks></remarks>
    public sealed class DbProviderFactory
    {
        #region "private Memeber"

        private static DbProvider defaultDbProvider;

        private static Dictionary<string, DbProvider> providerCache = new Dictionary<string, DbProvider>();

        private static readonly object lockObject = new object();

        private DbProviderFactory() { }

        #endregion

        /// <summary>
        /// Creates the db provider.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="classTypeName">Name of the class.</param>
        /// <param name="connectionString">The conn STR.</param>
        /// <returns>The db provider.</returns>
        public static DbProvider CreateDbProvider(string assemblyName, string classTypeName, string connectionString)
        {
            lock (lockObject)
            {
                string cacheKey = string.Concat(assemblyName, classTypeName, connectionString);

                if (providerCache.ContainsKey(cacheKey))
                {
                    return providerCache[cacheKey];
                }
                else
                {
                    Assembly assembly = null;

                    if (string.IsNullOrEmpty(assemblyName))
                    {
                        assembly = typeof(DbProvider).Assembly;
                    }
                    else
                    {
                        assembly = Assembly.Load(assemblyName);
                    }

                    DbProvider dbProvider = (DbProvider)assembly.CreateInstance(
                        classTypeName, true, BindingFlags.Default, null, new object[] { connectionString }, null, null);

                    if (!providerCache.ContainsKey(cacheKey))
                    {
                        providerCache.Add(cacheKey, dbProvider);
                    }

                    return dbProvider;
                }
            }
        }

        /// <summary>
        /// Create the specified DbProvider instance by the name of connection string.
        /// </summary>
        /// <param name="connectionStringSectionName"></param>
        /// <returns></returns>
        public static DbProvider CreateDbProvider(string connectionStringSectionName)
        {
            ConnectionStringSettings connStrSetting = ConfigurationManager.ConnectionStrings[connectionStringSectionName];

            string connectionString = connStrSetting.ConnectionString;
            
            string providerName = connStrSetting.ProviderName;

            string[] assAndClass = providerName.Split(new char[] { ',' });

            try
            {
                if (assAndClass.Length.Equals(2))
                {
                    return CreateDbProvider(assAndClass[1].Trim(), assAndClass[0].Trim(), connectionString);
                }
                else
                {
                    return CreateDbProvider(string.Empty, providerName, connectionString);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static DbProvider CreateDefaultDbProvider() 
        {
            if (ConfigurationManager.ConnectionStrings == null ||
                ConfigurationManager.ConnectionStrings.Count.Equals(0))
            {
                throw new ConfigException("Please provide a connection string including name, provider and connection string.");
            }

            ConnectionStringSettings connStrSetting = ConfigurationManager.ConnectionStrings[0];

            string connectionString = connStrSetting.ConnectionString;

            string providerName = connStrSetting.ProviderName.Trim();

            string[] assAndClass = providerName.Split(new char[] { ',' });

            try
            {
                if (assAndClass.Length.Equals(2))
                {
                    return CreateDbProvider(assAndClass[1].Trim(), assAndClass[0].Trim(), connectionString);
                }
                else
                {
                    return CreateDbProvider(string.Empty, providerName, connectionString);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DbProvider Default
        {
            get
            {
                if (defaultDbProvider == null)
                {
                    defaultDbProvider = CreateDefaultDbProvider();
                }

                return defaultDbProvider;
            }
        }

    }

}
