using Android.Content;
using Android.Graphics;
using Android.Views;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xamarin.Forms.Labs.Charting.Controls;
using AndroidColor = Android.Graphics.Color;

namespace Xamarin.Forms.Labs.Charting.Droid.Controls
{
    public class ChartSurface : SurfaceView
    {
        private SeriesCollection _series;
        private Paint _paint;
        private AndroidColor[] _colors;
        private float _height, _width, _spacing;

        public ChartSurface(Context context, Chart chart, AndroidColor color, AndroidColor[] colors)
            : base(context)
        {
            SetWillNotDraw(false);

            _colors = colors;
            _paint = new Paint() { Color = color, StrokeWidth = 2 };

            _series = chart.Series;
            _height = chart.Height;
            _width = chart.Width;
            _spacing = chart.Spacing;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            int count = 0;
            float highestValue = 0;

            // Set count and highestValue
            foreach (Series series in _series)
            {
                if (series.Type == ChartType.Bar)
                    count += series.Points.Count();
                float tempHighestValue = series.Points.Max(p => p.Value);
                if (highestValue < tempHighestValue)
                {
                    highestValue = tempHighestValue;
                }
            }

            canvas.DrawLine(0, 0, 0, _height, _paint);
            canvas.DrawLine(0, _height, _width, _height, _paint);
            _height -= 2;

            for (int i = 0; i < _series.Count; i++)
            {
                Series series = _series[i];

                Paint seriesPaint = new Paint() { Color = _colors[i], StrokeWidth = 2 };

                switch (series.Type)
                {
                    case ChartType.Bar:
                        DrawBarChart(canvas, highestValue, count, i, series.Points, seriesPaint);
                        break;
                    case ChartType.Line:
                        DrawLineChart(canvas, highestValue, count, i, series.Points, seriesPaint);
                        break;
                }
            }
        }

        private void DrawBarChart(Canvas canvas, float highestValue, float count, int barNo, DataPointCollection points, Paint paint)
        {
            float widthPerBar = (_width - (_spacing * (count - 1))) / count;
            float widthIterator = 2 + (barNo * widthPerBar);

            foreach (DataPoint point in points)
            {
                float heightOfBar = (_height / highestValue) * point.Value;

                canvas.DrawRect(widthIterator, _height - heightOfBar, widthIterator + widthPerBar, _height, paint);

                widthIterator += widthPerBar * _series.Count(s => s.Type == ChartType.Bar) + _spacing;
            }
        }

        private void DrawLineChart(Canvas canvas, float highestValue, float count, int lineNo, DataPointCollection points, Paint paint)
        {
            float widthPerBar = (_width - (_spacing * (count - 1))) / count;
            float widthOfAllBars = _series.Count(s => s.Type == ChartType.Bar) * widthPerBar;
            float widthIterator = 2;

            List<float> pointsList = new List<float>();
            for (int i = 0; i < points.Count; i++)
            {
                float heightOfLine = (_height / highestValue) * points[i].Value;

                pointsList.Add(widthIterator + (widthOfAllBars / 2));
                pointsList.Add(_height - heightOfLine);
                if (i != 0 && i != points.Count - 1)
                {
                    pointsList.Add(widthIterator + (widthOfAllBars / 2));
                    pointsList.Add(_height - heightOfLine);
                }
                canvas.DrawCircle(widthIterator + (widthOfAllBars / 2), _height - heightOfLine, 5, paint);
                canvas.DrawText(points[i].Label, (widthIterator + widthOfAllBars / 2) - (points[i].Label.Length * 4), _height + 25, _paint);

                widthIterator += widthPerBar * _series.Count(s => s.Type == ChartType.Bar) + _spacing;
            }

            canvas.DrawLines(pointsList.ToArray(), paint);
        }
    }
}
