using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using System.ComponentModel;
using System.Drawing;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ExtendedScrollView), typeof(ExtendedScrollViewRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class ExtendedScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            this.Scrolled += (sender, ev) => {
                ExtendedScrollView sv = (ExtendedScrollView) Element;
                sv.UpdateBounds(this.Bounds.ToRectangle());
            };

            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }

        double EPSILON = 0.1;

        protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExtendedScrollView.PositionProperty.PropertyName) {
                ExtendedScrollView sv = (ExtendedScrollView)Element;
                Point pt = sv.Position;

                if (System.Math.Abs(this.Bounds.Location.Y - pt.Y) < EPSILON
                    && System.Math.Abs(this.Bounds.Location.X - pt.X) < EPSILON)
                    return;

                this.ScrollRectToVisible(
                    new RectangleF((float)pt.X, (float)pt.Y, Bounds.Width, Bounds.Height), sv.AnimateScroll);
            }
        }
    }
}

