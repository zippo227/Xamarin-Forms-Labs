using System;
using System.Windows.Media;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Platform.WinPhone;
using XamarinEvolve.Mobile.WinPhone.Renderers;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]
namespace XamarinEvolve.Mobile.WinPhone.Renderers
{

    public class CircleImageRenderer : ImageRenderer
    {

        public CircleImageRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control != null && Control.Clip == null)
            {
                var min = Math.Min(Element.Width, Element.Height) / 2.0f;

                if (min <= 0)
                    return;

                Control.Clip = new EllipseGeometry
                {
                    Center = new System.Windows.Point(min, min),
                    RadiusX = min,
                    RadiusY = min
                };
            }
        }
    }
}

