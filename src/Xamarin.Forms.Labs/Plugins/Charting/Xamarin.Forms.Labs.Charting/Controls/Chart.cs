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
        #region Constants
        private const int PADDING_LEFT = 50;
        private const int PADDING_TOP = 20;
        #endregion

        #region BindableProperties
        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Chart), Color.White, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty SeriesProperty = BindableProperty.Create("Series", typeof(SeriesCollection), typeof(Chart), default(SeriesCollection), BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty WidthProperty = BindableProperty.Create("Width", typeof(float), typeof(Chart), 250F, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty HeightProperty = BindableProperty.Create("Height", typeof(float), typeof(Chart), 250F, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(float), typeof(Chart), 5F, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty GridProperty = BindableProperty.Create("Grid", typeof(bool), typeof(Chart), true, BindingMode.OneWay, null, null, null, null);
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the color of the grid and border of the chart element.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the series which should be displayed inside the chart element.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the desired width of the chart element.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the desired height of the chart element.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the desired spacing between series inside chart element.
        /// </summary>
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

        /// <summary>
        /// Gets or sets a value indicating whether this element contains horizontal grid lines.
        /// </summary>
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
        #endregion

        public Chart()
        {
            Series = new SeriesCollection();
        }

        /// <summary>
        /// Event args for the Chart events.
        /// </summary>
        /// <typeparam name="T">A DrawingData class, which is put into the Data property.</typeparam>
        internal class DrawEventArgs<T> : EventArgs
        {
            /// <summary>
            /// The event coming from the Chart
            /// </summary>
            public T Data { get; set; }
        }

        #region Events
        /// <summary>
        /// Fires when canvas needs to draw text
        /// </summary>
        internal event EventHandler<DrawEventArgs<TextDrawingData>> OnDrawText;

        /// <summary>
        /// Fires when canvas needs to draw a grid line
        /// </summary>
        internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawGridLine;

        /// <summary>
        /// Fires when canvas needs to draw a bar
        /// </summary>
        internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawBar;

        /// <summary>
        /// Fires when canvas needs to draw a line
        /// </summary>
        internal event EventHandler<DrawEventArgs<DoubleDrawingData>> OnDrawLine;

        /// <summary>
        /// Fires when canvas needs to draw a circle
        /// </summary>
        internal event EventHandler<DrawEventArgs<SingleDrawingData>> OnDrawCircle;
        #endregion

        /// <summary>
        /// Draw the chart using the specified handlers.
        /// </summary>
        /// <remarks>
        /// Set the events before calling DrawChart.
        /// </remarks>
        internal void DrawChart()
        {
            int noOfBars = 0;
            float highestValue = 0;
            foreach (Series series in Series)
            {
                if (series.Type == ChartType.Bar)
                    noOfBars += series.Points.Count();

                float tempHighestValue = series.Points.Max(p => p.Value);
                if (highestValue < tempHighestValue)
                {
                    highestValue = tempHighestValue;
                }
            }

            OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, PADDING_TOP, PADDING_LEFT, Height, 0) });  //Y-axis
            OnDrawGridLine(this, new DrawEventArgs<DoubleDrawingData>() { Data = new DoubleDrawingData(PADDING_LEFT, Height, Width, Height, 0) });              //X-axis

            highestValue = DrawGrid(highestValue);
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

        #region Private Drawing Functions
        /// <summary>
        /// Draw the horizontal grid lines and Y-axis labels.
        /// </summary>
        /// <param name="highestValue">Highest Y-value within the series.</param>
        /// <returns>New highest value.</returns>
        private float DrawGrid(float highestValue)
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

        /// <summary>
        /// Draw a bar.
        /// </summary>
        /// <param name="highestValue">Highest Y-value possible after drawing the grid.</param>
        /// <param name="widthPerBar">Width of a single bar.</param>
        /// <param name="barNo">The number of the series</param>
        /// <param name="points">Specified points in the series.</param>
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

        /// <summary>
        /// Draw a line.
        /// </summary>
        /// <param name="highestValue">Highest Y-value possible after drawing the grid.</param>
        /// <param name="widthPerBar">Width of a single bar.</param>
        /// <param name="lineNo">The number of the series</param>
        /// <param name="points">Specified points in the series.</param>
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

        /// <summary>
        /// Draw text.
        /// </summary>
        /// <param name="highestValue">Highest Y-value possible after drawing the grid.</param>
        /// <param name="widthPerBar">Width of a single bar.</param>
        /// <param name="points">Specified points in the series.</param>
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
        #endregion
    }
}
