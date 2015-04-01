using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RedPacketService
{
    [DataContract()]
    public class RedPacketScenarioDataObject
    {
        [DataMember()]
        public ChatGroupDataObject ChatGroup { get; set; }

        [DataMember()]
        public List<RedPacketDataObject> RedPacketDataObjects { get; set; }

        [DataMember()]
        public string Message { get; set; }

    }
}