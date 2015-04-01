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
    public class ChatGroupDataObject
    {
        [DataMember()]
        public Guid Id { get; set; }

        [DataMember()]
        public string Name { get; set; }

        [DataMember()]
        public List<UserDataObject> Users { get; set; }
    }
}