using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Events
{
    public sealed class SingleDrawingData
    {
        public int SeriesNo { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Size { get; set; }

        public SingleDrawingData(float x, float y, int seriesNo)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = 5;
        }
        public SingleDrawingData(float x, float y, int seriesNo, float size)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = size;
        }
    }
}
