using System;
using MonoTouch.UIKit;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace XForms.Toolkit
{
    public abstract class AppleDevice : IDevice
    {
        private const string iPhoneExpression = "iPhone([1-6]),([1-4])";
        private const string iPodExpression = "iPod([1-5]),([1])";
        private const string iPadExpression = "iPad([1-4]),([1-6])";

        private static IDevice device;

        [DllImport(MonoTouch.Constants.SystemLibrary)]
        static internal extern int sysctlbyname([MarshalAs(UnmanagedType.LPStr)] string property, IntPtr output, IntPtr oldLen, IntPtr newp, uint newlen);

        protected AppleDevice()
        {
//            this.BluetoothHub = new BluetoothHub ();
//            this.Battery = new BatteryImpl ();
            this.FirmwareVersion = UIDevice.CurrentDevice.SystemVersion;
        }

        public static IDevice CurrentDevice ()
        {
            if (device != null)
            {
                return device;
            }

            var hardwareVersion = GetSystemProperty ("hw.machine");


            var regex = new Regex (iPhoneExpression).Match(hardwareVersion);
            if (regex.Success)
            {
                return device = new Phone (int.Parse (regex.Groups [1].Value), int.Parse (regex.Groups [2].Value));
            }

            regex = new Regex (iPodExpression).Match (hardwareVersion);
            if (regex.Success)
            {
                return device = new Pod (int.Parse (regex.Groups [1].Value), int.Parse (regex.Groups [2].Value));
            }

            regex = new Regex (iPadExpression).Match (hardwareVersion);
            if (regex.Success)
            {
                return device = new Pad (int.Parse (regex.Groups [1].Value), int.Parse (regex.Groups [2].Value));
            }

            return device = new Simulator ();

            //

            //

            //
            //            if ([platform isEqualToString:@"i386"])         return @"Simulator";
            //            if ([platform isEqualToString:@"x86_64"])       return @"Simulator";
        }

        #region IDevice implementation

        public IDisplay Display
        {
            get;
            protected set;
        }

        public IPhoneService PhoneService
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public string FirmwareVersion
        {
            get;
            protected set;
        }

        public string HardwareVersion
        {
            get;
            protected set;
        }

        public string Manufacturer
        {
            get
            {
                return "Apple";
            }
        }
        #endregion

        public static string GetSystemProperty(string property)
        {
            var pLen = Marshal.AllocHGlobal(sizeof(int));
            sysctlbyname(property, IntPtr.Zero, pLen, IntPtr.Zero, 0);
            var length = Marshal.ReadInt32(pLen);
            var pStr = Marshal.AllocHGlobal(length);
            sysctlbyname(property, pStr, pLen, IntPtr.Zero, 0);
            return Marshal.PtrToStringAnsi(pStr);
        }
    }
}

