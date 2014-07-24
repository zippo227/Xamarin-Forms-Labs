using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Services;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using Microsoft.Phone.Net.NetworkInformation;

namespace Xamarin.Forms.Labs.WP8.Services
{
    public class Network : INetwork
    {
        public Network()
        {
            DeviceNetworkInformation.NetworkAvailabilityChanged += DeviceNetworkInformation_NetworkAvailabilityChanged;
        }

        #region INetwork Members

        public Task<bool> IsReachable(string host, TimeSpan timeout)
        {
            return Task<bool>.Run(() =>
                {
                    if (!DeviceNetworkInformation.IsNetworkAvailable)
                    {
                        return false;
                    }

                    AutoResetEvent e = new AutoResetEvent(false);

                    bool isReachable = false;
                    NameResolutionCallback d = delegate(NameResolutionResult result) 
                    {
                        isReachable = result.NetworkErrorCode == NetworkError.Success;
                        e.Set();
                    };

                    DeviceNetworkInformation.ResolveHostNameAsync(new System.Net.DnsEndPoint(host, 0), d, this);


                    e.WaitOne(timeout);

                    return isReachable;
                });
        }

        void DeviceNetworkInformation_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            if (e.NotificationType == NetworkNotificationType.InterfaceConnected)
            {

            }
            else if (e.NotificationType == NetworkNotificationType.InterfaceDisconnected)
            {

            }
        }

        #endregion
    }
}
