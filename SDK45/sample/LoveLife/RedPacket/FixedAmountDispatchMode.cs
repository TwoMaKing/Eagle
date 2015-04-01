using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveLife
{
    public class FixedAmountDispatchMode : IDispatchMode
    {
        public IEnumerable<RedPacket> GetDispatchedRedPackets(int redPacketCount, decimal amount)
        {
            List<RedPacket> redPacketList = new List<RedPacket>();

            for (int index = 0; index < redPacketCount; index++)
            {
                RedPacket redPacket = new RedPacket();
                redPacket.Amount = amount;
            }

            return redPacketList;
        }
    }
}