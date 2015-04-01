using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveLife
{

    /// <summary>
    /// 代表红包实体
    /// </summary>
    public class RedPacket
    {
        /// <summary>
        /// 被分配的金额 通过随机分配或者固定分配的金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 指定该钱包是否已被领取
        /// </summary>
        public bool Received
        {
            get 
            {
                return this.Receiver != null &&
                       this.ReceivedDate != null;
            }
        }

        /// <summary>
        /// 领取人
        /// </summary>
        public User Receiver {get;set;}

        /// <summary>
        /// 领取日期
        /// </summary>
        public DateTime? ReceivedDate { get; set; }

    }



}