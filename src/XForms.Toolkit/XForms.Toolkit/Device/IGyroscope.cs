using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XForms.Toolkit.Helpers;

namespace XForms.Toolkit
{
    public interface IGyroscope
    {
        event EventHandler<EventArgs<Vector3>> ReadingAvailable;

        Vector3 LatestReading { get; }

        AccelerometerInterval Interval { get; set; }
    }
}
