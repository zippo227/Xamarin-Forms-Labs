using Xamarin.Forms;

using XLabs.Forms.Controls;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckBoxCellRenderer))]

namespace XLabs.Forms.Controls
{
	using Xamarin.Forms;

	/// <summary>
	/// Class CheckBoxCellRenderer.
	/// </summary>
	public class CheckBoxCellRenderer : ExtendedTextCellRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CheckBoxCellRenderer"/> class.
		/// </summary>
		public CheckBoxCellRenderer ()
		{
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="tv">The tv.</param>
		/// <returns>UITableViewCell.</returns>
		public override MonoTouch.UIKit.UITableViewCell GetCell (Cell item, MonoTouch.UIKit.UITableView tv)
		{
			CheckboxCell viewCell = item as CheckboxCell;
			var nativeCell = base.GetCell (item, tv);

			if (viewCell.Checked)
				nativeCell.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.Checkmark;
			else 
				nativeCell.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.None;

			nativeCell.SelectionStyle = MonoTouch.UIKit.UITableViewCellSelectionStyle.None;

			viewCell.CheckedChanged += (s,e) => {
				tv.ReloadData();
			};

			return nativeCell;
		}

	}
}

