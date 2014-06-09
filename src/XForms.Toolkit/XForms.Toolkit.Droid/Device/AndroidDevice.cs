using System;
using Android.Telephony;
using Android.OS;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Device
{
    public class AndroidDevice: IDevice
    {
        private static IDevice currentDevice;

        private AndroidDevice ()
        {
            var manager = XForms.Toolkit.Services.PhoneService.Manager;

            if (manager != null && manager.PhoneType != PhoneType.None)
            {
                this.PhoneService = new PhoneService();
            }

//            if (BluetoothAdapter.DefaultAdapter != null)
//            {
//                this.BluetoothHub = new BluetoothHub(BluetoothAdapter.DefaultAdapter);
//            }

            this.Display = new Display ();

            this.Manufacturer = Build.Manufacturer;
            this.Name = Build.Model;
            this.HardwareVersion = Build.Hardware;
            this.FirmwareVersion = Build.VERSION.Release;

//            this.Battery = new BatteryImpl ();
        }

        public static IDevice CurrentDevice { get { return currentDevice ?? (currentDevice = new AndroidDevice()); } }

        #region IDevice implementation

        public IPhoneService PhoneService 
        { 
            get; 
            private set; 
        }

        public IDisplay Display
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public string FirmwareVersion
        {
            get;
            private set;
        }

        public string HardwareVersion
        {
            get;
            private set;
        }

        public string Manufacturer
        {
            get;
            private set;
        }

//        public IBluetoothHub BluetoothHub { get; private set; }
//
//        public IBattery Battery { get; private set; }
        #endregion
    }
}

