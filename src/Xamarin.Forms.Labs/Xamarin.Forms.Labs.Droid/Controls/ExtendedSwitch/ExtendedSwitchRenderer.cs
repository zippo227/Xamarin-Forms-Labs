using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Droid.Controls;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace Xamarin.Forms.Labs.Droid.Controls
{

    using Xamarin.Forms.Platform.Android;
    using ToggleSwitch = Android.Widget.Switch;

    public class ExtendedSwitchRenderer : ViewRenderer<ExtendedSwitch, ToggleSwitch>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedSwitch> e)
        {
            if (this.Control == null)
            {
                var toggle = new ToggleSwitch(this.Context);
                toggle.CheckedChange += Control_ValueChanged;
                this.SetNativeControl(toggle);
            }

            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.Toggled -= Element_Toggled;
            }

            if (e.NewElement != null)
            {
                this.Control.Checked = e.NewElement.IsToggled;
                this.SetTintColor(this.Element.TintColor);
                this.Element.Toggled += Element_Toggled;
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "TintColor")
            {
                this.SetTintColor(this.Element.TintColor);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Control.CheckedChange -= this.Control_ValueChanged;
                this.Element.Toggled -= Element_Toggled;
            }

            base.Dispose(disposing);
        }

        private void SetTintColor(Color color)
        {
            var thumbStates = new StateListDrawable();
            thumbStates.AddState(new int[]{Android.Resource.Attribute.StateChecked}, new ColorDrawable(color.ToAndroid()));
            //thumbStates.AddState(new int[]{-android.R.attr.state_enabled}, new ColorDrawable(colorDisabled));
            //thumbStates.addState(new int[]{}, new ColorDrawable(this.app.colorOff)); // this one has to come last
            this.Control.ThumbDrawable = thumbStates;
        }

        private void Element_Toggled(object sender, ToggledEventArgs e)
        {
            this.Control.Checked = this.Element.IsToggled;
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            this.Element.IsToggled = this.Control.Checked;
        }
    }
}