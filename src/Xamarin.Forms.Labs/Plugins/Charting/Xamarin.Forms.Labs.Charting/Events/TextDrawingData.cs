using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Events
{
    public class TextDrawingData
    {
        public float X { get; set; }
        public float Y { get; set; }
        public string Text { get; set; }

        public TextDrawingData(string text, float x, float y)
        {
            Text = text;
            X = x;
            Y = y;
        }
    }
}
