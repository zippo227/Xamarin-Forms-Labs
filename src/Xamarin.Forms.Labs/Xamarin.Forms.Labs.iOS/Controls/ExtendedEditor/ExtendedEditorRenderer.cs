using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
	public class ExtendedEditorRenderer : EditorRenderer
	{
		public ExtendedEditorRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Editor> e)
		{
			base.OnElementChanged (e);

			var view = (ExtendedEditor)Element;

			if (view.Font != null)
				Control.Font = view.Font.ToUIFont ();
		}
	}
}

