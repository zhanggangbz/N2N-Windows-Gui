using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonTools
{
    public class BinarySeria
    {
        public static object GetObject(byte[] bytes)
        {
            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream(bytes))
            {
                memStream.Position = 0;

                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer =
                    new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                return deserializer.Deserialize(memStream);
            }
        }

        public static byte[] GetBytes(object obj)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter serializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                serializer.Serialize(memStream, obj);

                return memStream.GetBuffer();
            }
        }
    }
}
