using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class ExtendedSwitchRenderer : ViewRenderer<ExtendedSwitch, UISwitch>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedSwitch> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.Toggled -= Element_Toggled;
            }

            if (e.NewElement != null)
            {
                this.SetNativeControl(new UISwitch());
                this.Control.On = e.NewElement.IsToggled;
                this.Control.ValueChanged += Control_ValueChanged;
                this.SetTintColor(this.Element.TintColor.ToUIColor());
                this.Element.Toggled += Element_Toggled;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "TintColor")
            {
                this.SetTintColor(this.Element.TintColor.ToUIColor());
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Control.ValueChanged -= this.Control_ValueChanged;
                this.Element.Toggled -= Element_Toggled;
            }

            base.Dispose(disposing);
        }

        private void SetTintColor(UIColor color)
        {
            this.Control.TintColor = color;
            //this.Control.ThumbTintColor = color;
            this.Control.OnTintColor = color;
        }

        private void Element_Toggled(object sender, ToggledEventArgs e)
        {
            this.Control.SetState(this.Element.IsToggled, true);
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            this.Element.IsToggled = this.Control.On;
        }
    }
}