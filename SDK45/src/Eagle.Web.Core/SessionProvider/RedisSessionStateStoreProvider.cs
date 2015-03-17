using Eagle.Core.Exceptions;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.SessionState;


namespace Eagle.Web.Core.SessionProvider
{
    public class RedisSessionStateStoreProvider : SessionStateStoreProviderBase, IDisposable
    {
        private SessionStateSection sessionConfigSection;

        private string[] writeHosts;
        private string[] readOnlyHosts;

        private bool writeExceptionsToEventLog;

        public override void Initialize(string name, NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
            {
                name = "RedisSessionStateStore";
            }

            if (String.IsNullOrEmpty(config["writeHosts"]))
            {
                config.Remove("writeHosts");
                config.Add("writeHosts", "127.0.0.1:6379");
            }

            if (String.IsNullOrEmpty(config["readOnlyHosts"]))
            {
                config.Remove("readOnlyHosts");
                config.Add("readOnlyHosts", "127.0.0.1:6379");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            //
            // Initialize the ApplicationName property.
            //

            if (ConfigurationManager.AppSettings.AllKeys.Contains("ApplicationName"))
            {
                this.ApplicationName = ConfigurationManager.AppSettings["ApplicationName"];
            }
            else
            {
                this.ApplicationName = HostingEnvironment.ApplicationVirtualPath;
            }

            //
            // Get <sessionState> configuration element.
            //
            Configuration webConfig = WebConfigurationManager.OpenWebConfiguration(HostingEnvironment.ApplicationVirtualPath);

            this.sessionConfigSection = (SessionStateSection)webConfig.GetSection("system.web/sessionState");

            if (config["writeHosts"] != null)
            {
                this.writeHosts = config["writeHosts"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            if (config["readOnlyHosts"] != null)
            {
                this.readOnlyHosts = config["readOnlyHosts"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            }

            //
            // Initialize WriteExceptionsToEventLog
            //
            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                    writeExceptionsToEventLog = true;
            }
        }

        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            return new SessionStateStoreData(new SessionStateItemCollection(), 
                                             SessionStateUtility.GetSessionStaticObjects(context), 
                                             timeout);
        }

        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            using (IRedisClient redisClient = this.CreateRedisClient())
            {
                try
                {
                    SessionItem sessionItem = new SessionItem()
                    {
                        SessionId = id,
                        ApplicationName = this.ApplicationName,
                        CreatedAt = DateTime.Now,
                        Expires = DateTime.Now.AddMinutes((Double)timeout),
                        Timeout = timeout,
                        LockId = 0,
                        LockDate = DateTime.Now,
                        Locked = true,
                        SessionItems = string.Empty,
                        Flags = 1
                    };

                    redisClient.Set<SessionItem>(this.GetRedisKey(id), sessionItem, DateTime.Now.AddMinutes(timeout));
                }
                catch (Exception e) 
                {
                    throw e;
                }
            }
        }

        public override void EndRequest(HttpContext context)
        {
            this.Dispose();
        }

        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return this.GetSessionStoreItem(false, context, id, out locked,  out lockAge, out lockId, out actions);
        }

        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return this.GetSessionStoreItem(true, context, id, out locked, out lockAge, out lockId, out actions);
        }

