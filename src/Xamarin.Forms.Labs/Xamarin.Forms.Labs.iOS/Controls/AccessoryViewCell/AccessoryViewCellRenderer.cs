using System;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using System.Drawing;

[assembly: ExportRenderer(typeof(AccessoryViewCell), typeof(AccessoryViewCellRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
	public class AccessoryViewCellRenderer : ExtendedTextCellRenderer
	{
		public AccessoryViewCellRenderer ()
		{
		}

		public override MonoTouch.UIKit.UITableViewCell GetCell (Cell item, MonoTouch.UIKit.UITableView tv)
		{
			AccessoryViewCell viewCell = item as AccessoryViewCell;
			var nativeCell = base.GetCell (item, tv);

			var frame = new RectangleF (0, 0, (float)viewCell.AccessoryView.WidthRequest, (float)viewCell.AccessoryView.HeightRequest);
			var nativeView = RendererFactory.GetRenderer (viewCell.AccessoryView).NativeView;
			nativeView.Frame = frame;
			nativeView.Bounds = frame;
			nativeCell.AccessoryView = nativeView;

			return nativeCell;
		}
	}
}

