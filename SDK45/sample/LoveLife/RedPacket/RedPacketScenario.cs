using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLife
{
    /// <summary>
    /// 发红包场景的领域对象(领域模型), 红包场景是该聚合的聚合根. 该聚合的边界也就是关联的实体是红包集合对象
    /// </summary>
    public class RedPacketScenario
    {
        private Guid id = new Guid();

        public RedPacketScenario(int redPacketCount, decimal totalAmount)
            : this(new RandomAmountDispatchMode(), redPacketCount, totalAmount) 
        { }

        public RedPacketScenario(IDispatchMode dispatchMode,
                                 int redPacketCount,
                                 decimal totalAmount)
            : this(dispatchMode, redPacketCount, totalAmount, string.Empty) 
        { }

        public RedPacketScenario(IDispatchMode dispatchMode,
                                 int redPacketCount,
                                 decimal totalAmount, 
                                 string message)
        {
            this.RedPackets = dispatchMode.GetDispatchedRedPackets(redPacketCount, totalAmount);

            this.Message = message;
        }

        public Guid Id 
        {
            get 
            {
                return this.id;
            }
            set 
            {
                this.id = value;
            }
        }

        /// <summary>
        /// 留言
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 聚合关联对象，红包集合
        /// </summary>
        public IEnumerable<RedPacket> RedPackets { get; private set; }

        /// <summary>
        /// 聚合关联对象，聊天群
        /// </summary>
        public ChatGroup ChatGroup { get; set; }

        public void Dispatch(IRedPacketDispatcher dispatcher)
        {
            dispatcher.Dispatch();
        }

        public void Receive(IRedPacketReceiver receiver)
        {
            receiver.Receive();
        }
    }
}
