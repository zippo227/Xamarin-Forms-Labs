// ***********************************************************************
// Assembly         : XLabs.Platform.iOS
// Author           : XLabs Team
// Created          : 12-27-2015
// 
// Last Modified By : XLabs Team
// Last Modified On : 01-04-2016
// ***********************************************************************
// <copyright file="AppleDevice.cs" company="XLabs Team">
//     Copyright (c) XLabs Team. All rights reserved.
// </copyright>
// <summary>
//       This project is licensed under the Apache 2.0 license
//       https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/LICENSE
//       
//       XLabs is a open source project that aims to provide a powerfull and cross 
//       platform set of controls tailored to work with Xamarin Forms.
// </summary>
// ***********************************************************************
// 

using System;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Foundation;
using ObjCRuntime;
using UIKit;
using XLabs.Enums;
using XLabs.Platform.Services;
using XLabs.Platform.Services.IO;
using XLabs.Platform.Services.Media;

namespace XLabs.Platform.Device
{
	/// <summary>
    /// Apple device base class.
    /// </summary>
    public abstract class AppleDevice : IDevice
    {
        /// <summary>
        /// The iPhone expression.
        /// </summary>
        protected const string PhoneExpression = "iPhone([1-8]),([1-4])";

        /// <summary>
        /// The iPod expression.
        /// </summary>
        protected const string PodExpression = "iPod([1-5]),([1])";

        /// <summary>
        /// The iPad expression.
        /// </summary>
        protected const string PadExpression = "iPad([1-4]),([1-8])";

        /// <summary>
        /// Generic CPU/IO.
        /// </summary>
        private const int CtlHw = 6;

        /// <summary>
        /// Total memory.
        /// </summary>
        private const int HwPhysmem = 5;

        /// <summary>
        /// The device.
        /// </summary>
        private static IDevice device;

        private static readonly long DeviceTotalMemory = GetTotalMemory();

        /// <summary>
        /// The file manager
        /// </summary>
        private IFileManager fileManager;

        /// <summary>
        /// Reference to the Bluetooth hub singleton.
        /// </summary>
        private IBluetoothHub bluetoothHub;

        /// <summary>
        /// Microphone audio stream.
        /// </summary>
        private IAudioStream microphone;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppleDevice" /> class.
        /// </summary>
        protected AppleDevice()
        {
            this.Battery = new Battery();
            this.Accelerometer = new Accelerometer();
            this.FirmwareVersion = UIDevice.CurrentDevice.SystemVersion;

            if (Device.Gyroscope.IsSupported)
            {
                this.Gyroscope = new Gyroscope();
            }

            this.MediaPicker = new MediaPicker();

            this.Network = new Network();
        }

        /// <summary>
        /// Gets the runtime device for Apple's devices.
        /// </summary>
        /// <value>The current device.</value>
        public static IDevice CurrentDevice
        {
            get
            {
                if (device != null)
                {
                    return device;
                }

                var deviceInfo = GetDeviceInfo();

                switch (deviceInfo.Type)
                {
                    case DeviceType.Phone:
                        return device = new Phone(deviceInfo.MajorVersion, deviceInfo.MinorVersion);
                    case DeviceType.Pad:
                        return device = new Pad(deviceInfo.MajorVersion, deviceInfo.MinorVersion);
                    case DeviceType.Pod:
                        return device = new Pod(deviceInfo.MajorVersion, deviceInfo.MinorVersion);
                }

                return device = new Simulator();
            }

            set
            {
                device = value;
            }
        }

        /// <summary>
        /// Sysctlbynames the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="output">The output.</param>
        /// <param name="oldLen">The old length.</param>
        /// <param name="newp">The newp.</param>
        /// <param name="newlen">The newlen.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(Constants.SystemLibrary)]
        internal static extern int sysctlbyname(
            [MarshalAs(UnmanagedType.LPStr)] string property,
            IntPtr output,
            IntPtr oldLen,
            IntPtr newp,
            uint newlen);

        [DllImport(Constants.SystemLibrary)]
        static internal extern int sysctl(
            [MarshalAs(UnmanagedType.LPArray)] int[] name, 
            uint namelen, 
            out uint oldp, 
            ref int oldlenp, 
            IntPtr newp, 
            uint newlen);


        /// <summary>
        /// Gets the system property.
        /// </summary>
        /// <param name="property">Property to get.</param>
        /// <returns>The system property value.</returns>
        public static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }

        #region IDevice implementation

        /// <summary>
        /// Gets Unique Id for the device.
        /// </summary>
        /// <value>The id for the device.</value>
        public virtual string Id
        {
            get
            {
                return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            }
        }

        /// <summary>
        /// Gets or sets the display information for the device.
        /// </summary>
        /// <value>The display.</value>
        public IDisplay Display { get; protected set; }

        /// <summary>
        /// Gets or sets the phone service for this device.
        /// </summary>
        /// <value>Phone service instance if available, otherwise null.</value>
        public IPhoneService PhoneService { get; protected set; }

        /// <summary>
        /// Gets or sets the battery for the device.
        /// </summary>
        /// <value>The battery.</value>
        public IBattery Battery { get; protected set; }

