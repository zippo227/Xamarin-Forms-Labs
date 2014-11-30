using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntryCell), typeof(ExtendedEntryCellRenderer))]
namespace XLabs.Forms.Controls
{
	using MonoTouch.UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	/// <summary>
	/// Class ExtendedEntryCellRenderer.
	/// </summary>
	public class ExtendedEntryCellRenderer : EntryCellRenderer
	{
		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="tv">The tv.</param>
		/// <returns>UITableViewCell.</returns>
		public override UITableViewCell GetCell(Cell item, UITableView tv)
		{
			ExtendedEntryCell entryCell = ((ExtendedEntryCell)item);
			var cell = base.GetCell (item, tv);
			if (cell != null) {
				UITextField textField = (UITextField)cell.ContentView.Subviews [0]; 
				textField.SecureTextEntry = entryCell.IsPassword;
			}
			return cell;
		}
	} 
}

