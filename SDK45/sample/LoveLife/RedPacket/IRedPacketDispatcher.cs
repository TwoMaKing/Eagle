using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveLife
{
    /// <summary>
    /// 红包场景的行为
    /// </summary>
    public interface IRedPacketDispatcher
    {
        /// <summary>
        /// 派发发送红包
        /// </summary>
        void Dispatch();
    }
}