using System.Linq;

using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Charting.Controls;
using Xamarin.Forms.Labs.Charting.iOS.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Chart), typeof(ChartRenderer))]
namespace Xamarin.Forms.Labs.Charting.iOS.Controls
{
    public class ChartRenderer : ViewRenderer<Chart, ChartSurface>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Chart> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;

            // Use color specified at DataPoints if it is a Pie Chart
            UIColor[] colors;
            Series pieSeries = Element.Series.FirstOrDefault(s => s.Type == ChartType.Pie);
            if (pieSeries != null)
            {
                colors = new UIColor[pieSeries.Points.Count];
                for (int i = 0; i < pieSeries.Points.Count; i++)
                {
                    colors[i] = pieSeries.Points[i].Color.ToUIColor();
                }
            }
            else
            {
                colors = new UIColor[Element.Series.Count];
                for (int i = 0; i < Element.Series.Count; i++)
                {
                    colors[i] = Element.Series[i].Color.ToUIColor();
                }
            }


            ChartSurface surfaceView = new ChartSurface(Element, Element.Color.ToUIColor(), colors);
            SetNativeControl(surfaceView);
        }
    }
}