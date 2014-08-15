using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Events
{
    public sealed class PieDrawingData
    {
        public int SeriesNo { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Size { get; set; }
        public float[] Percentages { get; set; }
        
        public PieDrawingData(float x, float y, int seriesNo, float size, float[] percentages)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = size;
            Percentages = percentages;
        }
    }
}
