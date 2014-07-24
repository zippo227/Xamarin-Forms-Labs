using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.iOS.Services
{
    public class Network : INetwork
    {
        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            return Task<bool>.Run(() => Reachability.IsHostReachable(host));
        }
    }
}