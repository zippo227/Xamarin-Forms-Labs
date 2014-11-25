using System;
using Xamarin.Forms.Platform.iOS;
#if __UNIFIED__
using UIKit;
#elif __IOS__
using MonoTouch.UIKit;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;

[assembly: ExportRenderer(typeof(ExtendedEntryCell), typeof(ExtendedEntryCellRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class ExtendedEntryCellRenderer : EntryCellRenderer
    {
	    public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
	    {
			ExtendedEntryCell entryCell = ((ExtendedEntryCell) item);
			var cell =  base.GetCell (item, reusableCell, tv);
			if (cell != null) {
				UITextField textField = (UITextField) cell.ContentView.Subviews[0];
				textField.SecureTextEntry = entryCell.IsPassword;
			}
			return cell;
	    }

    } 
}

