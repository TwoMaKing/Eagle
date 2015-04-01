using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveLife
{
    /// <summary>
    /// 代表用户的聚合根对象
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string IPAddress { get; set; }
    }
}