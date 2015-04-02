using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLife
{
    public interface IDispatchMode
    {
        IEnumerable<RedPacket> GetDispatchedRedPackets(int redPacketCount, decimal amount);
    }
}
