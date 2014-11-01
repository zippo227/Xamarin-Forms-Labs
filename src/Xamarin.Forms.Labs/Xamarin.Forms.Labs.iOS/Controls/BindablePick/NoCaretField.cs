using System;
using MonoTouch.UIKit;
using System.Drawing;

namespace Xamarin.Forms.Labs.iOS.Controls.BindablePick
{
	public class NoCaretField : UITextField
	{
		public NoCaretField() : base(default(RectangleF))
		{
		}
		public override RectangleF GetCaretRectForPosition(UITextPosition position)
		{
			return default(RectangleF);
		}
	}
}

