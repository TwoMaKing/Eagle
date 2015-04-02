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
    public class RedPacketDataObject
    {
        /// <summary>
        /// 被分配的金额 通过随机分配或者固定分配的金额
        /// </summary>
        [DataMember()]
        public decimal Amount { get; set; }

        /// <summary>
        /// 领取人
        /// </summary>
        [DataMember()]
        public int? ReceiverId { get; set; }

        /// <summary>
        /// 领取日期
        /// </summary>
        [DataMember()]
        public DateTime? ReceivedDate { get; set; }
    }
}