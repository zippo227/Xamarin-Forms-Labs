using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Helpers;

namespace Xamarin.Forms.Labs
{
    public partial class Accelerometer : IAccelerometer
    {
        /// <summary>
        /// Gravitational force is 9.81 m/s^2
        /// </summary>
        public const double Gravitation = 9.81;

        /// <summary>
        /// Initializes a new instance of the <see cref="Accelerometer"/> class.
        /// </summary>
        public Accelerometer()
        {
            this.Interval = AccelerometerInterval.Ui;
        }

        /// <summary>
        /// The reading available event handler.
        /// </summary>
        public event EventHandler<EventArgs<Vector3>> ReadingAvailable
        {
            add
            {
                if (this.readingAvailable == null)
                {
                    this.Start();
                }

                this.readingAvailable += value;
            }

            remove
            {
                this.readingAvailable -= value;
                if (this.readingAvailable == null)
                {
                    this.Stop();
                }
            }
        }

        private event EventHandler<EventArgs<Vector3>> readingAvailable;

        /// <summary>
        /// Gets the latest reading.
        /// </summary>
        public Vector3 LatestReading
        {
            get;
            private set;
        }

        partial void Start();

        partial void Stop();
    }
}
