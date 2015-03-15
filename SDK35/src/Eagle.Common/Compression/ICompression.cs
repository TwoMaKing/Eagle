using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Common.Serialization;

namespace Eagle.Common.Compression
{
    public interface ICompression
    {
        byte[] Zip(object obj);
        
        byte[] Zip<T>(T obj);

        object Unzip(byte[] bytes);

        T Unzip<T>(byte[] bytes);
    }
}
