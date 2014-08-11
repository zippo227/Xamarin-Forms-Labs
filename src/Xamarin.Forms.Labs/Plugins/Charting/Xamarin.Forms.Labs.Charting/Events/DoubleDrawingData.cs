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
        public float XFrom { get; set; }
        public float YFrom { get; set; }
        public float XTo { get; set; }
        public float YTo { get; set; }

        public DoubleDrawingData(float xFrom, float yFrom, float xTo, float yTo, int seriesNo)
        {
            XFrom = xFrom;
            YFrom = yFrom;
            XTo = xTo;
            YTo = yTo;
            SeriesNo = seriesNo;
        }
    }
}
