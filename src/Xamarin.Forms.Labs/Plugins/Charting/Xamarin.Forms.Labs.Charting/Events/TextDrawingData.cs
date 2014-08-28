using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Events
{
    public class TextDrawingData
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Text { get; set; }

        public TextDrawingData(string text, double x, double y)
        {
            Text = text;
            X = x;
            Y = y;
        }
    }
}
