using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace XLabs.Web
{
    public class HttpHandler : HttpClientHandler
    {
        private const string CookieHeader = "Set-Cookie";

        public EventHandler<EventArgs<Cookie>> CookieReceived;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            foreach (var header in request.Headers)
            {
                System.Diagnostics.Debug.WriteLine(header.Key);
                System.Diagnostics.Debug.WriteLine(header.Value.FirstOrDefault());
            }

            var response = await base.SendAsync(request, cancellationToken);

            foreach (var header in response.Headers)
            {
                System.Diagnostics.Debug.WriteLine(header.Key);
                foreach (var value in header.Value)
                    System.Diagnostics.Debug.WriteLine(value);
            }

            if (response.IsSuccessStatusCode && response.Headers.Contains(CookieHeader))
            {
                try
                {
                    var cookie = response.Headers.GetValues(CookieHeader).First().Split(';')[0].Split('=');
                    this.CookieReceived.Invoke<Cookie>(this, new Cookie(cookie[0], cookie[1]));
                }
                catch
                {

                }
            }

            return response;
        }
    }
}
