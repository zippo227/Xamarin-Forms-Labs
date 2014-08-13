using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Charting.Controls;
using Xamarin.Forms.Labs.Charting.Droid.Controls;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Chart), typeof(ChartRenderer))]
namespace Xamarin.Forms.Labs.Charting.Droid.Controls
{
    public class ChartRenderer : ViewRenderer<Chart, ChartSurface>
    {

        protected override void OnElementChanged(ElementChangedEventArgs<Chart> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;

            Android.Graphics.Color[] colors = new Android.Graphics.Color[Element.Series.Count];

            for (int i = 0; i < Element.Series.Count; i++)
            {
                colors[i] = Element.Series[i].Color.ToAndroid();
            }

            ChartSurface surfaceView = new ChartSurface(Forms.Context, Element, Element.Color.ToAndroid(), colors);
            SetNativeControl(surfaceView);
        }
    }
}