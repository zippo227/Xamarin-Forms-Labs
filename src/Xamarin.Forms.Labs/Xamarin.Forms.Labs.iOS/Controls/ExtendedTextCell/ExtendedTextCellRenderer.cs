using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Labs;
using MonoTouch.UIKit;
using System.Drawing;
using Xamarin.Forms.Labs.Controls;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(ExtendedTextCell), typeof(ExtendedTextCellRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls
{
	public class ExtendedTextCellRenderer : TextCellRenderer
	{
		private static readonly Color DefaultDetailColor = new Color(0.32, 0.4, 0.57);
		private static readonly Color DefaultTextColor = Color.Black;

		public override UITableViewCell GetCell(Cell item, UITableView tv)
		{
			var extendedCell = (ExtendedTextCell)item;

			TextCell textCell = (TextCell)item;
			UITableViewCellStyle style = UITableViewCellStyle.Subtitle;
			if (extendedCell.DetailLocation == Xamarin.Forms.Labs.Enums.TextCellDetailLocation.Right)
				style = UITableViewCellStyle.Value1;

			string fullName = item.GetType ().FullName;
			CellTableViewCell cell = tv.DequeueReusableCell (fullName) as CellTableViewCell;
			if (cell == null) {
				cell = new CellTableViewCell (style, fullName);
			}
			else {
				cell.Cell.PropertyChanged -= new PropertyChangedEventHandler (cell.HandlePropertyChanged);
			}
			cell.Cell = textCell;
			textCell.PropertyChanged += new PropertyChangedEventHandler (cell.HandlePropertyChanged);
			cell.PropertyChanged = new Action<object, PropertyChangedEventArgs> (this.HandlePropertyChanged);
			cell.TextLabel.Text = textCell.Text;
			cell.DetailTextLabel.Text = textCell.Detail;
			cell.TextLabel.TextColor = textCell.TextColor.ToUIColor (ExtendedTextCellRenderer.DefaultTextColor);
			cell.DetailTextLabel.TextColor = textCell.DetailColor.ToUIColor (ExtendedTextCellRenderer.DefaultDetailColor);

			base.UpdateBackground (cell, item);

			if (cell != null) {
				cell.BackgroundColor = extendedCell.BackgroundColor.ToUIColor ();
				cell.SeparatorInset = new UIEdgeInsets ((float)extendedCell.SeparatorPadding.Top, (float)extendedCell.SeparatorPadding.Left,
					(float)extendedCell.SeparatorPadding.Bottom, (float)extendedCell.SeparatorPadding.Right);

				if (extendedCell.ShowDisclousure) {
					cell.Accessory = MonoTouch.UIKit.UITableViewCellAccessory.DisclosureIndicator;
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

