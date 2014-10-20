using System;
using Xamarin.Forms.Labs.Services.Media;

namespace Xamarin.Forms.Labs.Controls
{
    public class CameraView : View
    {
        public CameraView()
        {
        }

        /// <summary>
        /// The camera device to use.
        /// </summary>
        public static readonly BindableProperty CameraProperty =
            BindableProperty.Create<CameraView, CameraDevice>(
                p => p.Camera, CameraDevice.Rear);

        /// <summary>
        /// Gets or sets the camera device to use.
        /// </summary>
        public CameraDevice Camera
        {
            get { return this.GetValue<CameraDevice>(CameraProperty); }
            set { this.SetValue(CameraProperty, value); }
        }
    }
}

