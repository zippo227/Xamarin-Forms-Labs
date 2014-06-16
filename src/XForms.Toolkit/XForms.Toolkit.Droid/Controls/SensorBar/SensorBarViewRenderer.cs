using System;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using Xamarin.Forms;
using XForms.Toolkit;
using XForms.Toolkit.Droid;

[assembly: ExportRenderer(typeof(SensorBarView), typeof(SensorBarViewRenderer))]
namespace XForms.Toolkit.Droid
{
    public class SensorBarViewRenderer : ViewRenderer<SensorBarView, SensorBarDroidView>
    {
        private SensorBarDroidView sensorBar;

        protected override void OnElementChanged(ElementChangedEventArgs<SensorBarView> e)
        {
            base.OnElementChanged (e);

            this.sensorBar = new SensorBarDroidView (this.Context);

            this.SetProperties ();
            this.Element.PropertyChanged += OnPropertyChanged;

            this.SetNativeControl (this.sensorBar);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetProperties ();
        }

        private void SetProperties()
        {
            this.sensorBar.CurrentValue = this.Element.CurrentValue;
            this.sensorBar.Limit = this.Element.Limit;
            this.sensorBar.NegativeColor = this.Element.NegativeColor.ToAndroid ();
            this.sensorBar.PositiveColor = this.Element.PositiveColor.ToAndroid ();

            this.sensorBar.Invalidate ();
        }
    }
}

