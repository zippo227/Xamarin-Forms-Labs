using System.ComponentModel;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs;

[assembly: ExportRenderer(typeof(SensorBarView), typeof(SensorBarViewRenderer))]

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The sensor bar view renderer.
    /// </summary>
    public class SensorBarViewRenderer : ViewRenderer<SensorBarView, UISensorBar>
    {
        private UISensorBar sensorBar;

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<SensorBarView> e)
        {
            base.OnElementChanged(e);

#if __UNIFIED__
            this.sensorBar = new UISensorBar(new RectangleF((float)this.Bounds.X, (float)this.Bounds.Y, (float)this.Bounds.Width, (float)this.Bounds.Height));
#elif __IOS__
            this.sensorBar = new UISensorBar(this.Bounds);
#endif
            this.SetProperties ();
            this.Element.PropertyChanged += this.OnPropertyChanged;

            this.SetNativeControl(this.sensorBar);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.SetProperties ();
        }

        private void SetProperties()
        {
            this.sensorBar.CurrentValue = this.Element.CurrentValue;
            this.sensorBar.Limit = this.Element.Limit;
            this.sensorBar.NegativeColor = this.Element.NegativeColor.ToUIColor();
            this.sensorBar.PositiveColor = this.Element.PositiveColor.ToUIColor();
        }
    }
}

