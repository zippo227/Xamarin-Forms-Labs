using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Controls
{
    public class SeriesCollection : Collection<Series>
    {
        public void Add(Series series)
        {
            base.Add(series);
        }

        public void Remove(Series series)
        {
            base.Remove(series);
        }

        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
            {
                throw new IndexOutOfRangeException("You tried to remove a series at an index which is invalid!");
            }
            else
            {
                base.RemoveAt(index);
            }
        }

        public Series this[int index]
        {
            get
            {
                // get the item for that index.
                return base[index];
            }
            set
            {
                // set the item for this index. value will be of type Thing.
                base[index] = value;
            }
        }
    }
}
