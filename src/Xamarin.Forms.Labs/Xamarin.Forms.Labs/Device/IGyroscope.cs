using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Helpers;

namespace Xamarin.Forms.Labs
{
    public interface IGyroscope
    {
        event EventHandler<EventArgs<Vector3>> ReadingAvailable;

        /// <summary>
        /// Gets the latest reading vector
        /// </summary>
        /// <value>Rotation values in radians per second</value>
        Vector3 LatestReading { get; }

        AccelerometerInterval Interval { get; set; }
    }
}
