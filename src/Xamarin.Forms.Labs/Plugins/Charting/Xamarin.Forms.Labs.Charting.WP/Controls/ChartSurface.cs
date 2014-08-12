using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Xamarin.Forms.Labs.Charting.Controls;
using WPColor = System.Windows.Media.Color;
using WPShapes = System.Windows.Shapes;

namespace Xamarin.Forms.Labs.Charting.WP.Controls
{
    public class ChartSurface : Canvas
    {
        private Chart _chart;
        private SolidColorBrush _brush;
        private WPColor[] _colors;

        public ChartSurface(Chart chart, WPColor color, WPColor[] colors) : base()
        {
            this.Background = new SolidColorBrush(System.Windows.Media.Colors.Blue);
            this.Width = chart.Width;
            this.Height = chart.Height;
            _chart = chart;
            _brush = new SolidColorBrush(color);
            _colors = colors;

            _chart.OnDrawBar += _chart_OnDrawBar;
            _chart.OnDrawCircle += _chart_OnDrawCircle;
            _chart.OnDrawGridLine += _chart_OnDrawGridLine;
            _chart.OnDrawLine += _chart_OnDrawLine;
            _chart.OnDrawText += _chart_OnDrawText;

            _chart.DrawChart();
        }

        void _chart_OnDrawBar(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            WPShapes.Rectangle rectangle = new WPShapes.Rectangle();
            rectangle.Fill = new SolidColorBrush(_colors[e.Data.SeriesNo]);
            rectangle.Width = e.Data.XTo - e.Data.XFrom;
            rectangle.Height = e.Data.YTo - e.Data.YFrom;

            Canvas.SetLeft(rectangle, e.Data.XFrom);
            Canvas.SetTop(rectangle, e.Data.YFrom);

            this.Children.Add(rectangle);
        }

        void _chart_OnDrawCircle(object sender, Chart.DrawEventArgs<Events.SingleDrawingData> e)
        {
            WPShapes.Ellipse ellipse = new WPShapes.Ellipse();
            ellipse.Fill = new SolidColorBrush(_colors[e.Data.SeriesNo]);
            ellipse.Width = 5;
            ellipse.Height = 5;

            Canvas.SetLeft(ellipse, e.Data.X);
            Canvas.SetTop(ellipse, e.Data.Y);

            this.Children.Add(ellipse);
        }

        void _chart_OnDrawGridLine(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            WPShapes.Line line = new WPShapes.Line();
            line.Stroke = _brush;
            line.StrokeThickness = 2;

            line.X1 = e.Data.XFrom;
            line.Y1 = e.Data.YFrom;
            line.X2 = e.Data.XTo;
            line.Y2 = e.Data.YTo;

            this.Children.Add(line);
        }

        void _chart_OnDrawLine(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            WPShapes.Line line = new WPShapes.Line();
            line.Stroke = new SolidColorBrush(_colors[e.Data.SeriesNo]);
            line.StrokeThickness = 2;

            line.X1 = e.Data.XFrom;
            line.Y1 = e.Data.YFrom;
            line.X2 = e.Data.XTo;
            line.Y2 = e.Data.YTo;

            this.Children.Add(line);
        }

        void _chart_OnDrawText(object sender, Chart.DrawEventArgs<Events.TextDrawingData> e)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Foreground = _brush;

            Canvas.SetLeft(textBlock, e.Data.X);
            Canvas.SetTop(textBlock, e.Data.Y);

            this.Children.Add(textBlock);
        }
    }
}