        /// <summary>
        /// Gets the picture chooser.
        /// </summary>
        /// <value>The picture chooser.</value>
        public IMediaPicker MediaPicker { get; private set; }

        /// <summary>
        /// Gets the network service.
        /// </summary>
        /// <value>The network service.</value>
        public INetwork Network { get; private set; }

        /// <summary>
        /// Gets or sets the accelerometer for the device if available
        /// </summary>
        /// <value>Instance of IAccelerometer if available, otherwise null.</value>
        public IAccelerometer Accelerometer { get; protected set; }

        /// <summary>
        /// Gets the gyroscope.
        /// </summary>
        /// <value>The gyroscope instance if available, otherwise null.</value>
        public IGyroscope Gyroscope { get; private set; }

        /// <summary>
        /// Gets the audio stream from the device's microphone.
        /// </summary>
        public IAudioStream Microphone
        {
            get
            {
                return this.microphone ?? (this.microphone = new Microphone());
            }
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
        /// <value>The name of the device.</value>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        /// <value>The firmware version.</value>
        public string FirmwareVersion { get; protected set; }

        /// <summary>
        /// Gets or sets the hardware version.
        /// </summary>
        /// <value>The hardware version.</value>
        public string HardwareVersion { get; protected set; }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <value>The manufacturer.</value>
        public string Manufacturer
        {
            get
            {
                return "Apple";
            }
        }

        /// <summary>
        /// Gets the bluetooth hub service.
        /// </summary>
        /// <value>The bluetooth hub service if available, otherwise null.</value>
        public IBluetoothHub BluetoothHub
        {
            get
            {
                return this.bluetoothHub ?? (this.bluetoothHub = new BluetoothHub());
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
                return DeviceTotalMemory; 
            }
        }

        /// <summary>
        /// Gets the language code.
        /// </summary>
        /// <value>The language code.</value>
        public string LanguageCode
        {
            get { return NSLocale.PreferredLanguages[0]; }
        }

        /// <summary>
        /// Gets the time zone offset.
        /// </summary>
        /// <value>The time zone offset.</value>
        public double TimeZoneOffset
        {
            get { return NSTimeZone.LocalTimeZone.GetSecondsFromGMT / 3600.0; }
        }

        /// <summary>
        /// Gets the time zone.
        /// </summary>
        /// <value>The time zone.</value>
        public string TimeZone
        {
            get { return NSTimeZone.LocalTimeZone.Name; }
        }

        /// <summary>
        /// Gets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public Orientation Orientation
        {
            get
            {
                switch (UIApplication.SharedApplication.StatusBarOrientation)
                {
                    case UIInterfaceOrientation.LandscapeLeft:
                        return Orientation.Landscape & Orientation.LandscapeLeft;
                    case UIInterfaceOrientation.Portrait:
                        return Orientation.Portrait & Orientation.PortraitUp;
                    case UIInterfaceOrientation.PortraitUpsideDown:
                        return Orientation.Portrait & Orientation.PortraitDown;
                    case UIInterfaceOrientation.LandscapeRight:
                        return Orientation.Landscape & Orientation.LandscapeRight;
                    default:
                        return Orientation.None;
                }
            }
        }

        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        public Task<bool> LaunchUriAsync(Uri uri)
        {
            var launchTaskSource = new TaskCompletionSource<bool>();
            var app = UIApplication.SharedApplication;
            app.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var url = NSUrl.FromString(uri.ToString()) ?? new NSUrl(uri.Scheme, uri.Host, uri.LocalPath);
                    var result = app.CanOpenUrl(url) && app.OpenUrl(url);
                    launchTaskSource.SetResult(result);
                }
                catch (Exception exception)
                {
                    launchTaskSource.SetException(exception);
                }
            });

            return launchTaskSource.Task;
        }
        #endregion

        /// <summary>
        /// Gets the hardware's <see cref="DeviceInfo"/>.
        /// </summary>
        /// <returns><see cref="DeviceInfo"/></returns>
        /// <exception cref="Exception">Throws an exception if unable to determine device type.</exception>
        public static DeviceInfo GetDeviceInfo()
        {
            var hardwareVersion = GetSystemProperty("hw.machine");

            var regex = new Regex(PhoneExpression).Match(hardwareVersion);
            if (regex.Success)
            {
                return new DeviceInfo(DeviceType.Phone, int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
            }

            regex = new Regex(PodExpression).Match(hardwareVersion);
            if (regex.Success)
            {
                return new DeviceInfo(DeviceType.Pod, int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
            }

            regex = new Regex(PadExpression).Match(hardwareVersion);
            if (regex.Success)
            {
                return new DeviceInfo(DeviceType.Pad, int.Parse(regex.Groups[1].Value), int.Parse(regex.Groups[2].Value));
            }

            return new DeviceInfo(DeviceType.Simulator, 0, 0);
        }

        private static uint GetTotalMemory()
        {
            var oldlenp = sizeof(int);
            var mib = new int[2] { CtlHw, HwPhysmem };

            uint mem;
            sysctl(mib, 2, out mem, ref oldlenp, IntPtr.Zero, 0);

            return mem;
        }
    }
}