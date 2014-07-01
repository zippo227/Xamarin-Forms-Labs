using System;
using Xamarin.Forms.Labs.Services.Serialization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

namespace Xamarin.Forms.Labs.Services.Web.RestClient
{
    public abstract class RestCoreClient
    {
        protected readonly ISerializer Serializer;
        protected readonly HttpClient Client;

        protected RestCoreClient(ISerializer serializer)
            : this(serializer, new HttpClient())
        {
        }

        protected RestCoreClient(ISerializer serializer, HttpClient client)
        {
            this.Serializer = serializer;
            this.Client = client ?? new HttpClient();
        }

        #region IRestClient Members

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


        public void AddHeader(string key, string value)
        {
            this.Client.DefaultRequestHeaders.Add(key, value);
        }

        public void RemoveHeader(string key)
        {
            this.Client.DefaultRequestHeaders.Remove(key);
        }

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
        #endregion

        protected abstract string StringContentType { get; }

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
                //var content = await response.Content.ReadAsStringAsync();
                ////// serialize the response to object
                //return serializer.Deserialize<ServiceResponse<T>>(content);
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

