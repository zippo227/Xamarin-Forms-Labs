using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services.Serialization;

namespace Xamarin.Forms.Labs.Services.Web.RestClient
{
    /// <summary>
    /// The rest core client.
    /// </summary>
    public abstract class RestCoreClient : IRestClient
    {
        /// <summary>
        /// The serializer to use.
        /// </summary>
        protected readonly ISerializer Serializer;

        /// <summary>
        /// The Http client.
        /// </summary>
        protected readonly HttpClient Client;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestCoreClient"/> class.
        /// </summary>
        /// <param name="serializer">
        /// The serializer to use.
        /// </param>
        protected RestCoreClient(ISerializer serializer)
            : this(serializer, new HttpClient())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RestCoreClient"/> class.
        /// </summary>
        /// <param name="serializer">
        /// The serializer to use.
        /// </param>
        /// <param name="client">
        /// The Http client.
        /// </param>
        protected RestCoreClient(ISerializer serializer, HttpClient client)
        {
            this.Serializer = serializer;
            this.Client = client ?? new HttpClient();
        }

        /// <summary>
        /// Gets or sets timeout in milliseconds
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                return this.Client.Timeout;
            }

            set
            {
                this.Client.Timeout = value;
            }
        }

        /// <summary>
        /// Gets the string content type.
        /// </summary>
        protected abstract string StringContentType { get; }

        /// <summary>
        /// Add request header.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddHeader(string key, string value)
        {
            this.Client.DefaultRequestHeaders.Add(key, value);
        }

        /// <summary>
        /// Remove request header.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        public void RemoveHeader(string key)
        {
            this.Client.DefaultRequestHeaders.Remove(key);
        }

        /// <summary>
        /// Async POST method.
        /// </summary>
        /// <returns>The async task.</returns>
        /// <param name="address">Address of the service.</param>
        /// <param name="dto">DTO to post.</param>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        public async Task<RestResponse<T>> PostAsync<T>(string address, object dto)
        {
            try
            {
                var content = (this.Serializer as IStringSerializer).Serialize(dto);

                var response = await this.Client.PostAsync(
                    address,
                    new StringContent(content, Encoding.UTF8, this.StringContentType));
                return await GetResponse<T>(response, this.Serializer);
            }
            catch (Exception ex)
            {
                return new RestResponse<T>(default(T))
                {
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Async PUT method.
        /// </summary>
        /// <returns>The async task.</returns>
        /// <param name="address">Address of the service.</param>
        /// <param name="dto">DTO to put.</param>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        public async Task<RestResponse<T>> PutAsync<T>(string address, object dto)
        {
            try
            {
                var content = (this.Serializer as IStringSerializer).Serialize(dto);

                var response = await this.Client.PutAsync(
                    address,
                    new StringContent(content, Encoding.UTF8, this.StringContentType));

                return await GetResponse<T>(response, this.Serializer);
            }
            catch (Exception ex)
            {
                return new RestResponse<T>(default(T))
                {
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Async GET method.
        /// </summary>
        /// <returns>The async task.</returns>
        /// <param name="address">Address of the service.</param>
        /// <typeparam name="T">The type of object to be returned.</typeparam>
        public async Task<RestResponse<T>> GetAsync<T>(string address)
        {
            try
            {
                var response = await this.Client.GetAsync(address);
                return await GetResponse<T>(response, this.Serializer);
            }
            catch (Exception ex)
            {
                return new RestResponse<T>(default(T))
                {
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Async GET method.
        /// </summary>
        /// <returns>The async task.</returns>
        /// <param name="address">Address of the service.</param>
        /// <param name="values">Values for the request.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<RestResponse<T>> GetAsync<T>(string address, Dictionary<string, string> values)
        {
            try
            {
                var builder = new StringBuilder(address);
                builder.Append("?");

                foreach (var pair in values)
                {
                    builder.Append(string.Format("{0}={1}&amp;", pair.Key, pair.Value));
                }

                var response = await this.Client.GetAsync(builder.ToString());
                return await GetResponse<T>(response, this.Serializer);
            }
            catch (Exception ex)
            {
                return new RestResponse<T>(default(T))
                {
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Deletes the async.
        /// </summary>
        /// <returns>The async task.</returns>
        /// <param name="address">Address of the service.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<RestResponse<T>> DeleteAsync<T>(string address)
        {
            try
            {
                var response = await this.Client.DeleteAsync(address);
                return await GetResponse<T>(response, this.Serializer);
            }
            catch (Exception ex)
            {
                return new RestResponse<T>(default(T))
                {
                    Exception = ex
                };
            }
        }

        /// <summary>
        /// Gets the response from Http response message
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <param name="response">Http response message</param>
        /// <param name="serializer">Serializer to use.</param>
        /// <returns>The async task.</returns>
        private static async Task<RestResponse<T>> GetResponse<T>(HttpResponseMessage response, ISerializer serializer)
        {
            if (!response.IsSuccessStatusCode)
            {
                return new RestResponse<T>(default(T))
                {
                    Exception = new Exception(response.ReasonPhrase)
                };
            }

            try
            {
                var stream = await response.Content.ReadAsStreamAsync();

                return new RestResponse<T>(serializer.Deserialize<T>(stream));
                ////// get response strings
                //// var content = await response.Content.ReadAsStringAsync();
                ////// serialize the response to object
                //// return serializer.Deserialize<ServiceResponse<T>>(content);
                ////returnResponse.Value = serializer.Deserialize<T>(returnResponse.Content);
            }
            catch (Exception ex)
            {
                return new RestResponse<T>(default(T))
                {
                    Exception = ex
                };
            }
        }
    }
}

