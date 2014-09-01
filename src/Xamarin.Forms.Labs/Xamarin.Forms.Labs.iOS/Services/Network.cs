using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms.Labs.Services;

[assembly: Xamarin.Forms.Dependency(typeof(Xamarin.Forms.Labs.iOS.Services.Network))]

namespace Xamarin.Forms.Labs.iOS.Services
{
	public class Network : INetwork
    {
		public Network()
		{
			Reachability.ReachabilityChanged += HandleReachabilityChanged;
		}

		void HandleReachabilityChanged (object sender, EventArgs e)
		{
			var reachabilityChanged = ReachabilityChanged;
			if (reachabilityChanged != null)
				ReachabilityChanged(this.InternetConnectionStatus());
		}

		public event Action<NetworkStatus> ReachabilityChanged;

		public Xamarin.Forms.Labs.Services.NetworkStatus InternetConnectionStatus ()
		{
			var status = Reachability.InternetConnectionStatus ();
			return (NetworkStatus)Enum.Parse (typeof(NetworkStatus), status.ToString ());
		}

        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            return Task<bool>.Run(() => Reachability.IsHostReachable(host));
        }
    }
}