using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Helpers;

namespace Xamarin.Forms.Labs
{
    public interface ISensor
    {
        event EventHandler<EventArgs<Vector3>> ReadingAvailable;

        Vector3 LatestReading { get; }

        AccelerometerInterval Interval { get; set; }
    }
}
