using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Net;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Droid
{
    public class Network : INetwork
    {
        #region INetwork Members

        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            return Task<bool>.Run(() =>
            {
                try
                {
                    var address = InetAddress.GetByName(host);
                    return address == null ? false : address.IsReachable((int)timeout.TotalMilliseconds);
                }
                catch (Java.Net.UnknownHostException)
                {
                    return false;
                }
            });
        }

        #endregion
    }
}