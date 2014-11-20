using System;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Labs.Services.Media;
using Xamarin.Forms.Labs.iOS.Services.Media;
using Xamarin.Forms.Labs.iOS.Services;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using Xamarin.Forms.Labs.Services.IO;
using System.IO.IsolatedStorage;

namespace Xamarin.Forms.Labs
{
    /// <summary>
    /// Apple device base class.
    /// </summary>
    public abstract class AppleDevice : IDevice
    {
        private const string iPhoneExpression = "iPhone([1-7]),([1-4])";
        private const string iPodExpression = "iPod([1-5]),([1])";
        private const string iPadExpression = "iPad([1-4]),([1-6])";
        private const int CTL_HW = 6; /* generic cpu/io */
        private const int HW_PHYSMEM = 5; /* int: total memory */

        private static IDevice device;

        private IFileManager fileManager;

        private static readonly long totalMemory = GetTotalMemory();
		
        [DllImport(MonoTouch.Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

        [DllImport(MonoTouch.Constants.SystemLibrary)]
        static internal extern int sysctl([MarshalAs(UnmanagedType.LPArray)] int[] name, uint namelen, out uint oldp, ref int oldlenp, IntPtr newp, uint newlen);
		
        /// <summary>
        /// Initializes a new instance of the <see cref="Xamarin.Forms.Labs.AppleDevice"/> class.
        /// </summary>
        protected AppleDevice()
        {
            this.Battery = new Battery();
            this.Accelerometer = new Accelerometer();
            this.FirmwareVersion = UIDevice.CurrentDevice.SystemVersion;
            //this.BluetoothHub = new BluetoothHub();

            if (Labs.Gyroscope.IsSupported)
            {
                this.Gyroscope = new Gyroscope();
            }

            this.MediaPicker = new MediaPicker();

            this.Network = new Network();
        }

        /// <summary>
        /// Gets the runtime device for Apple's devices.
        /// </summary>
        /// <value>
        /// The current device.
        /// </value>
        public static IDevice CurrentDevice
        {
            get
            {
                if (device != null)
                {
                    return device;
                }

                var hardwareVersion = GetSystemProperty("hw.machine");

                var regex = new Regex(iPhoneExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Phone(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(iPodExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pod(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                regex = new Regex(iPadExpression).Match(hardwareVersion);
                if (regex.Success)
                {
                    return device = new Pad(int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
                }

                return device = new Simulator();
            }
        }

        #region IDevice implementation
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
               return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            }
        }

        /// <summary>
        /// Gets or sets the display information for the device.
        /// </summary>
        /// <value>
        /// The display.
        /// </value>
        public IDisplay Display
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the battery for the device.
        /// </summary>
        /// <value>
        /// The battery.
        /// </value>
        public IBattery Battery
        {
            get;
            protected set;
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
        /// Gets or sets the accelerometer for the device if available
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer
        {
            get;
            protected set;
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
        //public IBluetoothHub BluetoothHub
        //{
        //    get;
        //    private set;
        //}

        /// <summary>
        /// Gets the default microphone for the device
        /// </summary>
        public IAudioStream Microphone
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the file manager for the device.
        /// </summary>
        /// <value>Device file manager.</value>
        public IFileManager FileManager
        {
            get
            {
                return this.fileManager ?? (this.fileManager = new FileManager(IsolatedStorageFile.GetUserStoreForApplication()));
            }
        }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the hardware version.
        /// </summary>
        public string HardwareVersion
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        public string Manufacturer
        {
            get
            {
                return "Apple";
            }
        }

        /// <summary>
        /// Gets the total memory in bytes.
        /// </summary>
        /// <value>The total memory in bytes.</value>
        public long TotalMemory 
        {
            get 
            { 
                return totalMemory; 
            }
        }

        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        public Task<bool> LaunchUriAsync(Uri uri)
        {
            return Task.Run(() => UIApplication.SharedApplication.OpenUrl(new NSUrl(uri.ToString())));
        }
        #endregion

        /// <summary>
        /// Gets the system property.
        /// </summary>
        /// <returns>The system property value.</returns>
        /// <param name="property">Property to get.</param>
        public static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }

        private static uint GetTotalMemory() 
        {
            var oldlenp = sizeof(int);
            var mib = new int[2] { CTL_HW, HW_PHYSMEM };

            uint mem;
            sysctl(mib, 2, out mem, ref oldlenp, IntPtr.Zero, 0);

            return mem;
        }
    }
}

