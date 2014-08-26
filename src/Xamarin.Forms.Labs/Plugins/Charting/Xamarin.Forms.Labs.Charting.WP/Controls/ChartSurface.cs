using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Xamarin.Forms.Labs.Charting.Controls;
using WPColor = System.Windows.Media.Color;
using WPPoint = System.Windows.Point;
using WPSize = System.Windows.Size;
using WPShapes = System.Windows.Shapes;

namespace Xamarin.Forms.Labs.Charting.WP.Controls
{
    public class ChartSurface : Canvas
    {
        public Chart Chart;
        public SolidColorBrush Brush;
        public WPColor[] Colors;

        public ChartSurface(Chart chart, WPColor color, WPColor[] colors)
            : base()
        {
            Chart = chart;
            Brush = new SolidColorBrush(color);
            Colors = colors;

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

            Redraw();
        }

        public void Redraw()
        {
            Chart.DrawChart();
        }

        void _chart_OnDrawBar(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            WPShapes.Rectangle rectangle = new WPShapes.Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors[e.Data.SeriesNo]);
            rectangle.Width = e.Data.XTo - e.Data.XFrom;
            rectangle.Height = e.Data.YTo - e.Data.YFrom;

            Canvas.SetLeft(rectangle, e.Data.XFrom);
            Canvas.SetTop(rectangle, e.Data.YFrom);

            this.Children.Add(rectangle);
        }

        void _chart_OnDrawCircle(object sender, Chart.DrawEventArgs<Events.SingleDrawingData> e)
        {
            WPShapes.Ellipse ellipse = new WPShapes.Ellipse();
            ellipse.Fill = new SolidColorBrush(Colors[e.Data.SeriesNo]);
            ellipse.Width = e.Data.Size;
            ellipse.Height = e.Data.Size;

            Canvas.SetLeft(ellipse, e.Data.X - (e.Data.Size / 2));
            Canvas.SetTop(ellipse, e.Data.Y - (e.Data.Size / 2));

            this.Children.Add(ellipse);
        }

        void _chart_OnDrawGridLine(object sender, Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            WPShapes.Line line = new WPShapes.Line();
            line.Stroke = Brush;
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
            line.Stroke = new SolidColorBrush(Colors[e.Data.SeriesNo]);
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
            textBlock.Foreground = Brush;
            textBlock.Text = e.Data.Text;

            Canvas.SetLeft(textBlock, e.Data.X);
            Canvas.SetTop(textBlock, e.Data.Y);

            this.Children.Add(textBlock);
        }
        void _chart_OnDrawPie(object sender, Chart.DrawEventArgs<Events.PieDrawingData> e)
        {
            double size = ((e.Data.X > e.Data.Y) ? e.Data.Y * 2 : e.Data.X * 2);
            double halfSize = size / 2;
            WPPoint previousPoint = new WPPoint(halfSize, 0);

            for (int i = 0; i < e.Data.Percentages.Length; i++)
            {
                double value = e.Data.Percentages[i];
                double coordinateX = halfSize * Math.Sin(value);
                double coordinateY = halfSize * Math.Cos(value);
                Path path = new Path();

                PathFigure pathFigure = new PathFigure();
                pathFigure.IsClosed = true;

                pathFigure.StartPoint = new WPPoint(halfSize, halfSize);

                LineSegment lineSegment = new LineSegment();
                lineSegment.Point = previousPoint;
                pathFigure.Segments.Add(lineSegment);

                previousPoint = new WPPoint(coordinateX + halfSize, coordinateY + halfSize);

                ArcSegment arcSegment = new ArcSegment();
                arcSegment.Size = new WPSize(halfSize, halfSize);
                arcSegment.Point = previousPoint;
                arcSegment.RotationAngle = 0;
                arcSegment.IsLargeArc = value > 180 ? true : false;
                arcSegment.SweepDirection = SweepDirection.Clockwise;
                pathFigure.Segments.Add(arcSegment);

                PathGeometry pathGeometry = new PathGeometry();
                pathGeometry.Figures = new PathFigureCollection();

                pathGeometry.Figures.Add(pathFigure);

                path.Data = pathGeometry;
                path.Fill = new SolidColorBrush(Colors[i]);
                this.Children.Add(path);
            }
        }
    }
}
