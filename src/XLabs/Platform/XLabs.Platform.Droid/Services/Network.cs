namespace XLabs.Platform.Services
{
	using System;
	using System.Threading.Tasks;

	using Android.App;
	using Android.Content;
	using Android.Net;

	using Java.Net;

	/// <summary>
	/// Android <see cref="INetwork" /> implementation.
	/// </summary>
	public class Network : INetwork
	{
		/// <summary>
		/// Internets the connection status.
		/// </summary>
		/// <returns>NetworkStatus.</returns>
		public NetworkStatus InternetConnectionStatus()
		{
			var status = NetworkStatus.NotReachable;

			var cm = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
			var ni = cm.ActiveNetworkInfo;

			if (ni != null && ni.IsConnectedOrConnecting)
			{
				var name = ni.TypeName.ToUpper();
				if (name.Contains("WIFI"))
				{
					status = NetworkStatus.ReachableViaWiFiNetwork;
				}
				else if (name.Contains("MOBILE"))
				{
					status = NetworkStatus.ReachableViaCarrierDataNetwork;
				}
			}

			return status;
		}

		/// <summary>
		/// Occurs when [reachability changed].
		/// </summary>
		public event Action<NetworkStatus> ReachabilityChanged;

		/// <summary>
		/// Determines whether the specified host is reachable.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="timeout">The timeout.</param>
		public Task<bool> IsReachable(string host, TimeSpan timeout)
		{
			return Task.Run(
				() =>
					{
						try
						{
							var address = InetAddress.GetByName(host);

							return address != null; // && (address.IsReachable((int)timeout.TotalMilliseconds) || );
						}
						catch (UnknownHostException)
						{
							return false;
						}
					});
		}

		//        public bool CanPing(string host)
		//        {
		//            Process p1 = Java.Lang.Runtime.GetRuntime().Exec(string.Format("ping -c 1 {0}", host));
		//
		//
		//            int returnVal = p1.();
		//            boolean reachable = (returnVal==0);
		//        }

		/// <summary>
		/// Determines whether [is reachable by wifi] [the specified host].
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="timeout">The timeout.</param>
		public async Task<bool> IsReachableByWifi(string host, TimeSpan timeout)
		{
			return InternetConnectionStatus() == NetworkStatus.ReachableViaWiFiNetwork && await IsReachable(host, timeout);
		}
	}
}