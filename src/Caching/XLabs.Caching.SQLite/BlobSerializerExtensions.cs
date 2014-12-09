using System;
using System.Reflection;
using SQLite.Net;
using XLabs.Serialization;

namespace XLabs.Caching.SQLite
{
    public static class BlobSerializerExtensions
    {
        public static IBlobSerializer AsBlobSerializer(this IByteSerializer serializer)
        {
            var gm = typeof(IByteSerializer).GetTypeInfo().GetDeclaredMethod("Deserialize");

            return new BlobSerializerDelegate(
                serializer.SerializeToBytes,
                (data, type) => gm.MakeGenericMethod(type).Invoke(serializer, new object[] { data }),
                serializer.CanDeserialize);
        }

        private static bool CanDeserialize(this IByteSerializer serializer, Type type)
        {
            return true;
        }
    }
}
