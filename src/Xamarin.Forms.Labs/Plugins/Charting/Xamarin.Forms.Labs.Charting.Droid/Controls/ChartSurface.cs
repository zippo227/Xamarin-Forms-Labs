using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
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
        private const int PADDING_LEFT = 50;
        private const int PADDING_TOP = 20;
        private int _noOfHorizontalLines = 4;
        private float _height, _width, _spacing;
        private bool _grid;

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
            _grid = chart.Grid;
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

            canvas.DrawLine(PADDING_LEFT, PADDING_TOP, PADDING_LEFT, _height, _paint);
            canvas.DrawLine(PADDING_LEFT, _height, _width, _height, _paint);

            highestValue = DrawGrid(canvas, highestValue, _series[0].Points.Count);

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

        private float DrawGrid(Canvas canvas, float highestValue, int noOfLabels)
        {
            float quarterValue = highestValue / 4;
            double valueOfPart = ((int)Math.Round(quarterValue / 10.0)) * 10;
            if (valueOfPart < quarterValue)
                _noOfHorizontalLines = 5;
            float quarterHeight = (_height - PADDING_TOP) / _noOfHorizontalLines;

            // Horizontal lines and Y-value labels
            canvas.DrawText((valueOfPart * _noOfHorizontalLines).ToString(), 10, PADDING_TOP + 5, _paint);
            for (int i = 1; i <= _noOfHorizontalLines; i++)
            {
                if (_grid)
                {
                    canvas.DrawLine(PADDING_LEFT, PADDING_TOP + (quarterHeight * i), _width, PADDING_TOP + (quarterHeight * i), _paint);
                }
                double currentValue = (valueOfPart * _noOfHorizontalLines) - (valueOfPart * i);
                canvas.DrawText(currentValue.ToString(), 10, PADDING_TOP + (quarterHeight * i) + 5, _paint);
            }

            return (float)valueOfPart * _noOfHorizontalLines;
        }

        private void DrawBarChart(Canvas canvas, float highestValue, float count, int barNo, DataPointCollection points, Paint paint)
        {
            float widthPerBar = ((_width - PADDING_LEFT) - (_spacing * (count - 1))) / count;
            float widthIterator = 2 + (barNo * widthPerBar) + PADDING_LEFT;

            foreach (DataPoint point in points)
            {
                float heightOfBar = ((_height - PADDING_TOP) / highestValue) * point.Value;

                canvas.DrawRect(widthIterator + 1, ((_height - PADDING_TOP) - heightOfBar) + PADDING_TOP, (widthIterator + widthPerBar) - 1, _height, paint);

                widthIterator += widthPerBar * _series.Count(s => s.Type == ChartType.Bar) + _spacing;
            }
        }

        private void DrawLineChart(Canvas canvas, float highestValue, float count, int lineNo, DataPointCollection points, Paint paint)
        {
            float widthPerBar = ((_width - PADDING_LEFT) - (_spacing * (count - 1))) / count;
            float widthOfAllBars = _series.Count(s => s.Type == ChartType.Bar) * widthPerBar;
            float widthIterator = 2 + PADDING_LEFT;

            List<float> pointsList = new List<float>();
            for (int i = 0; i < points.Count; i++)
            {
                float heightOfLine = ((_height - PADDING_TOP) / highestValue) * points[i].Value;

                pointsList.Add(widthIterator + (widthOfAllBars / 2));
                pointsList.Add(((_height - PADDING_TOP) - heightOfLine) + PADDING_TOP);
                if (i != 0 && i != points.Count - 1)
                {
                    pointsList.Add(widthIterator + (widthOfAllBars / 2));
                    pointsList.Add(((_height - PADDING_TOP) - heightOfLine) + PADDING_TOP);
                }
                canvas.DrawCircle(widthIterator + (widthOfAllBars / 2), ((_height - PADDING_TOP) - heightOfLine) + PADDING_TOP, 5, paint);
                canvas.DrawText(points[i].Label, (widthIterator + widthOfAllBars / 2) - (points[i].Label.Length * 4), _height + 25, _paint);

                widthIterator += widthPerBar * _series.Count(s => s.Type == ChartType.Bar) + _spacing;
            }

            canvas.DrawLines(pointsList.ToArray(), paint);
        }
    }
}
