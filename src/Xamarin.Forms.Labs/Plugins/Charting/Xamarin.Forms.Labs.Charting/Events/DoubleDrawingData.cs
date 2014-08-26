using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Events
{
    public sealed class DoubleDrawingData
    {
        public int SeriesNo { get; set; }
        public double XFrom { get; set; }
        public double YFrom { get; set; }
        public double XTo { get; set; }
        public double YTo { get; set; }

        public DoubleDrawingData(double xFrom, double yFrom, double xTo, double yTo, int seriesNo)
        {
            XFrom = xFrom;
            YFrom = yFrom;
            XTo = xTo;
            YTo = yTo;
            SeriesNo = seriesNo;
        }
    }
}
