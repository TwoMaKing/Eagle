using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Common.Serialization
{
    public interface IObjectSerializer
    {
        byte[] Serialize(object obj);

        T Deserialize<T>(byte[] bytes);
    }
}
