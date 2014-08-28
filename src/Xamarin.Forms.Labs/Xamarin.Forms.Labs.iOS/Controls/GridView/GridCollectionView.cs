using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.CoreGraphics;
using System.ComponentModel;

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class GridCollectionView : UICollectionView
    {
        public GridCollectionView () : this (default(RectangleF))
        {
        }

        public GridCollectionView (RectangleF frm) : base (default(RectangleF), new UICollectionViewFlowLayout () { })
        {
            this.AutoresizingMask = UIViewAutoresizing.All;
            this.ContentMode = UIViewContentMode.ScaleToFill;
            RegisterClassForCell (typeof(GridViewCell), new NSString (GridViewCell.Key));

        }

        public override UICollectionViewCell CellForItem(NSIndexPath indexPath)
        {
            return base.CellForItem(indexPath);
        }

        public override void Draw (RectangleF rect)
        {
            this.CollectionViewLayout.InvalidateLayout ();

            base.Draw (rect);
        }

        public double RowSpacing {
            get { 
                return (double)(this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing;
            }
            set {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing = (float)value;
            }
        }

        public double ColumnSpacing {
            get { 
                return (double)(this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing;
            }
            set {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing = (float)value;
            }
        }

        public SizeF ItemSize {
            get { 
                return (this.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize;
            }
            set {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = value;
            }
        }

      

    }

    //GridView cell
    public class GridViewCell : UICollectionViewCell
    {
        public const string Key = "GridViewCell";

        private ViewCell viewCell;
        private UIView view;

        public ViewCell ViewCell {
            get {
                return this.viewCell;
            }
            set {
                if (this.viewCell == value) {
                    return;
                }
                this.UpdateCell (value);
            }
        }

        [Export ("initWithFrame:")]
        public GridViewCell (System.Drawing.RectangleF frame) : base (frame)
        {
//            SelectedBackgroundView = new GridItemSelectedViewOverlay (frame);
//            this.BringSubviewToFront (SelectedBackgroundView);

        }

        private void UpdateCell (ViewCell cell)
        {
            if (this.viewCell != null) {
                //this.viewCell.SendDisappearing ();
                this.viewCell.PropertyChanged -= new PropertyChangedEventHandler (this.HandlePropertyChanged);
            }
            this.viewCell = cell;
            this.viewCell.PropertyChanged += new PropertyChangedEventHandler (this.HandlePropertyChanged);
            //this.viewCell.SendAppearing ();
            this.UpdateView ();
        }

        private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            this.UpdateView ();
        }

        private void UpdateView ()
        {

            if (this.view != null) {
                this.view.RemoveFromSuperview ();
            }
            this.view = RendererFactory.GetRenderer (this.viewCell.View).NativeView;
            this.view.AutoresizingMask = UIViewAutoresizing.All;
            this.view.ContentMode = UIViewContentMode.ScaleToFill;

            this.AddSubview (this.view);
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            RectangleF frame = this.ContentView.Frame;
            frame.X = (this.Bounds.Width - frame.Width) / 2;
            frame.Y = (this.Bounds.Height - frame.Height) / 2;
            this.ViewCell.View.Layout (frame.ToRectangle ());
            this.view.Frame = frame;
        }
    }

    //SelectedView Overlay Windows8 style
    public class GridItemSelectedViewOverlay : UIView
    {

        public GridItemSelectedViewOverlay (RectangleF frame) : base (frame)
        {
            BackgroundColor = UIColor.Clear;
        }

        public override void Draw (RectangleF rect)
        {
            using (var g = UIGraphics.GetCurrentContext ()) {
                g.SetLineWidth (10);
                UIColor.FromRGB (64, 30, 168).SetStroke ();
                UIColor.Clear.SetFill ();

                //create geometry
                var path = new CGPath ();
                path.AddRect (rect);
                path.CloseSubpath ();

                //add geometry to graphics context and draw it
                g.AddPath (path);
                g.DrawPath (CGPathDrawingMode.Stroke);
            }
        }
    }
}

