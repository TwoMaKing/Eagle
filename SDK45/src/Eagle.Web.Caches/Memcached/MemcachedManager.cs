using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Caches
{
    public class MemcachedManager : ICacheManager
    {
        private static MemcachedClient memcachedClient = new MemcachedClient("enyim.com/memcached");  

        public void AddItem(string key, object item)
        {
            memcachedClient.Store(StoreMode.Add, key, item);
        }

        public void AddItem(string key, object item, int expire)
        {
            memcachedClient.Store(StoreMode.Add, key, item, DateTime.Now.AddSeconds(expire));
        }

        public void AddItem<T>(string key, T item)
        {
            memcachedClient.Store(StoreMode.Add, key, item);
        }

        public void AddItem<T>(string key, T item, int expire)
        {
            memcachedClient.Store(StoreMode.Add, key, item, DateTime.Now.AddSeconds(expire));
        }

        public void Replace(string key, object item)
        {
            memcachedClient.Store(StoreMode.Replace, key, item);
        }

        public void Replace<T>(string key, T item)
        {
            memcachedClient.Store(StoreMode.Replace, key, item);
        }

        public void Replace(string key, object item, int expire)
        {
            memcachedClient.Store(StoreMode.Replace, key, item, DateTime.Now.AddSeconds(expire));
        }

        public void Replace<T>(string key, T item, int expire)
        {
            memcachedClient.Store(StoreMode.Replace, key, item, DateTime.Now.AddSeconds(expire));
        }

        public bool ContainsKey(string key)
        {
            return memcachedClient.CheckAndSet(key, new object(), 1);
        }

        public object GetItem(string key)
        {
            return memcachedClient.Get(key);
        }

        public T GetItem<T>(string key)
        {
            return memcachedClient.Get<T>(key);
        }

        public void RemoveItem(string key)
        {
            memcachedClient.Remove(key);
        }

        public void FlushAll()
        {
            memcachedClient.FlushAll();
        }

        public void Dispose()
        {
            memcachedClient.Dispose();
        }
    }
}
