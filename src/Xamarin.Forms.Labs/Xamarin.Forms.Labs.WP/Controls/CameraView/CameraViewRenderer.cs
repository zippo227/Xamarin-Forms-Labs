using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.WP8.Controls;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(CameraView), typeof(CameraViewRenderer))]

namespace Xamarin.Forms.Labs.WP8.Controls
{
    public class CameraViewRenderer : ViewRenderer<CameraView, Canvas>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CameraView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var videoBrush = new VideoBrush();
                // TODO: determine how to dispose the camera...
                var camera = new PhotoCamera();

                videoBrush.SetSource(camera);

                var canvas = new Canvas()
                {
                    Background = videoBrush
                };

                this.SetNativeControl(canvas);
            }
        }


    }
}
