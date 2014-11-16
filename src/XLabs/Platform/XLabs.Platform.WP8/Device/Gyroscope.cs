namespace XLabs.Platform.Device
{
	using System;

	using Microsoft.Devices.Sensors;

	using XLabs.Platform.Extensions;

	/// <summary>
	/// Class Gyroscope.
	/// </summary>
	public partial class Gyroscope
	{
		/// <summary>
		/// The _gyroscope
		/// </summary>
		private Microsoft.Devices.Sensors.Gyroscope _gyroscope;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>The interval.</value>
		public AccelerometerInterval Interval { get; set; }

		/// <summary>
		/// Starts this instance.
		/// </summary>
		partial void Start()
		{
			_gyroscope = new Microsoft.Devices.Sensors.Gyroscope
				             {
					             TimeBetweenUpdates = TimeSpan.FromMilliseconds((long)Interval)
				             };

			_gyroscope.CurrentValueChanged += GyroscopeCurrentValueChanged;
			_gyroscope.Start();
		}

		/// <summary>
		/// Stops this instance.
		/// </summary>
		partial void Stop()
		{
			_gyroscope.CurrentValueChanged -= GyroscopeCurrentValueChanged;
			_gyroscope.Stop();
			_gyroscope = null;
		}

		/// <summary>
		/// Gyroscopes the current value changed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The e.</param>
		private void GyroscopeCurrentValueChanged(object sender, SensorReadingEventArgs<GyroscopeReading> e)
		{
			if (_gyroscope.IsDataValid)
			{
				this.LatestReading = e.SensorReading.RotationRate.AsVector3();
				this.readingAvailable.Invoke(this, this.LatestReading);
			}
		}
	}
}