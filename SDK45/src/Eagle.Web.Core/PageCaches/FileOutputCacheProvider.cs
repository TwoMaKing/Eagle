using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Eagle.Web.Core.PageCaches
{
    public class FileOutputCacheProvider : OutputCacheProvider
    {
        private string CachePath { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            if (name == null || name.Length == 0)
            {
                name = "FileOutputCacheProvider";
            }

            if (config["path"] == null)
            {
                string physicalCahcePath = HttpContext.Current.Server.MapPath("~/OutputCache");
                config.Remove("path");
                config.Add("path", physicalCahcePath);
            }

            base.Initialize(name, config);

            if (config["path"] != null)
            {
                this.CachePath = HttpContext.Current.Server.MapPath(config["path"]);
            }
        }

        public override object Add(string key, object entry, DateTime utcExpiry)
        {
            Object obj = this.Get(key);
            
            if (obj != null)    //这一步很重要
            {
                return obj;
            }

            Set(key, entry, utcExpiry);
            return entry;
        }

        public override object Get(string key)
        {
            string filePath = this.GetCacheFileName(key);

            if (!File.Exists(filePath))
            {
                return null;
            }

            try
            {
                CacheItem cacheItem = null;

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    cacheItem = (CacheItem)formatter.Deserialize(fs);
                }

                if (cacheItem.ExpiryDate <= DateTime.Now.ToUniversalTime())
                {
                    this.Remove(key);

                    return null;
                }

                return cacheItem.Item;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public override void Remove(string key)
        {
            string filePath = this.GetCacheFileName(key);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }

        public override void Set(string key, object entry, DateTime utcExpiry)
        {
            CacheItem cacheItem = new CacheItem(entry, utcExpiry);

            string filePath = this.GetCacheFileName(key);

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, cacheItem);
            }
        }

        private string GetCacheFileName(string key) 
        {
            string name = key.Replace("/", "-").Replace("\\", "-") + ".txt";

            return Path.Combine(this.CachePath, name);
        }
    }
}
