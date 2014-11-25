using System;
#if __UNIFIED__
using Foundation;
using UIKit;
#elif __IOS__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using System.Drawing;

[assembly: ExportRenderer(typeof(EditableListView<Object>), typeof(EditableListViewRenderer<Object>))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
	public class EditableListViewRenderer<T> : ViewRenderer<EditableListView<T>, UITableView>
	{
		protected float _rowHeight = 44;
		private EditableListViewSource _editableListViewSource;
		private UITableView _tableView;

		protected override void OnElementChanged(ElementChangedEventArgs<EditableListView<T>> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				_tableView = new UITableView(new RectangleF(0,0,1,1), UITableViewStyle.Plain);
				SetNativeControl(_tableView);
			}

			Unbind(e.OldElement);
			Bind(e.NewElement);

			if (e.NewElement.CellHeight > 0)
				_rowHeight = e.NewElement.CellHeight;

			_editableListViewSource = new EditableListViewSource(this);
			_tableView.Source = _editableListViewSource;

			_tableView.SetEditing(true, true);
			_tableView.TableFooterView = new UIView();
		}

		private void Unbind(EditableListView<T> oldElement)
		{
			if (oldElement != null)
			{
				oldElement.PropertyChanged -= ElementPropertyChanged;
				oldElement.Source.CollectionChanged += DataCollectionChanged;
			}
		}

		private void Bind(EditableListView<T> newElement)
		{
			if (newElement != null)
			{
				newElement.PropertyChanged += ElementPropertyChanged;
				newElement.Source.CollectionChanged += DataCollectionChanged;
			}
		}

		private void DataCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			_tableView.ReloadData();
		}

		private void ElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Source")
			{
				Element.Source.CollectionChanged += DataCollectionChanged;
			}
		}

		private class EditableListViewSource : UITableViewSource 
		{
			EditableListViewRenderer<T> _containerRenderer;

			public EditableListViewSource(EditableListViewRenderer<T> containerRenderer)
			{
				_containerRenderer = containerRenderer;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row == _containerRenderer.Element.Source.Count)
				{
					var addCell = new UITableViewCell();
					addCell.TextLabel.Text = "Add ...";
					return addCell;
				}
				else
				{
					var item = _containerRenderer.Element.Source[indexPath.Row];

					View view = Activator.CreateInstance(_containerRenderer.Element.ViewType) as View;

					view.BindingContext = item;
					ViewCell viewCell = new ViewCell();
					viewCell.View = view;
					UITableViewCell cell = new ViewCellRenderer().GetCell(viewCell,null,tableView);
					return cell;
				}
			}

			public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete)
				{
					_containerRenderer.Element.Source.RemoveAt(indexPath.Row);
					tableView.ReloadData();
				}
				else if (editingStyle == UITableViewCellEditingStyle.Insert)
				{
					_containerRenderer.Element.ExecuteAddRow();
				}

			}

			public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Row == _containerRenderer.Element.Source.Count)
					return UITableViewCellEditingStyle.Insert;
				else
					return UITableViewCellEditingStyle.Delete;
			}

#if __UNIFIED__
            public override nint RowsInSection(UITableView tableView, nint section)
#elif __IOS__
            public override int RowsInSection(UITableView tableView, int section)
#endif
			{
				return _containerRenderer.Element.Source.Count + 1;
			}

			public override bool CanMoveRow(UITableView tableView, NSIndexPath indexPath)
			{
				return indexPath.Row != _containerRenderer.Element.Source.Count;
			}

			public override NSIndexPath CustomizeMoveTarget(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath proposedIndexPath)
			{
				if (proposedIndexPath.Row == _containerRenderer.Element.Source.Count)
					return sourceIndexPath;
				else
					return proposedIndexPath;
			}

			public override void MoveRow(UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
				_containerRenderer.Element.Source.Move(sourceIndexPath.Row, destinationIndexPath.Row);
			}

#if __UNIFIED__
            public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
#elif __IOS__
            public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
#endif
            {
				return _containerRenderer._rowHeight;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{

			}
		}

	}
}