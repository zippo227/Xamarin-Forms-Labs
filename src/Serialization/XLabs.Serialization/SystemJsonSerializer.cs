using System.IO;
using System.Runtime.Serialization.Json;

namespace XLabs.Serialization
{
    /// <summary>
    /// The system JSON serializer.
    /// </summary>
    public class SystemJsonSerializer : StreamSerializer, IJsonSerializer
    {
        /// <summary>
        /// Gets the format.
        /// </summary>
        public override SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

        /// <summary>
        /// Cleans memory.
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// Serializes object to a stream.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="stream">Stream to serialize to.</param>
        public override void Serialize<T>(T obj, Stream stream)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            serializer.WriteObject(stream, obj);
        }

        /// <summary>
        /// Deserializes stream into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="stream">Stream to deserialize from.</param>
        /// <returns>Object of type T.</returns>
        public override T Deserialize<T>(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(stream);
        }

        public override T Deserialize<T>(string data)
        {
            return (this as IStreamSerializer).DeserializeFromString<T>(data, System.Text.Encoding.UTF8);
        }

        public override string Serialize<T>(T obj)
        {
            return (this as IStreamSerializer).SerializeToString(obj, System.Text.Encoding.UTF8);
        }
    }
}
