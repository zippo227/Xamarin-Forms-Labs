using System;
using System.Drawing;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(Separator), typeof(SeparatorRenderer))]

namespace Xamarin.Forms.Labs.iOS
{
    public class SeparatorRenderer : ViewRenderer<Separator,UISeparator>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Separator> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (this.Control == null)
            {
                this.BackgroundColor = Color.Transparent.ToUIColor();
#if __UNIFIED__
                this.SetNativeControl(new UISeparator(new RectangleF((float)this.Bounds.X, (float)this.Bounds.Y, (float)this.Bounds.Width, (float)this.Bounds.Height)));
#elif __IOS__
                this.SetNativeControl(new UISeparator(this.Bounds));
#endif
            }

            this.SetProperties ();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            this.SetProperties();
        }

        private void SetProperties()
        {
            var separator = Control;
            separator.Thickness = Element.Thickness;
            separator.StrokeColor = Element.Color.ToUIColor();
            separator.StrokeType = Element.StrokeType;
            separator.Orientation = Element.Orientation;
            separator.SpacingBefore = Element.SpacingBefore;
            separator.SpacingAfter = Element.SpacingAfter;
        }
    }
}

