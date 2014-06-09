using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using MonoTouch.UIKit;
using XForms.Toolkit.Controls;
using XForms.Toolkit.iOS;

[assembly: ExportRenderer (typeof (ExtendedLabel), typeof (ExtendedLabelRenderer))]
namespace XForms.Toolkit.iOS
{
	public class ExtendedLabelRenderer : LabelRenderer
	{
		public ExtendedLabelRenderer ()
		{
		}

		protected override void OnModelSet(VisualElement model)
		{
			base.OnModelSet (model);

			var view = (ExtendedLabel)model;
			var control = (UILabel) Control;

			UpdateUi (view, control);
		}

		void UpdateUi (ExtendedLabel view, UILabel control)
		{
			if (!string.IsNullOrEmpty (view.FontName)) {
				var font = UIFont.FromName (view.FontName,
					(view.FontSize > 0) ? (float)view.FontSize : 12.0f);
				if (font != null)
					control.Font = font;

			}
			if (!string.IsNullOrEmpty (view.FontNameIOS)) {
				var font = UIFont.FromName (view.FontNameIOS,
					(view.FontSize > 0) ? (float)view.FontSize : 12.0f);
				if (font != null)
					control.Font = font;

			}

		}
	}
}

