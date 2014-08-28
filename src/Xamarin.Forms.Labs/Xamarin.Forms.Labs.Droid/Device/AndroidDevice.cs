using Android.Telephony;
using Android.OS;
using Xamarin.Forms.Labs.Droid.Services.Media;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;
using Xamarin.Forms.Labs.Droid;
using Xamarin.Forms.Labs.Droid.Services;

namespace Xamarin.Forms.Labs
{
    /// <summary>
    /// Android device implements <see cref="IDevice"/>.
    /// </summary>
    public class AndroidDevice : IDevice
    {
        private static IDevice currentDevice;

        private IBluetoothHub btHub;

        /// <summary>
        /// Prevents a default instance of the <see cref="AndroidDevice"/> class from being created. 
        /// </summary>
        private AndroidDevice()
        {
            var manager = Services.PhoneService.Manager;

            if (manager != null && manager.PhoneType != PhoneType.None)
            {
                this.PhoneService = new PhoneService();
            }

            if (Labs.Accelerometer.IsSupported)
            {
                this.Accelerometer = new Accelerometer();
            }

            if (Labs.Gyroscope.IsSupported)
            {
                this.Gyroscope = new Gyroscope();
            }

//            if (BluetoothAdapter.DefaultAdapter != null)
//            {
//                this.BluetoothHub = new BluetoothHub(BluetoothAdapter.DefaultAdapter);
//            }

            this.Display = new Display();

            this.Manufacturer = Build.Manufacturer;
            this.Name = Build.Model;
            this.HardwareVersion = Build.Hardware;
            this.FirmwareVersion = Build.VERSION.Release;

            this.Battery = new Battery();

            this.MediaPicker = new MediaPicker();

            this.Network = new Network();
        }

        /// <summary>
        /// Gets the current device.
        /// </summary>
        /// <value>
        /// The current device.
        /// </value>
        public static IDevice CurrentDevice { get { return currentDevice ?? (currentDevice = new AndroidDevice()); } }

        #region IDevice implementation
        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>
        /// The id for the device.
        /// </value>
        public string Id
        {
            // TODO: Verify what is the best combination of Unique Id for Android
            get { return Build.Serial; }
        }

        /// <summary>
        /// Gets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService 
        { 
            get; 
            private set; 
        }

        /// <summary>
        /// Gets the display information for the device.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        public IDisplay Display
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the battery.
        /// </summary>
        /// <value>
        /// The battery.
        /// </value>
        public IBattery Battery
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        public IMediaPicker MediaPicker
        {
            get; 
            private set;
        }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        public INetwork Network
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the accelerometer for the device if available.
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        public IBluetoothHub BluetoothHub
        {
            get
            {
                if (this.btHub == null && Android.Bluetooth.BluetoothAdapter.DefaultAdapter != null)
                {
                    this.btHub = new BluetoothHub(Android.Bluetooth.BluetoothAdapter.DefaultAdapter);
                }

                return this.btHub;
            }
        }

        /// <summary>
        /// Gets the name of the device.
        /// </summary>
        /// <value>
        /// The name of the device.
        /// </value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the firmware version.
        /// </summary>
        /// <value>
        /// The firmware version.
        /// </value>
        public string FirmwareVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the hardware version.
        /// </summary>
        /// <value>
        /// The hardware version.
        /// </value>
        public string HardwareVersion
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        public string Manufacturer
        {
            get;
            private set;
        }
        #endregion
    }
}

