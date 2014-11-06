using MonoTouch.UIKit;
using XLabs;

namespace Xamarin.Forms.Labs
{
    /// <summary>
    /// The accelerometer.
    /// </summary>
    public partial class Accelerometer
    {
        private AccelerometerInterval interval = AccelerometerInterval.Ui;

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        public AccelerometerInterval Interval
        {
            get { return this.interval; }
            set
            {
                if (this.interval != value)
                {
                    this.interval = value;
                    UIAccelerometer.SharedAccelerometer.UpdateInterval = ((long)this.interval) / 1000d;
                }
            }
        }

        partial void Start()
        {
            UIAccelerometer.SharedAccelerometer.Acceleration += this.HandleAcceleration;
        }

        partial void Stop()
        {
            UIAccelerometer.SharedAccelerometer.Acceleration -= this.HandleAcceleration;
        }

        private void HandleAcceleration(object sender, UIAccelerometerEventArgs e)
        {
            readingAvailable.Invoke(sender, new Vector3(e.Acceleration.X, e.Acceleration.Y, e.Acceleration.Z));
        }
    }
}