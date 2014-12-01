using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class DynamicUITableViewRenderer<T> : ViewRenderer<DynamicListView<T>, UITableView>
    {
        private float defaultRowHeight = 22;

        private TableViewDelegate tableDelegate;
        private TableDataSource dataSource;
        private UITableView tableView;

        private TableDataSource DataSource
        {
            get
            {
                return dataSource ??
                    (dataSource =
                    new TableDataSource(this.GetCell, this.RowsInSection));
            }
        }

        private TableViewDelegate TableDelegate
        {
            get
            {
                return tableDelegate ??
                    (tableDelegate =
                    new TableViewDelegate(this.RowSelected, this.GetHeightForRow));
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DynamicListView<T>> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                this.tableView = new UITableView();
                this.SetNativeControl(this.tableView);
            }

            this.Unbind(e.OldElement);
            this.Bind(e.NewElement);

            this.tableView.DataSource = this.DataSource;
            this.tableView.Delegate = this.TableDelegate;
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
            this.tableView.ReloadData();
        }

        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Data")
            {
                this.Element.Data.CollectionChanged += DataCollectionChanged;
            }
        }

        #region UITableView weak delegate
        //[Export("tableView:cellForRowAtIndexPath:")]
        /// <summary>
        /// Gets cell for UITableView
        /// </summary>
        /// <param name="tableView">
        /// The table view.
        /// </param>
        /// <param name="indexPath">
        /// The index path.
        /// </param>
        /// <returns>
        /// The <see cref="UITableViewCell"/>.
        /// </returns>
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var item = this.Element.Data[indexPath.Row];
            return GetCell(tableView, item);
        }

        protected virtual UITableViewCell GetCell(UITableView tableView, T item)
        {
            var cell = tableView.DequeueReusableCell(this.GetType().Name) ?? new UITableViewCell(UITableViewCellStyle.Value1, this.GetType().Name);

            cell.TextLabel.Text = item.ToString();
            return cell;
        }

        //[Export("tableView:numberOfRowsInSection:")]
        /// <summary>
        /// The rows in section.
        /// </summary>
        /// <param name="tableView">
        /// The table view.
        /// </param>
        /// <param name="section">
        /// The section.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int RowsInSection(UITableView tableView, int section)
        {
            return this.Element.Data.Count;
        }

        //[Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            this.Element.InvokeItemSelectedEvent(tableView, this.Element.Data[indexPath.Item]);
        }

        //[Export("tableView:heightForRowAtIndexPath:")]
        public virtual float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return GetHeightForRow(tableView, this.Element.Data[indexPath.Item]);
        }

        protected virtual float GetHeightForRow(UITableView tableView, T item)
        {
            return this.defaultRowHeight;
        }
        #endregion

        private class TableDataSource : UITableViewDataSource
        {
            public delegate UITableViewCell OnGetCell(UITableView tableView, NSIndexPath indexPath);
            public delegate int OnRowsInSection(UITableView tableView, int section);

            private readonly OnGetCell onGetCell;
            private readonly OnRowsInSection onRowsInSection;

            public TableDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection)
            {
                this.onGetCell = onGetCell;
                this.onRowsInSection = onRowsInSection;
            }

            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                return onGetCell(tableView, indexPath);
            }

            public override int RowsInSection(UITableView tableView, int section)
            {
                return onRowsInSection(tableView, section);
            }
        }

        private class TableViewDelegate : UITableViewDelegate
        {
            public delegate void OnRowSelected(UITableView tableView, NSIndexPath indexPath);
            public delegate float OnGetHeightForRow(UITableView tableView, NSIndexPath indexPath);

            private readonly OnRowSelected onRowSelected;
            private readonly OnGetHeightForRow onGetHeightForRow;

            public TableViewDelegate(OnRowSelected onRowSelected, OnGetHeightForRow onGetHeightForRow)
            {
                this.onRowSelected = onRowSelected;
                this.onGetHeightForRow = onGetHeightForRow;
            }

            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
            {
                return onGetHeightForRow(tableView, indexPath);
            }

            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                onRowSelected(tableView, indexPath);
            }
        }
    }
}