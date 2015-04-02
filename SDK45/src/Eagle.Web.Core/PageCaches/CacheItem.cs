using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Core.PageCaches
{
    [Serializable()]
    public class CacheItem
    {
        public CacheItem(object entry, DateTime utcExpiry)
        {
            this.Item = entry;
            this.ExpiryDate = utcExpiry;
        }

        public DateTime ExpiryDate { get; set; }

        public object Item { get; set; }
    }
}
