using System;

namespace Xamarin.Forms.Labs.Services.Web
{
    /// <summary>
    /// Rest response.
    /// </summary>
	public class RestResponse<T>
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Forms.Labs.Services.Web.RestResponse{T}"/> class.
        /// </summary>
        /// <param name="response">Response value.</param>
        public RestResponse(T response)
        {
            this.Response = response;
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <value>The response.</value>
        public T Response { get; private set; }

        /// /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception if response was not a success.</value>
        public Exception Exception { get; set; }
	}
}

