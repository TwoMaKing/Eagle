using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Eagle.Common.Serialization
{
    public class ObjectJsonSerializer : IObjectSerializer
    {
        public byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            byte[] bytes = null;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();

                    jsonSerializer.Serialize(streamWriter, obj);

                    bytes = memoryStream.ToArray();

                    streamWriter.Close();
                }

                memoryStream.Close();
            }

            return bytes;
        }

        public T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null)
            {
                return default(T);
            }

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(memoryStream))
                    {
                        JsonSerializer jsonSerializer = new JsonSerializer();

                        object obj = jsonSerializer.Deserialize(streamReader, typeof(T));

                        return (T)obj;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    memoryStream.Close();
                }
            }
        }
    }
}
