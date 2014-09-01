using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms.Labs.Charting.Controls;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace Xamarin.Forms.Labs.Charting.iOS.Controls
{
    public class ChartSurface : UIView
    {
        public Chart Chart;
        public UIColor color;
        public UIColor[] Colors;

        public ChartSurface(Chart chart, UIColor color, UIColor[] colors)
        {
            Chart = chart;
            this.color = color;
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
        }

        public override void Draw(RectangleF rect)
        {
            base.Draw(rect);

            Chart.DrawChart();
        }

        private void _chart_OnDrawBar(object sender, Charting.Controls.Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            using (CGContext g = UIGraphics.GetCurrentContext())
            {

                //set up drawing attributes
                g.SetLineWidth(1);
                Colors[e.Data.SeriesNo].SetFill();
                Colors[e.Data.SeriesNo].SetStroke();

                var path = new CGPath();
                RectangleF rect = new RectangleF((float)e.Data.XFrom, (float)e.Data.YFrom, (float)(e.Data.XTo - e.Data.XFrom), (float)(e.Data.YTo - e.Data.YFrom));
                //DrawRect(rect, new UIViewPrintFormatter());
                
                //add geometry to graphics context and draw it
                g.AddRect(rect);
                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

        private void _chart_OnDrawCircle(object sender, Charting.Controls.Chart.DrawEventArgs<Events.SingleDrawingData> e)
        {
            //throw new NotImplementedException();
        }

        private void _chart_OnDrawGridLine(object sender, Charting.Controls.Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                //set up drawing attributes
                g.SetLineWidth(1);
                color.SetFill();
                color.SetStroke();

                var path = new CGPath();

                g.MoveTo((float)e.Data.XFrom, (float)e.Data.YFrom);
                g.AddLineToPoint((float)e.Data.XTo, (float)e.Data.YTo);

                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

        private void _chart_OnDrawLine(object sender, Charting.Controls.Chart.DrawEventArgs<Events.DoubleDrawingData> e)
        {
            using (CGContext g = UIGraphics.GetCurrentContext())
            {
                //set up drawing attributes
                g.SetLineWidth(1);
                Colors[e.Data.SeriesNo].SetFill();
                Colors[e.Data.SeriesNo].SetStroke();

                var path = new CGPath();

                g.MoveTo((float)e.Data.XFrom, (float)e.Data.YFrom);
                g.AddLineToPoint((float)e.Data.XTo, (float)e.Data.YTo);

                g.DrawPath(CGPathDrawingMode.FillStroke);
            }
        }

        private void _chart_OnDrawText(object sender, Charting.Controls.Chart.DrawEventArgs<Events.TextDrawingData> e)
        {
            //throw new NotImplementedException();
        }

        private void _chart_OnDrawPie(object sender, Charting.Controls.Chart.DrawEventArgs<Events.PieDrawingData> e)
        {
            //throw new NotImplementedException();
        }
    }
}