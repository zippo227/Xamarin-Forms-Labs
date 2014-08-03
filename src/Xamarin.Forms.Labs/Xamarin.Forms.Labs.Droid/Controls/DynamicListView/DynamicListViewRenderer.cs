using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AndroidList = Android.Widget.ListView;
using Xamarin.Forms.Labs.Controls;
using Android.Widget;

namespace Xamarin.Forms.Labs.Droid
{
    public class DynamicListViewRenderer<T> : ViewRenderer<DynamicListView<T>,AndroidList>
    {
        private AndroidList tableView;

        private DataSource source;

        private DataSource Source
        {
            get
            {
                return source ?? (source = new DataSource(this));
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DynamicListView<T>> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                this.tableView = new AndroidList(this.Context);
                this.SetNativeControl(this.tableView);
            }

            this.Unbind(e.OldElement);
            this.Bind(e.NewElement);

            this.tableView.Adapter = this.Source;
        }

        protected virtual Android.Views.View GetView(T item, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            var v = convertView as TextView ?? new TextView(parent.Context);
            v.Text = item.ToString();
            return v;
        }

        private void Unbind(DynamicListView<T> oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging += ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                oldElement.Data.CollectionChanged += DataCollectionChanged;
            }
        }

        private void Bind(DynamicListView<T> newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                newElement.Data.CollectionChanged += DataCollectionChanged;
            }
        }

        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                this.Element.Data.CollectionChanged -= DataCollectionChanged;
            }
        }

        private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.Source.NotifyDataSetChanged();
        }

        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                this.Element.Data.CollectionChanged += DataCollectionChanged;
            }
        }

        private class DataSource : BaseAdapter
        {
            DynamicListViewRenderer<T> parent;

            public DataSource(DynamicListViewRenderer<T> parent)
            {
                this.parent = parent;
            }

            #region implemented abstract members of BaseAdapter
            public override Java.Lang.Object GetItem(int position)
            {
                return position;
            }

            public override long GetItemId(int position)
            {
                return position;
            }

            public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
            {
                return this.parent.GetView(this.parent.Element.Data [position], convertView, parent);
            }

            public override int Count
            {
                get
                {
                    return this.parent.Element.Data.Count;
                }
            }
            #endregion
        }
    }
}

