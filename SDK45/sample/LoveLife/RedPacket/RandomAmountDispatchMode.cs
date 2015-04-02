using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveLife
{
    public class RandomAmountDispatchMode : IDispatchMode
    {
        public IEnumerable<RedPacket> GetDispatchedRedPackets(int redPacketCount, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}