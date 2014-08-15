using Xamarin.Forms;
using Xamarin.Forms.Labs.Charting.Controls;
using Xamarin.Forms.Labs.Charting.WP.Controls;
using Xamarin.Forms.Platform.WinPhone;
using System.Linq;

[assembly: ExportRenderer(typeof(Chart), typeof(ChartRenderer))]
namespace Xamarin.Forms.Labs.Charting.WP.Controls
{
    public class ChartRenderer : ViewRenderer<Chart, ChartSurface>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Chart> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;


            // Use color specified at DataPoints if it is a Pie Chart
            System.Windows.Media.Color[] colors;
            Series pieSeries = Element.Series.FirstOrDefault(s => s.Type == ChartType.Pie);
            if (pieSeries != null)
            {
                colors = new System.Windows.Media.Color[pieSeries.Points.Count];
                for (int i = 0; i < pieSeries.Points.Count; i++)
                {
                    colors[i] = System.Windows.Media.Color.FromArgb(
                       (byte)(pieSeries.Points[i].Color.A * 255),
                       (byte)(pieSeries.Points[i].Color.R * 255),
                       (byte)(pieSeries.Points[i].Color.G * 255),
                       (byte)(pieSeries.Points[i].Color.B * 255));
                }
            }
            else
            {
                colors = new System.Windows.Media.Color[Element.Series.Count];
                for (int i = 0; i < Element.Series.Count; i++)
                {
                    colors[i] = System.Windows.Media.Color.FromArgb(
                    (byte)(Element.Series[i].Color.A * 255),
                    (byte)(Element.Series[i].Color.R * 255),
                    (byte)(Element.Series[i].Color.G * 255),
                    (byte)(Element.Series[i].Color.B * 255));
                }
            }

            System.Windows.Media.Color color = System.Windows.Media.Color.FromArgb(
                    (byte)(Element.Color.A * 255),
                    (byte)(Element.Color.R * 255),
                    (byte)(Element.Color.G * 255),
                    (byte)(Element.Color.B * 255));

            ChartSurface surfaceView = new ChartSurface(Element, color, colors);
            SetNativeControl(surfaceView);
        }
    }
}
