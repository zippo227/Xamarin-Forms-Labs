using System.IO;
using System.Text;

namespace Xamarin.Forms.Labs.Services.Serialization
{
    public static class StreamSerializerExtensions
    {
        public static T DeserializeFromString<T>(this IStreamSerializer serializer, string value, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            using (var stream = new MemoryStream(encoder.GetBytes(value)))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static string SerializeToString<T>(this IStreamSerializer serializer, T obj, Encoding encoding = null)
        {
            var encoder = encoding ?? Encoding.UTF8;
            using (var stream = new StreamReader(new MemoryStream(), encoder))
            {
                serializer.Serialize<T>(obj, stream.BaseStream);
                return stream.ReadToEnd();
            }
        }

        public static T DeserializeFromBytes<T>(this IStreamSerializer serializer, byte[] data, Encoding encoding = null)
        {
            using (var stream = new MemoryStream(data))
            {
                return serializer.Deserialize<T>(stream);
            }
        }

        public static byte[] GetSerializedBytes(this IStreamSerializer serializer, object obj, Encoding encoding = null)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(obj, stream);
                return stream.ToArray();
            }
        }
    }
}
