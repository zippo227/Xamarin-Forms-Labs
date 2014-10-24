using System;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using System.Collections.Specialized;
using Xamarin.Forms;
using Android.Content;
using Android.Views;
using System.Collections;
using System.Linq;
using Android.Graphics;
using System.Net;
using Xamarin.Forms.Labs.Droid.Controls.GridView;
using GridView = Android.Widget.GridView;
using Android.Content.Res;

[assembly: ExportRenderer (typeof(Xamarin.Forms.Labs.Controls.GridView), typeof(GridViewRenderer))]
namespace Xamarin.Forms.Labs.Droid.Controls.GridView
{
    public class GridViewRenderer : ViewRenderer<Xamarin.Forms.Labs.Controls.GridView, Android.Widget.GridView>
    {
        private Android.Content.Res.Orientation orientation = Android.Content.Res.Orientation.Undefined;
        public GridViewRenderer ()
        {
        }

        protected override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            if (newConfig.Orientation != orientation)
                OnElementChanged(new ElementChangedEventArgs<Labs.Controls.GridView>(this.Element, this.Element));
        }

        protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Labs.Controls.GridView> e)
        {
            base.OnElementChanged (e);

            var collectionView = new Android.Widget.GridView (Forms.Context);
            collectionView.SetGravity(GravityFlags.Center);
            collectionView.SetColumnWidth (Convert.ToInt32(Element.ItemWidth));
            collectionView.StretchMode = StretchMode.StretchColumnWidth;
    
            var metrics = Resources.DisplayMetrics;
            var spacing = (int)e.NewElement.ColumnSpacing;
            var width = metrics.WidthPixels;
            var itemWidth = (int)e.NewElement.ItemWidth;

            int noOfColumns = width / (itemWidth + spacing);
            // If possible add another row without spacing (because the number of columns will be one less than the number of spacings)
            if (width - (noOfColumns * (itemWidth + spacing)) >= itemWidth)
                noOfColumns++;

            collectionView.SetNumColumns (noOfColumns);
            collectionView.SetPadding(Convert.ToInt32(Element.Padding.Left),Convert.ToInt32(Element.Padding.Top), Convert.ToInt32(Element.Padding.Right),Convert.ToInt32(Element.Padding.Bottom));

            collectionView.SetBackgroundColor (Element.BackgroundColor.ToAndroid ());
            collectionView.SetHorizontalSpacing (Convert.ToInt32(Element.RowSpacing));
            collectionView.SetVerticalSpacing(Convert.ToInt32(Element.ColumnSpacing));

            this.Unbind(e.OldElement);
            this.Bind(e.NewElement);

            collectionView.Adapter = this.DataSource;

            collectionView.ItemClick += collectionView_ItemClick;

            base.SetNativeControl(collectionView);

        }


        void collectionView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = this.Element.ItemsSource.Cast<object>().ElementAt(e.Position);
            this.Element.InvokeItemSelectedEvent(this, item);
        }

        private void Unbind(Xamarin.Forms.Labs.Controls.GridView oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging += ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                if (oldElement.ItemsSource is INotifyCollectionChanged) {
                    (oldElement.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
                }
            }
        }

        private void Bind(Xamarin.Forms.Labs.Controls.GridView newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                if (newElement.ItemsSource is INotifyCollectionChanged) {
                    (newElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
                }
            }
        }

        private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                if (this.Element.ItemsSource is INotifyCollectionChanged) {
                    (this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
                }
            }
        }
        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyName == "ItemsSource")
            {
                if (this.Element.ItemsSource is INotifyCollectionChanged) {
                    (this.Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
                }
            }
        }

        private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //  Control.ReloadData();
        }

        private GridDataSource dataSource;
        private GridDataSource DataSource
        {
            get
            {
                return dataSource ??
                    (dataSource =
                        new GridDataSource( this.GetCell,this.RowsInSection));
            }
        }

        public int RowsInSection()
        {
            return (this.Element.ItemsSource as ICollection).Count;
        }

        public  global::Android.Views.View GetCell(int position, global::Android.Views.View convertView, ViewGroup parent)
        {
            var item = this.Element.ItemsSource.Cast<object>().ElementAt(position);
            var viewCellBinded = (Element.ItemTemplate.CreateContent () as ViewCell);
            viewCellBinded.BindingContext = item;
            var view = RendererFactory.GetRenderer (viewCellBinded.View);
            // Platform.SetRenderer (viewCellBinded.View, view);
            view.ViewGroup.LayoutParameters = new  Android.Widget.GridView.LayoutParams (Convert.ToInt32(this.Element.ItemWidth), Convert.ToInt32(this.Element.ItemHeight));
            view.ViewGroup.SetBackgroundColor (global::Android.Graphics.Color.Blue);
            return view.ViewGroup;
            //                this.AddView (this.view.ViewGroup);

            //            GridViewCellRenderer render = new GridViewCellRenderer ();
            //           
            //            return render.GetCell (viewCellBinded, convertView, parent, this.Context);;
            //          //  view.LayoutParameters = new GridView.LayoutParams (this.Element.ItemWidth,this.Element.ItemHeight);
            //            return view;
            //            ImageView imageView;
            //
            //            if (convertView == null) {  // if it's not recycled, initialize some attributes
            //                imageView = new ImageView (Forms.Context);
            //                imageView.LayoutParameters = new GridView.LayoutParams (85, 85);
            //                imageView.SetScaleType (ImageView.ScaleType.CenterCrop);
            //                imageView.SetPadding (8, 8, 8, 8);
            //            } else {
            //                imageView = (ImageView)convertView;
            //            }
            //            var imageBitmap = GetImageBitmapFromUrl("http://xamarin.com/resources/design/home/devices.png");
            //            imageView.SetImageBitmap(imageBitmap);
            //            return imageView;
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }

    public class GridDataSource : BaseAdapter
    {
        Context context;

        public delegate global::Android.Views.View OnGetCell(int position, global::Android.Views.View convertView, ViewGroup parent);
        public delegate int OnRowsInSection();

        private readonly OnGetCell onGetCell;
        private readonly OnRowsInSection onRowsInSection;

        public GridDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection)
        {
            this.onGetCell = onGetCell;
            this.onRowsInSection = onRowsInSection;
        }

        public GridDataSource (Context c)
        {
            context = c;
        }

        public override int Count {
            get { return onRowsInSection (); }
        }

        public override Java.Lang.Object GetItem (int position)
        {
            return null;
        }

        public override long GetItemId (int position)
        {
            return 0;
        }

        public override global::Android.Views.View GetView (int position, global::Android.Views.View convertView, ViewGroup parent)
        {
            return onGetCell (position, convertView, parent);
        }

    }

    public class GridViewCellRenderer : CellRenderer
    {
        //
        // Methods
        //
        protected override global::Android.Views.View GetCellCore (Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
        {
            ViewCell viewCell = (ViewCell)item;
            GridViewCellRenderer.ViewCellContainer viewCellContainer = convertView as GridViewCellRenderer.ViewCellContainer;
            if (viewCellContainer != null) {
                viewCellContainer.Update (viewCell);
                return viewCellContainer;
            }

            IVisualElementRenderer renderer = RendererFactory.GetRenderer (viewCell.View);
            //   Platform.SetRenderer (viewCell.View, renderer);
            // viewCell.View.IsPlatformEnabled = true;
            return new GridViewCellRenderer.ViewCellContainer (context, renderer, viewCell,parent);
        }

        //
        // Nested Types
        //
        private class ViewCellContainer : ViewGroup
        {

            IVisualElementRenderer view;
            global::Android.Views.View parent;
            ViewCell viewCell;
            public ViewCellContainer (Context context, IVisualElementRenderer view, ViewCell viewCell, global::Android.Views.View parent) : base (context)
            {

                this.view = view;
                this.parent = parent;
                //                this.unevenRows = unevenRows;
                //                this.rowHeight = rowHeight;
                this.viewCell = viewCell;
                this.AddView (view.ViewGroup);
            }

            public void Update (ViewCell cell)
            {
                IVisualElementRenderer visualElementRenderer = this.GetChildAt (0) as IVisualElementRenderer;
                //              Type type = Registrar.Registered.GetHandlerType (cell.View.GetType ()) ?? typeof(RendererFactory.DefaultRenderer);
                //                if (visualElementRenderer != null && visualElementRenderer.GetType () == type) {
                //                    this.viewCell = cell;
                //                    visualElementRenderer.SetElement (cell.View);
                //                    Platform.SetRenderer (cell.View, this.view);
                //                    cell.View.IsPlatformEnabled = true;
                //                    this.Invalidate ();
                //                    return;
                //                }
                //                this.RemoveView (this.view.ViewGroup);
                //                Platform.SetRenderer (this.viewCell.View, null);
                //                this.viewCell.View.IsPlatformEnabled = false;
                //                this.view.ViewGroup.Dispose ();
                //                this.viewCell = cell;
                //                this.view = RendererFactory.GetRenderer (this.viewCell.View);
                //                Platform.SetRenderer (this.viewCell.View, this.view);
                //                this.AddView (this.view.ViewGroup);
            }
            //
            protected override void OnLayout (bool changed, int l, int t, int r, int b)
            {
                double width = base.Context.FromPixels ((double)(r - l));
                double height = base.Context.FromPixels ((double)(b - t));
                this.view.Element.Layout (new Rectangle (0, 0, width, height));
                this.view.UpdateLayout ();
            }
            //
            //            protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
            //            {
            //                int size = View.MeasureSpec.GetSize (widthMeasureSpec);
            //                int measuredHeight;
            //               measuredHeight = (int)base.Context.ToPixels ((this.ParentRowHeight == -1) ? 44 : ((double)this.ParentRowHeight));
            //                base.SetMeasuredDimension (size, measuredHeight);
            //            }
        }
    }
}