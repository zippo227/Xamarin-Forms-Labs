using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Info;

namespace XForms.Toolkit.Device
{
    public class Display : IDisplay
    {
        public Display()
        {
            object physicalScreenResolutionObject;

            if (DeviceExtendedProperties.TryGetValue("PhysicalScreenResolution", out physicalScreenResolutionObject))
            {
                var physicalScreenResolution = (Size)physicalScreenResolutionObject;
                this.Height = (int)physicalScreenResolution.Height;
                this.Width = (int)physicalScreenResolution.Width;
            }
            else
            {
                var scaleFactor = Application.Current.Host.Content.ScaleFactor;
                this.Height = (int)(Application.Current.Host.Content.ActualHeight * scaleFactor);
                this.Width = (int)(Application.Current.Host.Content.ActualWidth * scaleFactor);
            }

            object rawDpiX, rawDpiY;

            if (DeviceExtendedProperties.TryGetValue("RawDpiX", out rawDpiX))
            {
                this.Xdpi = (double)rawDpiX;
            }

            if (DeviceExtendedProperties.TryGetValue("RawDpiY", out rawDpiY))
            {
                this.Ydpi = (double)rawDpiY;
            }
        }

        #region IDisplay Members

        public int Height
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public double Xdpi
        {
            get;
            private set;
        }

        public double Ydpi
        {
            get;
            private set;
        }

        #endregion
    }
}
