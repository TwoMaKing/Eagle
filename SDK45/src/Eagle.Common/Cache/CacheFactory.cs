﻿using Eagle.Core.Application;
using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Common.Cache
{
    public sealed class CacheFactory
    {
        private static Dictionary<string, ICacheManager> cacheManagerDictionary = new Dictionary<string, ICacheManager>();

        private static readonly object lockObject = new object();

        private CacheFactory() { }

        public static ICacheManager GetCacheManager() 
        {
            string defaultCacheManager = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.CacheManagers.Default;

            if (string.IsNullOrEmpty(defaultCacheManager) ||
                string.IsNullOrWhiteSpace(defaultCacheManager))
            {
                defaultCacheManager = "Redis";
            }

            return GetCacheManager(defaultCacheManager);
        }

        public static ICacheManager GetCacheManager(string name)
        {
            string cacheManagerTypeName = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.CacheManagers[name].Type;

            if (cacheManagerDictionary.ContainsKey(cacheManagerTypeName))
            {
                return (ICacheManager)cacheManagerDictionary[cacheManagerTypeName];
            }

            if (string.IsNullOrEmpty(cacheManagerTypeName))
            {
                throw new ConfigException("The cache manager has not been defined in the ConfigSource.");
            }

            Type cacheManagerType = Type.GetType(cacheManagerTypeName);

            if (cacheManagerType == null)
            {
                throw new InfrastructureException("The Cache Manager defined by the type {0} doesn't exist.", cacheManagerTypeName);
            }

            if (!typeof(ICacheManager).IsAssignableFrom(cacheManagerType))
            {
                throw new ConfigException("Type '{0}' is not a Cache Manager.", cacheManagerType);
            }

            ICacheManager cacheManager;

            lock (lockObject)
            {
                if (!cacheManagerDictionary.ContainsKey(cacheManagerTypeName))
                {
                    cacheManager = (ICacheManager)AppRuntime.Instance.CurrentApplication.ObjectContainer.Resolve(cacheManagerType,
                                                                                                          cacheManagerTypeName);

                    cacheManagerDictionary.Add(cacheManagerTypeName, cacheManager);
                }
                else
                {
                    cacheManager = (ICacheManager)cacheManagerDictionary[cacheManagerTypeName];
                }
            }

            return cacheManager;
        }

    }
}
