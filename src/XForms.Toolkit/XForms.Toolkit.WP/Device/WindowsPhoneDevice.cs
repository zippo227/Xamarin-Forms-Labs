using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Info;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Device
{
    public class WindowsPhoneDevice : IDevice
    {
        private static WindowsPhoneDevice currentDevice;

        private WindowsPhoneDevice()
        {
            this.Display = new Display();
            this.PhoneService = new PhoneService();
        }

        public static IDevice CurrentDevice { get { return currentDevice ?? (currentDevice = new WindowsPhoneDevice()); } }

        #region IDevice Members

        public IDisplay Display
        {
            get;
            private set;
        }

        public IPhoneService PhoneService
        {
            get;
            private set;
        }

        public string Name
        {
            get { return DeviceStatus.DeviceName; }
        }

        public string FirmwareVersion
        {
            get { return DeviceStatus.DeviceFirmwareVersion; }
        }

        public string HardwareVersion
        {
            get { return DeviceStatus.DeviceHardwareVersion; }
        }

        public string Manufacturer
        {
            get { return DeviceStatus.DeviceManufacturer; }
        }

        #endregion
    }
}
