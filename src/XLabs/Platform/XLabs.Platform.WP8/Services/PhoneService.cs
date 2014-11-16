using XLabs.Platform.WP8.Services;

[assembly: Dependency(typeof(PhoneService))]
namespace XLabs.Platform.WP8.Services
{
	using Microsoft.Phone.Net.NetworkInformation;
	using Microsoft.Phone.Tasks;

	/// <summary>
    /// Phone service for Windows Phone devices.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
        public string CellularProvider
        {
            get
            {
                return DeviceNetworkInformation.CellularMobileOperator;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data enabled.
        /// </summary>
        public bool? IsCellularDataEnabled
        {
            get
            {
                return DeviceNetworkInformation.IsCellularDataEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data roaming enabled.
        /// </summary>
        public bool? IsCellularDataRoamingEnabled
        {
            get
            {
                return DeviceNetworkInformation.IsCellularDataRoamingEnabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is network available.
        /// </summary>
        public bool? IsNetworkAvailable
        {
            get
            {
                return DeviceNetworkInformation.IsNetworkAvailable;
            }
        }

        /// <summary>
        /// Gets the ISO Country Code
        /// </summary>
        public string ICC
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the Mobile Country Code
        /// </summary>
        public string MCC
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets the Mobile Network Code
        /// </summary>
        public string MNC
        {
            get { return string.Empty; }
        }

        public bool CanSendSMS
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Opens native dialog to dial the specified number
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            new PhoneCallTask{ PhoneNumber = number }.Show();
        }

        public void SendSMS(string to, string body)
        {
            new SmsComposeTask{ To = to, Body = body }.Show();
        }
    }
}
