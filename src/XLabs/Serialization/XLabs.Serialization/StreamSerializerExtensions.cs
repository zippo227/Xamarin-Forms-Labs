using System;
using System.IO;
using System.Text;

namespace XLabs.Serialization
{
    public static class StreamSerializerExtensions
    {
        public static T DeserializeFromString<T>(this IStreamSerializer serializer, string value, Encoding encoding = null)
        {
            //var encoder = encoding ?? Encoding.UTF8;

            //var bytes = encoder.GetBytes(value);
            var bytes = Convert.FromBase64String(value);
            using (var stream = new MemoryStream(bytes))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static string SerializeToString<T>(this IStreamSerializer serializer, T obj, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            using (var stream = new MemoryStream())
            {
                serializer.Serialize<T>(obj, stream);
                stream.Position = 0;
                var bytes = new byte[stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);

                return Convert.ToBase64String(bytes);
            }
        }

        public static T DeserializeFromBytes<T>(this IStreamSerializer serializer, byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static byte[] GetSerializedBytes(this IStreamSerializer serializer, object obj)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(obj, stream);
                return stream.ToArray();
            }
        }
    }
}
