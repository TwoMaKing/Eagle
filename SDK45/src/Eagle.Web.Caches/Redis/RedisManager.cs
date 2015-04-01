using Eagle.Common.Serialization;
using Eagle.Core.Application;
using ServiceStack.Caching;
using ServiceStack.Data;
using ServiceStack.Model;
using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using ServiceStack.Redis.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Web.Caches
{
    public class RedisManager : ICacheManager
    {
        private static PooledRedisClientManager redisClientManager;

        private static RedisClient redisClient;

        private int expire = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.TimeOutSeconds;

        private static MethodInfo replaceMethod;

        static RedisManager()
        {
            CreateRedisClient();
        }

        private static void CreateRedisClient() 
        {
            string writeServerList = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.WriteHosts;
            string readOnlyServerList = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.ReadOnlyHosts;

            string[] writeHosts = writeServerList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            string[] readOnlyHosts = readOnlyServerList.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            RedisClientManagerConfig config = new RedisClientManagerConfig();
            config.MaxWritePoolSize = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.MaxWritePoolSize;
            config.MaxReadPoolSize = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.MaxReadPoolSize;
            config.AutoStart = AppRuntime.Instance.CurrentApplication.ConfigSource.Config.Redis.AutoStart;

            redisClientManager = new PooledRedisClientManager(writeHosts, readOnlyHosts, config);

            replaceMethod = typeof(RedisClient).GetMethod("Replace", BindingFlags.Instance | BindingFlags.Public);

            redisClient = (RedisClient)redisClientManager.GetClient();
        }

        public void AddItem(string key, object item)
        {
            this.AddItem(key, item, this.expire);
        }

        public void AddItem(string key, object item, int expire)
        {
            byte[] objectBytes = SerializationManager.SerializeToBinary(item);

            redisClient.Set(key, objectBytes, DateTime.Now.AddSeconds(expire));
        }

        public void AddItem<T>(string key, T item)
        {
            this.AddItem<T>(key, item, this.expire);
        }

        public void AddItem<T>(string key, T item, int expire)
        {
            redisClient.Set<T>(key, item, DateTime.Now.AddSeconds(expire));
        }

        public void Replace(string key, object item)
        {
            Type itemType = item.GetType();

            MethodInfo genericReplaceMethod = replaceMethod.MakeGenericMethod(itemType);

            genericReplaceMethod.Invoke(redisClient, new object[] { key, item });
        }

        public void Replace<T>(string key, T item)
        {
            redisClient.Replace<T>(key, item);
        }

        public void Replace(string key, object item, int expire)
        {
            Type itemType = item.GetType();

            MethodInfo genericReplaceMethod = replaceMethod.MakeGenericMethod(itemType);

            genericReplaceMethod.Invoke(redisClient, new object[] { key, item, DateTime.Now.AddSeconds(expire) });
        }

        public void Replace<T>(string key, T item, int expire)
        {
            redisClient.Replace<T>(key, item, DateTime.Now.AddSeconds(expire));
        }

        public bool ContainsKey(string key)
        {
            return redisClient.ContainsKey(key);
        }

        public object GetItem(string key)
        {
            byte[] objectBytes = redisClient.Get(key);

            return SerializationManager.DeserializeFromBinary(objectBytes);
        }

        public T GetItem<T>(string key)
        {
            return redisClient.Get<T>(key);
        }

        public void RemoveItem(string key)
        {
            redisClient.Remove(key);
        }

        public void FlushAll()
        {
            redisClient.FlushAll();
        }

        public void Dispose()
        {
            redisClient.Dispose();
        }
    }
}
