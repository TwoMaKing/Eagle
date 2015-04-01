using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveLife
{
    /// <summary>
    /// 代表聊天群的聚合根
    /// </summary>
    public class ChatGroup
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 聊天群名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前群拥有的成员
        /// </summary>
        public IEnumerable<User> Users { get; }

        /// <summary>
        /// 添加用户
        /// </summary>
        public void AddUser(User user) 
        { }

        /// <summary>
        /// 移除用户
        /// </summary>
        public void RemoveUser(User user)
        { }
    }
}