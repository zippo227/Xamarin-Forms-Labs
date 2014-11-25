using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Labs;
#if __UNIFIED__
using UIKit;
#elif __IOS__
using MonoTouch.UIKit;
#endif
using System.Drawing;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(ExtendedTextCell), typeof(ExtendedTextCellRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls
{
	public class ExtendedTextCellRenderer : TextCellRenderer
	{
		public override UITableViewCell GetCell (Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var extendedCell = (ExtendedTextCell)item;
			var cell = base.GetCell (item, reusableCell, tv);
			if (cell != null) {
				cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor ();
				cell.SeparatorInset = new UIEdgeInsets ((float)extendedCell.SeparatorPadding.Top, (float)extendedCell.SeparatorPadding.Left,
					(float)extendedCell.SeparatorPadding.Bottom, (float)extendedCell.SeparatorPadding.Right);

				if (extendedCell.ShowDisclousure) {
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
					if (!string.IsNullOrEmpty (extendedCell.DisclousureImage)) {
						var detailDisclosureButton = UIButton.FromType (UIButtonType.Custom);
						detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Normal);
						detailDisclosureButton.SetImage (UIImage.FromBundle (extendedCell.DisclousureImage), UIControlState.Selected);

						detailDisclosureButton.Frame = new RectangleF (0f, 0f, 30f, 30f);
						detailDisclosureButton.TouchUpInside += (object sender, EventArgs e) => {
							var index = tv.IndexPathForCell (cell);
							tv.SelectRow (index, true, UITableViewScrollPosition.None);
							tv.Source.AccessoryButtonTapped (tv, index);
						};
						cell.AccessoryView = detailDisclosureButton;
					}
				}
			}

			if(!extendedCell.ShowSeparator)
				tv.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			tv.SeparatorColor = extendedCell.SeparatorColor.ToUIColor();


			return cell;
		}


	}
}

