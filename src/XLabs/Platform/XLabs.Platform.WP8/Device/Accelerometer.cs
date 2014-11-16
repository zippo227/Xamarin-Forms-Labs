namespace XLabs.Platform.WP8.Device
{
	using System;

	using Microsoft.Devices.Sensors;

	using XLabs.Platform.WP8.Extensions;

	public partial class Accelerometer
    {
        private Microsoft.Devices.Sensors.Accelerometer accelerometer;

        public AccelerometerInterval Interval { get; set; }

        partial void Start()
        {
            this.accelerometer = new Microsoft.Devices.Sensors.Accelerometer { TimeBetweenUpdates = TimeSpan.FromMilliseconds((long)this.Interval) };

            this.accelerometer.CurrentValueChanged += this.AccelerometerOnCurrentValueChanged;
            this.accelerometer.Start();
        }

        partial void Stop()
        {
            if (this.accelerometer != null)
            {
                this.accelerometer.CurrentValueChanged -= this.AccelerometerOnCurrentValueChanged;
                this.accelerometer.Stop();
                this.accelerometer = null;
            }
        }

        private void AccelerometerOnCurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> sensorReadingEventArgs)
        {
            this.LatestReading = sensorReadingEventArgs.SensorReading.Acceleration.AsVector3();
            readingAvailable.Invoke(sender, this.LatestReading);
        }
    }
}
