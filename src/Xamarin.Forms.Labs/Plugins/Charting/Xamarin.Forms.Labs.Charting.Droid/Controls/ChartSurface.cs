using Android.Content;
using Android.Graphics;
using Android.Views;
using Xamarin.Forms.Labs.Charting.Controls;
using AndroidColor = Android.Graphics.Color;

namespace Xamarin.Forms.Labs.Charting.Droid.Controls
{
    public class ChartSurface : SurfaceView
    {
        private Chart _chart;
        private Paint _paint;
        private AndroidColor[] _colors;
        private Canvas _canvas;

        public ChartSurface(Context context, Chart chart, AndroidColor color, AndroidColor[] colors)
            : base(context)
        {
            SetWillNotDraw(false);

            _chart = chart;
            _paint = new Paint() { Color = color, StrokeWidth = 2 };
            _colors = colors;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            _canvas = canvas;

            _chart.OnDrawBar += _chart_OnDrawBar;
            _chart.OnDrawCircle += _chart_OnDrawCircle;
            _chart.OnDrawGridLine +=_chart_OnDrawGridLine;
            _chart.OnDrawLine += _chart_OnDrawLine;
            _chart.OnDrawText += _chart_OnDrawText;

            _chart.DrawChart();
        }

        void _chart_OnDrawBar(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            _canvas.DrawRect(e.Data.XFrom, e.Data.YFrom, e.Data.XTo, e.Data.YTo, new Paint() { Color = _colors[e.Data.SeriesNo] });
        }

        void _chart_OnDrawCircle(object sender, Chart.DrawEventArgs<Events.SingleDrawingData> e)
        {
            _canvas.DrawCircle(e.Data.X, e.Data.Y, 5, new Paint() { Color = _colors[e.Data.SeriesNo] });
        }

        void _chart_OnDrawGridLine(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            _canvas.DrawLine(e.Data.XFrom, e.Data.YFrom, e.Data.XTo, e.Data.YTo, _paint);
        }

        void _chart_OnDrawLine(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            _canvas.DrawLine(e.Data.XFrom, e.Data.YFrom, e.Data.XTo, e.Data.YTo, new Paint() { Color = _colors[e.Data.SeriesNo], StrokeWidth = 2.5F });
        }

        void _chart_OnDrawText(object sender, Chart.DrawEventArgs<Events.TextDrawingData> e)
        {
            _canvas.DrawText(e.Data.Text, e.Data.X, e.Data.Y, _paint);
        }
    }
}
