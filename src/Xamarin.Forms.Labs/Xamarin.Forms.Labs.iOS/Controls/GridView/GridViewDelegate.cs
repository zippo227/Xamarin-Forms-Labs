using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class GridViewDelegate: UICollectionViewDelegate
    {
        public delegate void OnItemSelected (UICollectionView tableView, NSIndexPath indexPath);

        private readonly OnItemSelected onItemSelected;

        public GridViewDelegate (OnItemSelected onItemSelected)
        {
            this.onItemSelected = onItemSelected;
        }

        public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
        {
            onItemSelected (collectionView, indexPath);
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            this.onItemSelected.Invoke(collectionView, indexPath);
        }

    }
}

