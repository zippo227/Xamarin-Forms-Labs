using Android.Content;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms.Labs.Charting.Controls;
using AndroidColor = Android.Graphics.Color;

namespace Xamarin.Forms.Labs.Charting.Droid.Controls
{
    public class ChartSurface : SurfaceView
    {
        public Chart Chart;
        public Paint Paint;
        public AndroidColor[] Colors;
        public Canvas Canvas;

        public ChartSurface(Context context, Chart chart, AndroidColor color, AndroidColor[] colors)
            : base(context)
        {
            SetWillNotDraw(false);

            Chart = chart;
            Paint = new Paint() { Color = color, StrokeWidth = 2 };
            Colors = colors;
        }

        protected override void OnDraw(Canvas canvas)
        {
            Canvas = new Canvas();
            base.OnDraw(Canvas);

            Canvas = canvas;

            Chart.OnDrawBar -= _chart_OnDrawBar;
            Chart.OnDrawBar += _chart_OnDrawBar;
            Chart.OnDrawCircle -= _chart_OnDrawCircle;
            Chart.OnDrawCircle += _chart_OnDrawCircle;
            Chart.OnDrawGridLine -= _chart_OnDrawGridLine;
            Chart.OnDrawGridLine += _chart_OnDrawGridLine;
            Chart.OnDrawLine -= _chart_OnDrawLine;
            Chart.OnDrawLine += _chart_OnDrawLine;
            Chart.OnDrawText -= _chart_OnDrawText;
            Chart.OnDrawText += _chart_OnDrawText;
            Chart.OnDrawPie -= _chart_OnDrawPie;
            Chart.OnDrawPie += _chart_OnDrawPie;

            Chart.DrawChart();
        }
        
        void _chart_OnDrawBar(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            Canvas.DrawRect((float)e.Data.XFrom, (float)e.Data.YFrom, (float)e.Data.XTo, (float)e.Data.YTo, new Paint() { Color = Colors[e.Data.SeriesNo] });
        }

        void _chart_OnDrawCircle(object sender, Chart.DrawEventArgs<Events.SingleDrawingData> e)
        {
            Canvas.DrawCircle((float)e.Data.X, (float)e.Data.Y, (float)e.Data.Size, new Paint() { Color = Colors[e.Data.SeriesNo] });
        }

        void _chart_OnDrawGridLine(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            Canvas.DrawLine((float)e.Data.XFrom, (float)e.Data.YFrom, (float)e.Data.XTo, (float)e.Data.YTo, Paint);
        }

        void _chart_OnDrawLine(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            Canvas.DrawLine((float)e.Data.XFrom, (float)e.Data.YFrom, (float)e.Data.XTo, (float)e.Data.YTo, new Paint() { Color = Colors[e.Data.SeriesNo], StrokeWidth = 2.5F });
        }

        void _chart_OnDrawText(object sender, Chart.DrawEventArgs<Events.TextDrawingData> e)
        {
            Canvas.DrawText(e.Data.Text, (float)e.Data.X, (float)e.Data.Y, Paint);
        }

        void _chart_OnDrawPie(object sender, Chart.DrawEventArgs<Events.PieDrawingData> e)
        {
            float pieDegrees = 360;
            float size = ((e.Data.X > e.Data.Y) ? (float)e.Data.Y * 2 : (float)e.Data.X * 2);
            for(int i = 0; i < e.Data.Percentages.Length; i++)
            {
                float value = (float)e.Data.Percentages[i];
                Canvas.DrawArc(new RectF(0, 0, size, size), 0, pieDegrees, true, new Paint() { Color = Colors[i] });
                pieDegrees -= value;
            }
        }
    }
}