        public override void InitializeRequest(HttpContext context) 
        { 
            // Nothing to do.
        }

        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            using (IRedisClient redisClient = this.CreateRedisClient())
            {
                SessionItem currentSessionItem = redisClient.Get<SessionItem>(this.GetRedisKey(id));

                if (currentSessionItem != null && currentSessionItem.LockId == (int?)lockId)
                {
                    currentSessionItem.Locked = false;
                    redisClient.Set<SessionItem>(this.GetRedisKey(id), currentSessionItem, DateTime.UtcNow.AddMinutes(this.sessionConfigSection.Timeout.TotalMinutes));
                }
            }
        }

        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            using (IRedisClient redisClient = this.CreateRedisClient())
            {
                // Remove the session by session id
                redisClient.Remove(this.GetRedisKey(id));
            }
        }

        public override void ResetItemTimeout(HttpContext context, string id)
        {
            using (IRedisClient redisClient = this.CreateRedisClient())
            {
                SessionItem currentSessionItem = redisClient.Get<SessionItem>(this.GetRedisKey(id));
                if (currentSessionItem != null)
                {
                    redisClient.ExpireEntryAt(this.GetRedisKey(id), DateTime.Now.AddMinutes(this.sessionConfigSection.Timeout.TotalMinutes));
                }
            }
        }

        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            // Serialize the SessionStateItemCollection as a string.
            string sessionItems = this.Serialize((SessionStateItemCollection)item.Items);

            using (IRedisClient redisClient = this.CreateRedisClient())
            {
                try
                {
                    if (newItem)
                    {
                        SessionItem currentSessionItem = redisClient.Get<SessionItem>(this.GetRedisKey(id));

                        if (currentSessionItem != null && currentSessionItem.Expires < DateTime.Now)
                        {
                            redisClient.Delete<SessionItem>(currentSessionItem);
                        }

                        SessionItem sessionItem = new SessionItem()
                        {
                            SessionId = id,
                            ApplicationName = this.ApplicationName,
                            CreatedAt = DateTime.Now,
                            Expires = DateTime.Now.AddMinutes((Double)item.Timeout),
                            Timeout = item.Timeout,
                            LockId = 0,
                            LockDate = DateTime.Now,
                            Locked = false,
                            SessionItems = sessionItems,
                            Flags = 0
                        };

                        redisClient.Set<SessionItem>(this.GetRedisKey(id), sessionItem, DateTime.Now.AddMinutes(item.Timeout));
                    }
                    else
                    {
                        SessionItem currentSessionItem = redisClient.Get<SessionItem>(this.GetRedisKey(id));

                        if (currentSessionItem != null && currentSessionItem.LockId == (int?)lockId)
                        {
                            currentSessionItem.Locked = false;
                            currentSessionItem.SessionItems = sessionItems;
                            redisClient.Set<SessionItem>(this.GetRedisKey(id), currentSessionItem, DateTime.UtcNow.AddMinutes(item.Timeout));
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
 
        }

        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            return true;
        }

        public override void Dispose() { }

        private string ApplicationName
        {
            get;
            set;
        }

        private string GetRedisKey(string id)
        {
            return string.Format("{0}{1}", !string.IsNullOrEmpty(this.ApplicationName) ? this.ApplicationName + ":" : string.Empty, id);
        }

        private IRedisClient CreateRedisClient()
        {
            var redisClientManager = new PooledRedisClientManager(this.writeHosts, this.readOnlyHosts);
            return redisClientManager.GetClient();
        }

        private SessionStateStoreData GetSessionStoreItem(bool lockRecord,
                                                          HttpContext context,
                                                          string id,
                                                          out bool locked,
                                                          out TimeSpan lockAge,
                                                          out object lockId,
                                                          out SessionStateActions actionFlags)
        {
            // Initial values for return value and out parameters.
            SessionStateStoreData item = null;
            lockAge = TimeSpan.Zero;
            lockId = null;
            locked = false;
            actionFlags = 0;

            // String to hold serialized SessionStateItemCollection.
            string serializedItems = string.Empty;
            // True if a record is found in the database.
            bool foundRecord = false;
            // True if the returned session item is expired and needs to be deleted.
            bool deleteData = false;
            // Timeout value from the data store.
            int timeout = 0;

            SessionItem currentSessionItem;

            using (IRedisClient redisClient = this.CreateRedisClient())
            {
                try
                {
                    // lockRecord is true when called from GetItemExclusive and
                    // false when called from GetItem.
                    // Obtain a lock if possible. Ignore the record if it is expired.
                    if (lockRecord)
                    {
                        currentSessionItem = redisClient.Get<SessionItem>(this.GetRedisKey(id));

                        if (currentSessionItem != null)
                        {
                            // If the item is locked then do not attempt to update it
                            if (!currentSessionItem.Locked)
                            {
                                currentSessionItem.Locked = true;
                                currentSessionItem.LockDate = DateTime.UtcNow;
                                redisClient.Set<SessionItem>(this.GetRedisKey(id), currentSessionItem, DateTime.UtcNow.AddMinutes(this.sessionConfigSection.Timeout.TotalMinutes));

                                locked = false;
                            }
                            else
                            {
                                locked = true;
                            }
                        }
                    }

                    currentSessionItem = redisClient.Get<SessionItem>(this.GetRedisKey(id));

                    if (currentSessionItem != null)
                    {
                        if (currentSessionItem.Expires < DateTime.Now)
                        {
                            // The record was expired. Mark it as not locked.
                            locked = false;
                            // The session was expired. Mark the data for deletion.
                            deleteData = true;
                        }
                        else
                        {
                            foundRecord = true;
                        }

                        serializedItems = currentSessionItem.SessionItems;
                        lockId = currentSessionItem.LockId;
                        lockAge = DateTime.UtcNow.Subtract(currentSessionItem.LockDate);
                        actionFlags = (SessionStateActions)currentSessionItem.Flags;
                        timeout = currentSessionItem.Timeout;
                    }
                    else
                    {
                        locked = false;
                    }

                    // If the returned session item is expired, 
                    // delete the record from the data source.
                    if (deleteData)
                    {
                        redisClient.Remove(this.GetRedisKey(id));
                    }

                    // The record was not found. Ensure that locked is false.
                    if (!foundRecord)
                    {
                        locked = false;
                    }

                    // If the record was found and you obtained a lock, then set 
                    // the lockId, clear the actionFlags,
                    // and create the SessionStateStoreItem to return.
                    if (foundRecord && !locked)
                    {
                        lockId = (int?)lockId + 1;
                        currentSessionItem.LockId = lockId != null ? (int)lockId : 0;
                        currentSessionItem.Flags = 0;

                        redisClient.Set<SessionItem>(this.GetRedisKey(id), currentSessionItem, DateTime.UtcNow.AddMinutes(this.sessionConfigSection.Timeout.TotalMinutes));

                        // If the actionFlags parameter is not InitializeItem,
                        // deserialize the stored SessionStateItemCollection.
                        if (actionFlags == SessionStateActions.InitializeItem)
                        {
                            item = CreateNewStoreData(context, 30);
                        }
                        else
                        {
                            item = Deserialize(context, serializedItems, timeout);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }

          return item;
        }

        #region Serialization
        /// <summary>
        /// Serialize is called by the SetAndReleaseItemExclusive method to
        /// convert the SessionStateItemCollection into a Base64 string to
        /// be stored in MongoDB.
        /// </summary>
        private string Serialize(SessionStateItemCollection items)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    if (items != null)
                    {
                        items.Serialize(writer);
                    }

                    writer.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private SessionStateStoreData Deserialize(HttpContext context, string serializedItems, int timeout)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(serializedItems)))
            {
                SessionStateItemCollection sessionItems = new SessionStateItemCollection();

                if (ms.Length > 0)
                {
                    using (BinaryReader reader = new BinaryReader(ms))
                    {
                        sessionItems = SessionStateItemCollection.Deserialize(reader);
                    }
                }

                return new SessionStateStoreData(sessionItems,
                  SessionStateUtility.GetSessionStaticObjects(context),
                  timeout);
            }
        }
        #endregion Serialization

    }
}
