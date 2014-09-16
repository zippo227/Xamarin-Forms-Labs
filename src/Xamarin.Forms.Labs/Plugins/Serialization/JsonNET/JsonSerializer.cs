using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Xamarin.Forms.Labs.Services.Serialization.JsonNET
{
    /// <summary>
    /// JSON serializer using Newtonsoft.Json library.
    /// </summary>
    /// <remarks>
    /// 
    /// Newtonsoft.Json copyright information.
    /// 
    /// The MIT License (MIT)
    /// Copyright (c) 2007 James Newton-King
    /// 
    /// https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md
    /// 
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1644:DocumentationHeadersMustNotContainBlankLines", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1629:DocumentationTextMustEndWithAPeriod", Justification = "Reviewed. Suppression is OK here."), SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public class JsonSerializer : StringSerializer, IJsonSerializer
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="Xamarin.Forms.Labs.Services.Serialization.JsonNET.JsonSerializer"/> class
        /// with the most flexible settings.
        /// </summary>
        /// <remarks>
        /// These settings might not be the most efficient so consider using the constructor
        /// with options.
        /// </remarks> 
        public JsonSerializer()
        {
            //JsonConvert.DefaultSettings = JsonConvert.DefaultSettings ?? new Func<JsonSerializerSettings>(() => 
            //    new JsonSerializerSettings()
            //    {
            //        TypeNameHandling = TypeNameHandling.All,
            //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //    });
        }

        public JsonSerializer(TypeNameHandling typeNameHandling, ReferenceLoopHandling referenceLoopHandling)
        {
            JsonConvert.DefaultSettings = JsonConvert.DefaultSettings ?? new Func<JsonSerializerSettings>(() => 
                new JsonSerializerSettings()
                {
                    TypeNameHandling = typeNameHandling,
                    ReferenceLoopHandling = referenceLoopHandling
                });
        }

        /// <summary>
        /// Gets the serialization format.
        /// </summary>
        /// <value>Serialization format.</value>
        public override SerializationFormat Format
        {
            get { return SerializationFormat.Json; }
        }

        /// <summary>
        /// Cleans memory.
        /// </summary>
        public override void Flush()
        {
#if DEBUG
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Serializes object to a string.
        /// </summary>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized string of the object.</returns>
        public override string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Deserializes string into an object.
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize to.</typeparam>
        /// <param name="data">Serialized object.</param>
        /// <returns>Object of type T.</returns>
        public override T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
