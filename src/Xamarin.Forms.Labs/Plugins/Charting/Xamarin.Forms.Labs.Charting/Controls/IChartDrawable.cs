using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Controls
{
    public interface IChartDrawable
    {
        void DrawTextYAxis(string text, float x, float y);
        void DrawGridLines(float xFrom, float yFrom, float xTo, float yTo);
        void DrawBars(float xFrom, float yFrom, float xTo, float yTo);
        void DrawLines(float xFrom, float yFrom, float xTo, float yTo);
    }
}
