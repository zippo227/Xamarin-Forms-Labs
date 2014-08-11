using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Labs.Charting.Events;

[assembly: InternalsVisibleTo("Xamarin.Forms.Labs.Charting.Droid")]
namespace Xamarin.Forms.Labs.Charting.Controls
{
    public class Chart : View
    {
        private const int PADDING_LEFT = 50;
        private const int PADDING_TOP = 20;

        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Chart), Color.White, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty SeriesProperty = BindableProperty.Create("Series", typeof(SeriesCollection), typeof(Chart), default(SeriesCollection), BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty WidthProperty = BindableProperty.Create("Width", typeof(float), typeof(Chart), 250F, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty HeightProperty = BindableProperty.Create("Height", typeof(float), typeof(Chart), 250F, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(float), typeof(Chart), 5F, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty GridProperty = BindableProperty.Create("Grid", typeof(bool), typeof(Chart), true, BindingMode.OneWay, null, null, null, null);

        public Color Color
        {
            get
            {
                return (Color)base.GetValue(Chart.ColorProperty);
            }
            set
            {
                base.SetValue(Chart.ColorProperty, value);
            }
        }

        public SeriesCollection Series
        {
            get
            {
                return (SeriesCollection)base.GetValue(Chart.SeriesProperty);
            }
            set
            {
                base.SetValue(Chart.SeriesProperty, value);
            }
        }

        public float Width
        {
            get
            {
                return (float)base.GetValue(Chart.WidthProperty);
            }
            set
            {
                base.SetValue(Chart.WidthProperty, value);
            }
        }

        public float Height
        {
            get
            {
                return (float)base.GetValue(Chart.HeightProperty);
            }
            set
            {
                base.SetValue(Chart.HeightProperty, value);
            }
        }

        public float Spacing
        {
            get
            {
                return (float)base.GetValue(Chart.SpacingProperty);
            }
            set
            {
                base.SetValue(Chart.SpacingProperty, value);
            }
        }

        public bool Grid
        {
            get
            {
                return (bool)base.GetValue(Chart.GridProperty);
            }
            set
            {
                base.SetValue(Chart.GridProperty, value);
            }
        }

        public Chart()
        {
            Series = new SeriesCollection();
        }

        internal class DrawEventArgs<T> : EventArgs
        {
            /// <summary>
            /// The event coming from the server.
            /// </summary>
            public T Data { get; set; }
        }

        internal event EventHandler<DrawEventArgs<TextDrawingData>> OnDrawText;
        internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawGridLine;
        internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawBar;
        internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawLine;
        internal event EventHandler<DrawEventArgs<SingleDrawingData>> OnDrawCircle;

        private int getNoOfBars()
        {
            int noOfBars = 0;
            foreach (Series series in Series)
            {
                if (series.Type == ChartType.Bar)
                    noOfBars += series.Points.Count();
            }
            return noOfBars;
        }

        private float getHighestValue()
        {
            float highestValue = 0;
            foreach (Series series in Series)
            {
                float tempHighestValue = series.Points.Max(p => p.Value);
                if (highestValue < tempHighestValue)
                {
                    highestValue = tempHighestValue;
                }
            }
            return highestValue;
        }

        internal void DrawChart()
        {
            int noOfBars = getNoOfBars();
            float highestValue = getHighestValue();

            OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, PADDING_TOP, PADDING_LEFT, Height, 0) });
            OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, Height, Width, Height, 0) });

            highestValue = DrawGrid(highestValue, Series[0].Points.Count);
            float widthPerBar = ((Width - PADDING_LEFT) - (Spacing * (noOfBars - 1))) / noOfBars;

            DrawLabels(highestValue, widthPerBar, Series[0].Points);
            
            Height -= 2; // Y-axis space

            for (int i = 0; i < Series.Count; i++)
            {
                Series series = Series[i];

                switch (series.Type)
                {
                    case ChartType.Bar:
                        DrawBarChart(highestValue, widthPerBar, i, series.Points);
                        break;
                    case ChartType.Line:
                        DrawLineChart(highestValue, widthPerBar, i, series.Points);
                        break;
                }
            }
        }

        private float DrawGrid(float highestValue, float noOfLabels)
        {
            int noOfHorizontalLines = 4;
            float quarterValue = highestValue / 4;
            double valueOfPart = ((int)Math.Round(quarterValue / 10.0)) * 10;
            if (valueOfPart < quarterValue)
                noOfHorizontalLines = 5;
            float quarterHeight = (Height - PADDING_TOP) / noOfHorizontalLines;

            // Horizontal lines and Y-value labels
            OnDrawText(this, new DrawEventArgs<TextDrawingData>() { Data = new TextDrawingData((valueOfPart * noOfHorizontalLines).ToString(), 10, PADDING_TOP + 5) });
            for (int i = 1; i <= noOfHorizontalLines; i++)
            {
                if (Grid)
                {
                    OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, PADDING_TOP + (quarterHeight * i), Width, PADDING_TOP + (quarterHeight * i), 0) });
                }
                double currentValue = (valueOfPart * noOfHorizontalLines) - (valueOfPart * i);
                OnDrawText(this, new DrawEventArgs<TextDrawingData>() { Data = new TextDrawingData(currentValue.ToString(), 10, PADDING_TOP + (quarterHeight * i) + 5) });
            }

            return (float)valueOfPart * noOfHorizontalLines;
        }

        private void DrawBarChart(float highestValue, float widthPerBar, int barNo, DataPointCollection points)
        {
            float widthIterator = 2 + (barNo * widthPerBar) + PADDING_LEFT;

            foreach (DataPoint point in points)
            {
                float heightOfBar = ((Height - PADDING_TOP) / highestValue) * point.Value;

                OnDrawBar(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(widthIterator + 1, ((Height - PADDING_TOP) - heightOfBar) + PADDING_TOP, (widthIterator + widthPerBar) - 1, Height, barNo) });

                widthIterator += widthPerBar * Series.Count(s => s.Type == ChartType.Bar) + Spacing;
            }
        }

        private void DrawLineChart(float highestValue, float widthPerBar, int lineNo, DataPointCollection points)
        {
            float widthOfAllBars = Series.Count(s => s.Type == ChartType.Bar) * widthPerBar;
            float widthIterator = 2 + PADDING_LEFT;

            List<float> pointsList = new List<float>();
            float[] previousPoints = new float[2];
            for (int i = 0; i < points.Count; i++)
            {
                float heightOfLine = ((Height - PADDING_TOP) / highestValue) * points[i].Value;

                float x = widthIterator + (widthOfAllBars / 2);
                float y = ((Height - PADDING_TOP) - heightOfLine) + PADDING_TOP;

                if (i != 0)
                {
                    OnDrawLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(previousPoints[0], previousPoints[1], x, y, lineNo) });
                }

                previousPoints[0] = x;
                previousPoints[1] = y;

                OnDrawCircle(this, new DrawEventArgs<SingleDrawingData>() { Data = new SingleDrawingData(widthIterator + (widthOfAllBars / 2), ((Height - PADDING_TOP) - heightOfLine) + PADDING_TOP, lineNo) });

                widthIterator += widthPerBar * Series.Count(s => s.Type == ChartType.Bar) + Spacing;
            }
        }

        private void DrawLabels(float highestValue, float widthPerBar, DataPointCollection points)
        {
            float widthOfAllBars = Series.Count(s => s.Type == ChartType.Bar) * widthPerBar;
            float widthIterator = 2 + PADDING_LEFT;

            for (int i = 0; i < points.Count; i++)
            {
                OnDrawText(this, new DrawEventArgs<TextDrawingData>() { Data = new TextDrawingData(points[i].Label, (widthIterator + widthOfAllBars / 2) - (points[i].Label.Length * 4), Height + 25) });
                widthIterator += widthPerBar * Series.Count(s => s.Type == ChartType.Bar) + Spacing;
            }
        }
    }
}
