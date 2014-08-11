using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Charting.Controls;
using Xamarin.Forms.Labs.Charting.WP.Controls;
using Xamarin.Forms.Platform.WinPhone;

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

            System.Windows.Media.Color[] colors = new System.Windows.Media.Color[Element.Series.Count];

            for (int i = 0; i < Element.Series.Count; i++)
            {
                colors[i] = System.Windows.Media.Color.FromArgb(
                    (byte)(Element.Series[i].Color.A * 255),
                    (byte)(Element.Series[i].Color.R * 255),
                    (byte)(Element.Series[i].Color.G * 255),
                    (byte)(Element.Series[i].Color.B * 255));
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
