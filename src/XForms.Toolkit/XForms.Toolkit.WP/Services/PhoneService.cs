
using Microsoft.Phone.Net.NetworkInformation;
using Microsoft.Phone.Tasks;

namespace XForms.Toolkit.Services
{
    public class PhoneService : IPhoneService
    {
        public string CellularProvider
        {
            get
            {
                return DeviceNetworkInformation.CellularMobileOperator;
            }
        }

        public bool? IsCellularDataEnabled
        {
            get
            {
                return DeviceNetworkInformation.IsCellularDataEnabled;
            }
        }

        public bool? IsCellularDataRoamingEnabled
        {
            get
            {
                return DeviceNetworkInformation.IsCellularDataRoamingEnabled;
            }
        }

        public bool? IsNetworkAvailable
        {
            get
            {
                return DeviceNetworkInformation.IsNetworkAvailable;
            }
        }

        public string ICC
        {
            get { return string.Empty; }
        }

        public string MCC
        {
            get { return string.Empty; }
        }

        public string MNC
        {
            get { return string.Empty; }
        }

        public void DialNumber(string number)
        {
            new PhoneCallTask() { PhoneNumber = number }.Show();
        }
    }
}
