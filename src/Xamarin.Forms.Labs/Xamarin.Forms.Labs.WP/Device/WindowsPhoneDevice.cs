
using System;
using Microsoft.Phone.Info;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;
using Xamarin.Forms.Labs.WP8.Services;
using Xamarin.Forms.Labs.WP8.Services.Media;

namespace Xamarin.Forms.Labs
{
	/// <summary>
	/// Windows phone device.
	/// </summary>
	public class WindowsPhoneDevice : IDevice
	{
		/// <summary>
		/// The current device.
		/// </summary>
		private static WindowsPhoneDevice currentDevice;

        /// <summary>
        /// The Id for the device.
        /// </summary>
        private string id;

		/// <summary>
		/// Prevents a default instance of the <see cref="WindowsPhoneDevice"/> class from being created.
		/// </summary>
		private WindowsPhoneDevice()
		{
			Display = new Display();
			PhoneService = new PhoneService();
			Battery = new Battery();

			if (Microsoft.Devices.Sensors.Accelerometer.IsSupported)
			{
				Accelerometer = new Accelerometer();
			}

			if (Microsoft.Devices.Sensors.Gyroscope.IsSupported)
			{
                this.Gyroscope = new Gyroscope();
			}

			MediaPicker = new MediaPicker();
		}

		/// <summary>
		/// Gets the current device.
		/// </summary>
		/// <value>The current device.</value>
		public static IDevice CurrentDevice { get { return currentDevice ?? (currentDevice = new WindowsPhoneDevice()); } }

		#region IDevice Members

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>
        /// The id for the device.
        /// </value>
	    public string Id
	    {
	        get
	        {
                if (string.IsNullOrEmpty(this.id))
                {
                    object o;
                    if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out o))
                    {
                        this.id = Convert.ToBase64String((byte[])o);
                    }
                }

	            return this.id;
	        }
	    }

		/// <summary>
		/// Gets the display.
		/// </summary>
		/// <value>The display.</value>
		public IDisplay Display
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the phone service.
		/// </summary>
		/// <value>Phone service instance if available, otherwise null.</value>
		public IPhoneService PhoneService
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the battery.
		/// </summary>
		/// <value>The battery.</value>
		public IBattery Battery
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
		/// Gets the picture chooser.
		/// </summary>
		/// <value>The picture chooser.</value>
		public IMediaPicker MediaPicker
		{
			get; 
			private set;
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name of the device.</value>
		public string Name
		{
			get { return DeviceStatus.DeviceName; }
		}

		/// <summary>
		/// Gets the firmware version.
		/// </summary>
		/// <value>The firmware version.</value>
		public string FirmwareVersion
		{
			get { return DeviceStatus.DeviceFirmwareVersion; }
		}

		/// <summary>
		/// Gets the hardware version.
		/// </summary>
		/// <value>The hardware version.</value>
		public string HardwareVersion
		{
			get { return DeviceStatus.DeviceHardwareVersion; }
		}

		/// <summary>
		/// Gets the manufacturer.
		/// </summary>
		/// <value>The manufacturer.</value>
		public string Manufacturer
		{
			get { return DeviceStatus.DeviceManufacturer; }
		}

		#endregion
	}
}
