using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Helpers;

namespace Xamarin.Forms.Labs
{
    public abstract class DeviceSensor : ISensor
    {
        protected event EventHandler<EventArgs<Vector3>> readingAvailable;

        public event EventHandler<EventArgs<Vector3>> ReadingAvailable
        {
            add
            {
                if (readingAvailable == null)
                {
                    Start();
                }
                readingAvailable += value;
            }
            remove
            {
                readingAvailable -= value;
                if (readingAvailable == null)
                {
                    Stop();
                }
            }
        }

        public Vector3 LatestReading
        {
            get;
            protected set;
        }

        protected abstract void Start();

        protected abstract void Stop();

        public abstract AccelerometerInterval Interval { get; set;}
    }
}
