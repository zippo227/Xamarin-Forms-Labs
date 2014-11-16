namespace XLabs.Platform.Device
{
	using System;
	using System.IO.IsolatedStorage;
	using System.Threading.Tasks;

	using Android.Bluetooth;
	using Android.Content;
	using Android.OS;
	using Android.Telephony;
	using Android.Util;

	using Xamarin.Forms.Labs.Services.IO;

	using XLabs.Platform.Services;
	using XLabs.Platform.Services.IO;
	using XLabs.Platform.Services.Media;

	/// <summary>
	///     Android device implements <see cref="IDevice" />.
	/// </summary>
	public class AndroidDevice : IDevice
	{
		private static IDevice currentDevice;

		private IBluetoothHub _btHub;

		private IFileManager _fileManager;

		/// <summary>
		///     Prevents a default instance of the <see cref="AndroidDevice" /> class from being created.
		/// </summary>
		private AndroidDevice()
		{
			var manager = Services.PhoneService.Manager;

			if (manager != null && manager.PhoneType != PhoneType.None)
			{
				PhoneService = new PhoneService();
			}

			if (Device.Accelerometer.IsSupported)
			{
				Accelerometer = new Accelerometer();
			}

			if (Device.Gyroscope.IsSupported)
			{
				Gyroscope = new Gyroscope();
			}

			//            if (BluetoothAdapter.DefaultAdapter != null)
			//            {
			//                this.BluetoothHub = new BluetoothHub(BluetoothAdapter.DefaultAdapter);
			//            }

			if (Services.Media.Microphone.IsEnabled)
			{
				Microphone = new Microphone();
			}

			Display = new Display();

			Manufacturer = Build.Manufacturer;
			Name = Build.Model;
			HardwareVersion = Build.Hardware;
			FirmwareVersion = Build.VERSION.Release;

			Battery = new Battery();

			MediaPicker = new MediaPicker();

			Network = new Network();
		}

		/// <summary>
		///     Gets the current device.
		/// </summary>
		/// <value>
		///     The current device.
		/// </value>
		public static IDevice CurrentDevice
		{
			get
			{
				return currentDevice ?? (currentDevice = new AndroidDevice());
			}
		}

		#region IDevice implementation

		/// <summary>
		///     Gets Unique Id for the device.
		/// </summary>
		/// <value>
		///     The id for the device.
		/// </value>
		public string Id
		{
			// TODO: Verify what is the best combination of Unique Id for Android
			get
			{
				return Build.Serial;
			}
		}

		/// <summary>
		///     Gets the phone service for this device.
		/// </summary>
		/// <value>Phone service instance if available, otherwise null.</value>
		public IPhoneService PhoneService { get; private set; }

		/// <summary>
		///     Gets the display information for the device.
		/// </summary>
		/// <value>
		///     The display.
		/// </value>
		public IDisplay Display { get; private set; }

		/// <summary>
		///     Gets the battery.
		/// </summary>
		/// <value>
		///     The battery.
		/// </value>
		public IBattery Battery { get; private set; }

		/// <summary>
		///     Gets the picture chooser.
		/// </summary>
		/// <value>The picture chooser.</value>
		public IMediaPicker MediaPicker { get; private set; }

		/// <summary>
		///     Gets the network service.
		/// </summary>
		/// <value>The network service.</value>
		public INetwork Network { get; private set; }

		/// <summary>
		///     Gets the accelerometer for the device if available.
		/// </summary>
		/// <value>Instance of IAccelerometer if available, otherwise null.</value>
		public IAccelerometer Accelerometer { get; private set; }

		/// <summary>
		///     Gets the gyroscope.
		/// </summary>
		/// <value>The gyroscope instance if available, otherwise null.</value>
		public IGyroscope Gyroscope { get; private set; }

		/// <summary>
		///     Gets the bluetooth hub service.
		/// </summary>
		/// <value>The bluetooth hub service if available, otherwise null.</value>
		public IBluetoothHub BluetoothHub
		{
			get
			{
				if (_btHub == null && BluetoothAdapter.DefaultAdapter != null)
				{
					_btHub = new BluetoothHub(BluetoothAdapter.DefaultAdapter);
				}

				return _btHub;
			}
		}

		/// <summary>
		///     Gets the default microphone for the device
		/// </summary>
		public IAudioStream Microphone { get; private set; }

		/// <summary>
		///     Gets the file manager for the device.
		/// </summary>
		/// <value>Device file manager.</value>
		public IFileManager FileManager
		{
			get
			{
				return _fileManager ?? (_fileManager = new FileManager(IsolatedStorageFile.GetUserStoreForApplication()));
			}
		}

		/// <summary>
		///     Gets the name of the device.
		/// </summary>
		/// <value>
		///     The name of the device.
		/// </value>
		public string Name { get; private set; }

		/// <summary>
		///     Gets the firmware version.
		/// </summary>
		/// <value>
		///     The firmware version.
		/// </value>
		public string FirmwareVersion { get; private set; }

		/// <summary>
		///     Gets the hardware version.
		/// </summary>
		/// <value>
		///     The hardware version.
		/// </value>
		public string HardwareVersion { get; private set; }

		/// <summary>
		///     Gets the manufacturer.
		/// </summary>
		/// <value>
		///     The manufacturer.
		/// </value>
		public string Manufacturer { get; private set; }

		/// <summary>
		///     Starts the default app associated with the URI for the specified URI.
		/// </summary>
		/// <param name="uri">The URI.</param>
		/// <returns>The launch operation.</returns>
		public Task<bool> LaunchUriAsync(Uri uri)
		{
			return Task.Run(
				() =>
					{
						try
						{
							Xamarin.Forms.Context.StartActivity(
								new Intent("android.intent.action.VIEW", Android.Net.Uri.Parse(uri.ToString())));
							return true;
						}
						catch (Exception ex)
						{
							Log.Error("Device.LaunchUriAsync", ex.Message);
							return false;
						}
					});
		}

		#endregion
	}
}