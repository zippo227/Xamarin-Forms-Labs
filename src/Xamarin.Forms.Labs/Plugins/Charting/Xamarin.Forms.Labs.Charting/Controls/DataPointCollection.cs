using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Controls
{
    public class DataPointCollection : Collection<DataPoint>
    {
        public void Add(DataPoint dataPoint)
        {
            base.Add(dataPoint);
        }

        public void Remove(DataPoint dataPoint)
        {
            base.Remove(dataPoint);
        }

        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("You tried to remove a datapoint at an index which is invalid!");
            }
            else
            {
                base.RemoveAt(index);
            }
        }

        public DataPoint this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
