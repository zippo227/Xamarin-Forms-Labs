using System;

namespace Xamarin.Forms.Labs
{
    public class WebResponseException : Exception
    {
        public WebResponseException()
        {
        }

        public WebResponseException(string message) : base(message)
        {

        }

        public WebResponseException(string message, Exception innerException) 
            : base(message, innerException)
        {

        }
    }
}

