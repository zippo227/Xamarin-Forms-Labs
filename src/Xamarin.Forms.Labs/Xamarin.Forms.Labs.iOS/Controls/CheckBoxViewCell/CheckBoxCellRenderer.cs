using System;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Labs.iOS.Controls;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckBoxCellRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
	public class CheckBoxCellRenderer : ExtendedTextCellRenderer
	{
		public CheckBoxCellRenderer ()
		{
		}

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

