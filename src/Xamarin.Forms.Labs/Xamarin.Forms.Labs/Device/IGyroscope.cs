using System;
using XLabs;

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
