using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using Xamarin.Forms;
using XForms.Toolkit;
using XForms.Toolkit.Controls;

[assembly: ExportRenderer(typeof(SensorBarView), typeof(SensorBarViewRenderer))]
namespace XForms.Toolkit.Controls
{
    public class SensorBarViewRenderer : ViewRenderer<SensorBarView, UISensorBar>
    {
        private UISensorBar sensorBar;

        protected override void OnElementChanged(ElementChangedEventArgs<SensorBarView> e)
        {
            base.OnElementChanged(e);

            this.sensorBar = new UISensorBar(this.Bounds);
            this.SetProperties ();
            this.Element.PropertyChanged += OnPropertyChanged;

            base.SetNativeControl(this.sensorBar);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetProperties ();
        }

        private void SetProperties()
        {
            this.sensorBar.CurrentValue = this.Element.CurrentValue;
            this.sensorBar.Limit = this.Element.Limit;
            this.sensorBar.NegativeColor = this.Element.NegativeColor.ToUIColor ();
            this.sensorBar.PositiveColor = this.Element.PositiveColor.ToUIColor ();
        }
    }
}

