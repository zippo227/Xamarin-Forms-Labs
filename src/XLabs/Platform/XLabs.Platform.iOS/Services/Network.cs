using XLabs.Platform.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(Network))]

namespace XLabs.Platform.iOS.Services
{
	using System;
	using System.Threading.Tasks;

	using XLabs.Platform.Services;

	public class Network : INetwork
    {
        private event Action<NetworkStatus> reachabilityChanged;

        public Network()
        {

        }

        void HandleReachabilityChanged (object sender, EventArgs e)
        {
            var reachabilityChanged = this.reachabilityChanged;
            if (reachabilityChanged != null)
                reachabilityChanged(this.InternetConnectionStatus());
        }

        public event Action<NetworkStatus> ReachabilityChanged
        {
            add
            {
                if (this.reachabilityChanged == null)
                {
                    // TODO: check if this actually works
                    Reachability.ReachabilityChanged += HandleReachabilityChanged;
                }

                this.reachabilityChanged += value;
            }

            remove
            {
                this.reachabilityChanged -= value;

                if (this.reachabilityChanged == null)
                {
                    Reachability.ReachabilityChanged -= HandleReachabilityChanged;
                }
            }
        }

        public NetworkStatus InternetConnectionStatus()
        {
            return Reachability.InternetConnectionStatus();
        }

        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            return Task<bool>.Run(() => Reachability.IsHostReachable(host));
        }

        public Task<bool> IsReachableByWifi(string host, TimeSpan timeout)
        {
            return Task<bool>.Run(() => Reachability.RemoteHostStatus(host) == NetworkStatus.ReachableViaWiFiNetwork);
        }
    }
}