using System;
using Xamarin.Forms.Labs.Services.Serialization;
using System.Net.Http;

namespace Xamarin.Forms.Labs.Services.Web.RestClient
{
    public class JsonRestClient : RestCoreClient
    {
        public JsonRestClient(IJsonSerializer serializer, HttpClient httpClient = null)
            : base(serializer, httpClient)
        {
            if (serializer.Format != SerializationFormat.Json)
            {
                throw new Exception(string.Format("Invalid serializer type: {0}. Valid type is: {1}", serializer.Format, SerializationFormat.Json));
            }
        }

        protected override string StringContentType
        {
            get { return "text/json"; }
        }
    }
}

