using System;
using System.Drawing;
#if __UNIFIED__
using UIKit;
using Foundation;
using CoreGraphics;
#elif __IOS__
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreGraphics;
#endif
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class GridCollectionView : UICollectionView
    {
        public bool SelectionEnable 
        {
            get;
            set;
        }

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

#if __UNIFIED__
        public override void Draw(CGRect rect)
#elif __IOS__
        public override void Draw (RectangleF rect)
#endif
        {
            this.CollectionViewLayout.InvalidateLayout ();
            base.Draw(rect);
        }

        public double RowSpacing 
        {
            get 
            { 
                return (double)(this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing;
            }
            set 
            {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing = (float)value;
            }
        }

        public double ColumnSpacing 
        {
            get
            { 
                return (double)(this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing;
            }
            set 
            {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing = (float)value;
            }
        }

        public SizeF ItemSize 
        {
            get 
            { 
#if __UNIFIED__
                return (this.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize.ToSizeF();
#elif __IOS__
                return (this.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize;
#endif
            }
            set 
            {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = value;
            }
        }
    }
}