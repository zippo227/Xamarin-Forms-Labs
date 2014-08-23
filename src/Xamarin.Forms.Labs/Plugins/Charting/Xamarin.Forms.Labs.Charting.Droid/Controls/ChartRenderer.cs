using Android.Graphics;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Charting.Controls;
using Xamarin.Forms.Labs.Charting.Droid.Controls;
using Xamarin.Forms.Platform.Android;
using System.Linq;

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

            // Use color specified at DataPoints if it is a Pie Chart
            Android.Graphics.Color[] colors;
            Series pieSeries = Element.Series.FirstOrDefault(s => s.Type == ChartType.Pie);
            if (pieSeries != null)
            {
                colors = new Android.Graphics.Color[pieSeries.Points.Count];
                for (int i = 0; i < pieSeries.Points.Count; i++)
                {
                    colors[i] = pieSeries.Points[i].Color.ToAndroid();
                }
            }
            else
            {
                colors = new Android.Graphics.Color[Element.Series.Count];
                for (int i = 0; i < Element.Series.Count; i++)
                {
                    colors[i] = Element.Series[i].Color.ToAndroid();
                }
            }


            ChartSurface surfaceView = new ChartSurface(Forms.Context, Element, Element.Color.ToAndroid(), colors);
            SetNativeControl(surfaceView);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (this.Element == null || this.Control == null)
                return;

            if(e.PropertyName == Chart.ColorProperty.PropertyName)
            {
                Control.Paint = new Paint() { Color = Element.Color.ToAndroid(), StrokeWidth = 2 };
                Control.Invalidate();
            }
            else if(e.PropertyName == Chart.GridProperty.PropertyName)
            {
                Control.Chart.Grid = Element.Grid;
                Control.Invalidate();
            }
            else if(e.PropertyName == Chart.HeightProperty.PropertyName)
            {
                Control.Chart.Height = Element.Height;
                Control.Invalidate();
            }
            else if(e.PropertyName == Chart.SeriesProperty.PropertyName)
            {
                Control.Chart.Series = Element.Series;
                Control.Invalidate();
            }
            else if(e.PropertyName == Chart.SpacingProperty.PropertyName)
            {
                Control.Chart.Spacing = Element.Spacing;
                Control.Invalidate();
            }
            else if(e.PropertyName == Chart.WidthProperty.PropertyName)
            {
                Control.Chart.WidthRequest = Element.Width;
                Control.Invalidate();
            }
        }
    }
}