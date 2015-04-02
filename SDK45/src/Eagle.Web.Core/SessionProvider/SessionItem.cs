using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Web.Core.SessionProvider
{
    [Serializable()]
    public class SessionItem
    {
        public string SessionId { get; set; }

        public string ApplicationName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime Expires { get; set; }

        public DateTime LockDate { get; set; }

        public int LockId { get; set; }
        
        public int Timeout { get; set; }
        
        public bool Locked { get; set; }
        
        public string SessionItems { get; set; }
        
        public int Flags { get; set; }

    }
}
