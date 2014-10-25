using System;

namespace Xamarin.Forms.Labs.Controls
{
	public class ExtendedEditor : Editor
	{
		public ExtendedEditor ()
		{
		}

		public static readonly BindableProperty FontProperty = BindableProperty.Create<ExtendedEditor, Font> (p => p.Font, default(Font));

		public Font Font {
			get { return (Font)GetValue (FontProperty); }
			set { SetValue (FontProperty, value); }
		}
	}
}

