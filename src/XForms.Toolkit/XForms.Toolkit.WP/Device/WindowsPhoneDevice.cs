using Microsoft.Phone.Info;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Media;
using XForms.Toolkit.Services.Geolocation;
using XForms.Toolkit.WP.Services.Media;

namespace XForms.Toolkit
{
	/// <summary>
	/// Windows phone device.
	/// </summary>
	public class WindowsPhoneDevice : IDevice
	{
		private static WindowsPhoneDevice _currentDevice;

		/// <summary>
		/// Initializes a new instance of the <see cref="XForms.Toolkit.WindowsPhoneDevice"/> class.
		/// </summary>
		private WindowsPhoneDevice()
		{
			Display = new Display();
			PhoneService = new PhoneService();
			Battery = new Battery();
			MediaPicker = new MediaPicker();
		}

		/// <summary>
		/// Gets the current device.
		/// </summary>
		public static IDevice CurrentDevice { get { return _currentDevice ?? (_currentDevice = new WindowsPhoneDevice()); } }

		#region IDevice Members
		/// <summary>
		/// Gets the display.
		/// </summary>
		public IDisplay Display
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the phone service.
		/// </summary>
		public IPhoneService PhoneService
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the battery.
		/// </summary>
		public IBattery Battery 
		{
			get;
			private set;
		}

		public IGeolocator Geolocator { get; private set; }

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
		public string Name
		{
			get { return DeviceStatus.DeviceName; }
		}

		/// <summary>
		/// Gets the firmware version.
		/// </summary>
		public string FirmwareVersion
		{
			get { return DeviceStatus.DeviceFirmwareVersion; }
		}

		/// <summary>
		/// Gets the hardware version.
		/// </summary>
		public string HardwareVersion
		{
			get { return DeviceStatus.DeviceHardwareVersion; }
		}

		/// <summary>
		/// Gets the manufacturer.
		/// </summary>
		public string Manufacturer
		{
			get { return DeviceStatus.DeviceManufacturer; }
		}

		#endregion
	}
}
