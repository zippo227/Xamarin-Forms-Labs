using Android.Telephony;
using Android.Content;
using Android.App;
using Android.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(XForms.Toolkit.Services.PhoneService))]
namespace XForms.Toolkit.Services
{
    public class PhoneService : IPhoneService
    {
        public static TelephonyManager Manager
        {
            get
            {
                return Application.Context.GetSystemService(Context.TelephonyService) as TelephonyManager;
            }
        }

        #region IPhone implementation

        public string CellularProvider
        {
            get
            {
                return PhoneService.Manager.NetworkOperatorName;
            }
        }

        public string ICC
        {
            get
            {
                return PhoneService.Manager.SimCountryIso;
            }
        }

        public string MCC
        {
            get
            {
                return PhoneService.Manager.NetworkOperator.Remove(3,3);
            }
        }

        public string MNC
        {
            get
            {
                return PhoneService.Manager.NetworkOperator.Remove(0,3);
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

        public void DialNumber(string number)
        {
            number.StartActivity(new Intent(Intent.ActionDial, Uri.Parse("tel:" + number)));
        }
        #endregion
    }
}

