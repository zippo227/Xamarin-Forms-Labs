using XLabs.Platform.iOS.Services;

[assembly: Dependency(typeof(PhoneService))]

namespace XLabs.Platform.iOS.Services
{
	using System;

	using MonoTouch.CoreTelephony;
	using MonoTouch.MessageUI;
	using MonoTouch.SystemConfiguration;
	using MonoTouch.UIKit;

	using XLabs.Platform.Services;

	/// <summary>
    /// Apple Phone service implements <see cref="IPhoneService"/>.
    /// </summary>
    public class PhoneService : IPhoneService
    {
        private static readonly Lazy<CTTelephonyNetworkInfo> TelNet = new Lazy<CTTelephonyNetworkInfo>();

        #region IPhone implementation
        /// <summary>
        /// Gets the cellular provider.
        /// </summary>
        /// <value>
        /// The cellular provider name.
        /// </value>
        public string CellularProvider
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.CarrierName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is cellular data enabled; otherwise, <c>false</c>.</value>
        public bool? IsCellularDataEnabled
        {
            get
            {
                NetworkReachabilityFlags flags;
                Reachability.IsNetworkAvailable(out flags);
                return (flags & NetworkReachabilityFlags.IsWWAN) != 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has cellular data roaming enabled.
        /// </summary>
        /// <value><c>true</c> if this instance is cellular data roaming enabled; otherwise, <c>false</c>.</value>
        public bool? IsCellularDataRoamingEnabled
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is network available.
        /// </summary>
        /// <value><c>true</c> if this instance is network available; otherwise, <c>false</c>.</value>
        public bool? IsNetworkAvailable
        {
            get
            {
                NetworkReachabilityFlags flags;
                return Reachability.IsNetworkAvailable(out flags);
            }
        }

        /// <summary>
        /// Gets the ISO Country Code.
        /// </summary>
        /// <value>
        /// The ISO Country Code.
        /// </value>
        public string ICC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.IsoCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Country Code.
        /// </summary>
        /// <value>
        /// The Mobile Country Code.
        /// </value>
        public string MCC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Network Code.
        /// </summary>
        /// <value>
        /// The Mobile Network Code.
        /// </value>
        public string MNC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileNetworkCode;
            }
        }

        public bool CanSendSMS
        {
            get
            {
                return MFMessageComposeViewController.CanSendText;
            }
        }
        /// <summary>
        /// Opens native dialog to dial the specified number.
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            UIApplication.SharedApplication.OpenUrl(new MonoTouch.Foundation.NSUrl("tel:" + number));
        }

        public void SendSMS(string to, string body)
        {
            if (this.CanSendSMS)
            {
                var smsController = new MFMessageComposeViewController()
                {
                    Body = body,
                    Recipients = new[] { to }
                };

                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(smsController, true, null);
            }
        }
        #endregion
    }
}

