using System;
using MonoTouch.CoreTelephony;
using MonoTouch.UIKit;

namespace XForms.Toolkit
{
    public class PhoneService : IPhoneService
    {
        private static Lazy<CTTelephonyNetworkInfo> TelNet = new Lazy<CTTelephonyNetworkInfo> ();

        #region IPhone implementation

        public string CellularProvider
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.CarrierName;
            }
        }

        public bool? IsCellularDataEnabled
        {
            get
            {
                return null;
            }
        }

        public bool? IsCellularDataRoamingEnabled
        {
            get
            {
                return null;
            }
        }

        public bool? IsNetworkAvailable
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the ISO Country Code
        /// </summary>
        public string ICC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.IsoCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Country Code
        /// </summary>
        public string MCC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileCountryCode;
            }
        }

        /// <summary>
        /// Gets the Mobile Network Code
        /// </summary>
        public string MNC
        {
            get
            {
                return TelNet.Value.SubscriberCellularProvider.MobileNetworkCode;
            }
        }

        public void DialNumber(string number)
        {
            UIApplication.SharedApplication.OpenUrl(new MonoTouch.Foundation.NSUrl("tel:" + number));
        }
        #endregion
    }
}

