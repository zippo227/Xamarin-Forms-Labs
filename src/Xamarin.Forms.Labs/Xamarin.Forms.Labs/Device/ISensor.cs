using System;

namespace Xamarin.Forms.Labs
{
    using XLabs;

    public interface ISensor
    {
        event EventHandler<EventArgs<Vector3>> ReadingAvailable;

        Vector3 LatestReading { get; }

        AccelerometerInterval Interval { get; set; }
    }
}
