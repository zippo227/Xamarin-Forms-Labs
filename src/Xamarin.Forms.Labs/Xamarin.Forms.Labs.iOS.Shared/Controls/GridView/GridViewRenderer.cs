using System;
using System.Linq;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
#if __UNIFIED__
using UIKit;
using Foundation;
#elif __IOS__
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif
using System.Collections;
using System.Collections.Specialized;


[assembly: ExportRenderer (typeof(GridView), typeof(GridViewRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class GridViewRenderer: ViewRenderer<GridView,GridCollectionView>
    {
        public GridViewRenderer ()
        {
        }

        protected override void OnElementChanged (ElementChangedEventArgs<GridView> e)
        {
            base.OnElementChanged (e);

            var collectionView = new GridCollectionView ();
            collectionView.AllowsMultipleSelection = false;
            collectionView.SelectionEnable = e.NewElement.SelectionEnabled;
            //set padding
            collectionView.ContentInset = new UIEdgeInsets ((float)Element.Padding.Top, (float)Element.Padding.Left, (float)Element.Padding.Bottom, (float)Element.Padding.Right);

            collectionView.BackgroundColor = Element.BackgroundColor.ToUIColor ();
            collectionView.ItemSize = new System.Drawing.SizeF ((float)Element.ItemWidth, (float)Element.ItemHeight);
            collectionView.RowSpacing = Element.RowSpacing;
            collectionView.ColumnSpacing = Element.ColumnSpacing;

            this.Unbind (e.OldElement);
            this.Bind (e.NewElement);

            collectionView.Source = this.DataSource;
            //collectionView.Delegate = this.GridViewDelegate;

            base.SetNativeControl (collectionView);

        }

        private void Unbind (GridView oldElement)
        {
            if (oldElement != null) {
                oldElement.PropertyChanging += ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                if (oldElement.ItemsSource is INotifyCollectionChanged) {
                    (oldElement.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
                }
            }
        }

        private void Bind (GridView newElement)
        {
            if (newElement != null) {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                if (newElement.ItemsSource is INotifyCollectionChanged) {
                    (newElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
                }
            }
        }

        private void ElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource") {
                if (this.Element.ItemsSource is INotifyCollectionChanged) {
                    (this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
                }
            }
        }

        private void ElementPropertyChanging (object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "ItemsSource") {
                if (this.Element.ItemsSource is INotifyCollectionChanged) {
                    (this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
                }
            }
        }

        private void DataCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try {
                if(Control != null)
                    Control.ReloadData();
            } catch (Exception ex) {

            }
        }

        private GridDataSource dataSource;

        private GridDataSource DataSource {
            get {
                return dataSource ??
                (dataSource =
                        new GridDataSource (this.GetCell, this.RowsInSection,this.ItemSelected));
            }
        }

        private GridViewDelegate gridViewDelegate;

        private GridViewDelegate GridViewDelegate {
            get {
                return gridViewDelegate ??
                (gridViewDelegate =
                        new GridViewDelegate (this.ItemSelected));
            }
        }

        public int RowsInSection (UICollectionView collectionView, int section)
        {
            return (this.Element.ItemsSource as ICollection).Count;
        }

        public void ItemSelected (UICollectionView tableView, NSIndexPath indexPath)
        {
            var item = this.Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
            this.Element.InvokeItemSelectedEvent(this, item);

        }

        public UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = this.Element.ItemsSource.Cast<object> ().ElementAt (indexPath.Row);
            var viewCellBinded = (Element.ItemTemplate.CreateContent () as ViewCell);
            viewCellBinded.BindingContext = item;

            return GetCell (collectionView, viewCellBinded, indexPath);
        }

        protected virtual UICollectionViewCell GetCell (UICollectionView collectionView, ViewCell item, NSIndexPath indexPath)
        {
            var collectionCell = collectionView.DequeueReusableCell (new NSString (GridViewCell.Key), indexPath) as GridViewCell;

            collectionCell.ViewCell = item;

            return collectionCell as UICollectionViewCell;
        }
           }
}
