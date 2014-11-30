using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class GridDataSource : UICollectionViewSource
    {
        public delegate UICollectionViewCell OnGetCell(UICollectionView collectionView, NSIndexPath indexPath);
        public delegate int OnRowsInSection(UICollectionView collectionView, int section);
        public delegate void OnItemSelected(UICollectionView collectionView, NSIndexPath indexPath);

        private readonly OnGetCell onGetCell;
        private readonly OnRowsInSection onRowsInSection;
        private readonly OnItemSelected onItemSelected;

        public GridDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection, OnItemSelected onItemSelected)
        {
            this.onGetCell = onGetCell;
            this.onRowsInSection = onRowsInSection;
            this.onItemSelected = onItemSelected;
        }

        #region implemented abstract members of UICollectionViewDataSource

        public override int GetItemsCount (UICollectionView collectionView, int section)
        {
            return onRowsInSection(collectionView, section);
        }

        public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
        {
            onItemSelected (collectionView, indexPath);
        }
        public override UICollectionViewCell GetCell (UICollectionView collectionView, NSIndexPath indexPath)
        {
            UICollectionViewCell cell = onGetCell (collectionView, indexPath);
            if ((collectionView as GridCollectionView).SelectionEnable) {
                cell.AddGestureRecognizer (new UITapGestureRecognizer ((v) => {
                    this.ItemSelected (collectionView, indexPath);
                }));
            } else
                cell.SelectedBackgroundView = new UIView();

            return cell;
        }

        #endregion
    }
}

