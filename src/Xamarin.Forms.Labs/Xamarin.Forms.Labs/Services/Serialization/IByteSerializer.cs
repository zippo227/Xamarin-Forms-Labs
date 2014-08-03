namespace Xamarin.Forms.Labs.Services.Serialization
{
    /// <summary>
    /// Common interface for byte buffer serializer.
    /// </summary>
    public interface IByteSerializer
    {
        /// <summary>
        /// Serializes object to a string.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized byte[] of the object.</returns>
        byte[] SerializeToBytes<T>(T obj);

        /// <summary>
        /// Deserializes string into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize to.</typeparam>
        /// <param name="data">Serialized object as byte buffer.</param>
        /// <returns>Object of type T.</returns>
        T Deserialize<T>(byte[] data);
    }
}
