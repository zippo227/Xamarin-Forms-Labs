using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
    public class DynamicListView<T> : View
    {
        //private Predicate<T> filter;

        /// <summary>
        /// The data.
        /// </summary>
        private ObservableCollection<T> data;

        public DynamicListView ()
        {
            this.Data = new ObservableCollection<T> ();
        }

        //public Predicate<T> Filter
        //{
        //    get { return this.filter; }
        //    set
        //    {
        //        this.filter = value;
        //        this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        //    }
        //}

        /// <summary>
        /// Occurs when item is selected.
        /// </summary>
        public event EventHandler<EventArgs<T>> OnSelected;

        /// <summary>
        /// The requested event occurs when an observer requests an item.
        /// </summary>
        /// <remarks>The sender will be the requesting observer, f.e. a ListView in Android
        /// or UITableView in iOS.</remarks>
        //public event EventHandler<EventArgs<int>> OnRequested;

        /// <summary>
        /// Add items to data collection.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add (T item)
        {
            this.Data.Add (item);
        }

        /// <summary>
        /// Replaces an object in collection.
        /// </summary>
        /// <param name="original">
        /// The original object.
        /// </param>
        /// <param name="replacement">
        /// The replacement object.
        /// </param>
        /// <returns>
        /// <see cref="bool"/>, true if replacement was successful, false if original object was not found.
        /// </returns>
        public bool Replace (T original, T replacement)
        {
            var index = this.Data.IndexOf (original);

            if (index < 0) {
                return false;
            }

            this.Data [index] = replacement;

            return true;
        }

        public void Remove (T item)
        {
            this.Data.Remove (item);
        }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public ObservableCollection<T> Data {
            get {
                return this.data;
            }

            set {
                this.OnPropertyChanging ();
                this.data = value;
                this.OnPropertyChanged ();
            }
        }

        /// <summary>
        /// Invokes the item selected event.
        /// </summary>
        /// <param name="item">Item.</param>
        public void InvokeItemSelectedEvent (object sender, T item)
        {
            if (this.OnSelected != null) {
                this.OnSelected.Invoke (sender, new EventArgs<T> (item));
            }
        }

        /// <summary>
        /// Invokes the item requested event.
        /// </summary>
        /// <param name="index">Index of the requested item.</param>
        //public void InvokeItemRequestedEvent(object sender, int index)
        //{
        //    if (this.OnRequested != null)
        //    {
        //        this.OnRequested(sender, new EventArgs<int>(index));
        //    }
        //}
    }
}
