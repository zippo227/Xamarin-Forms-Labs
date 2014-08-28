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
        public double X { get; set; }
        public double Y { get; set; }
        public double Size { get; set; }
        public double[] Percentages { get; set; }

        public PieDrawingData(double x, double y, int seriesNo, double size, double[] percentages)
        {
            X = x;
            Y = y;
            SeriesNo = seriesNo;
            Size = size;
            Percentages = percentages;
        }
    }
}
