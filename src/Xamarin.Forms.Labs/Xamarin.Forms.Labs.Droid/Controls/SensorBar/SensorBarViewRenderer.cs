using System;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Labs;
using Xamarin.Forms.Labs.Droid;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(SensorBarView), typeof(SensorBarViewRenderer))]
namespace Xamarin.Forms.Labs.Droid
{
    public class SensorBarViewRenderer : ViewRenderer<SensorBarView, SensorBarDroidView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SensorBarView> e)
        {
            base.OnElementChanged (e);

            if (e.NewElement == null)
            {
                return;
            }

            if (this.Control == null)
            {
                this.SetNativeControl(new SensorBarDroidView(this.Context));
            }

            this.SetProperties();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            this.SetProperties();
        }

        private void SetProperties()
        {
            this.Control.CurrentValue = this.Element.CurrentValue;
            this.Control.Limit = this.Element.Limit;
            this.Control.NegativeColor = this.Element.NegativeColor.ToAndroid();
            this.Control.PositiveColor = this.Element.PositiveColor.ToAndroid();

            this.Control.Invalidate();
        }
    }
}

