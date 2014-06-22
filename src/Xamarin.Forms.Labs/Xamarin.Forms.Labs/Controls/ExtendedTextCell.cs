using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Controls
{
	public class ExtendedTextCell : TextCell
	{
		public ExtendedTextCell ()
		{
		}


		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<ExtendedTextCell,double> (
				p => p.FontSize, -1);

		public double FontSize {
			get { return (double)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty,value); }
		}

		public static readonly BindableProperty FontNameAndroidProperty =
			BindableProperty.Create<ExtendedTextCell,string> (
				p => p.FontNameAndroid, string.Empty);

		public string FontNameAndroid {
			get { return (string)GetValue (FontNameAndroidProperty); }
			set { SetValue (FontNameAndroidProperty, value); }
		}

		public static readonly BindableProperty FontNameIOSProperty =
			BindableProperty.Create<ExtendedTextCell,string> (
				p => p.FontNameIOS, string.Empty);

		public string FontNameIOS {
			get { return (string)GetValue (FontNameIOSProperty); }
			set { SetValue (FontNameIOSProperty, value); }
		}

		public static readonly BindableProperty FontNameWPProperty =
			BindableProperty.Create<ExtendedTextCell,string> (
				p => p.FontNameWP, string.Empty);

		public string FontNameWP {
			get { return (string)GetValue (FontNameWPProperty); }
			set { SetValue (FontNameWPProperty, value); }
		}


		public static readonly BindableProperty FontNameProperty =
			BindableProperty.Create<ExtendedTextCell,string> (
				p => p.FontName, string.Empty);

		public string FontName {
			get { return (string)GetValue (FontNameProperty); }
			set { SetValue (FontNameProperty, value);
			
			}
		}

		public static readonly BindableProperty BackgroundColorProperty =
			BindableProperty.Create<ExtendedTextCell, Color> (p => p.BackgroundColor, Color.Transparent);

		public Color BackgroundColor {
			get { return (Color)GetValue (BackgroundColorProperty); }
			set { SetValue (BackgroundColorProperty, value); }
		}

		public static readonly BindableProperty SeparatorColorProperty =
			BindableProperty.Create<ExtendedTextCell, Color> (p => p.SeparatorColor, Color.FromRgba(199,199,204,255));

		public Color SeparatorColor {
			get { return (Color)GetValue (SeparatorColorProperty); }
			set { SetValue (SeparatorColorProperty, value); }
		}

		public static readonly BindableProperty SeparatorPaddingProperty =
			BindableProperty.Create<ExtendedTextCell, Thickness> (p => p.SeparatorPadding, default(Thickness));

		public Thickness SeparatorPadding {
			get { return (Thickness)GetValue (SeparatorPaddingProperty); }
			set { SetValue (SeparatorPaddingProperty, value); }
		}


		public static readonly BindableProperty ShowSeparatorProperty =
			BindableProperty.Create<ExtendedTextCell, bool> (p => p.ShowSeparator, true);

		public bool ShowSeparator {
			get { return (bool)GetValue (ShowSeparatorProperty); }
			set { SetValue (ShowSeparatorProperty, value); }
		}


		public static readonly BindableProperty ShowDisclousureProperty =
			BindableProperty.Create<ExtendedTextCell, bool> (p => p.ShowDisclousure, true);

		public bool ShowDisclousure {
			get { return (bool)GetValue (ShowDisclousureProperty); }
			set { SetValue (ShowDisclousureProperty, value); }
		}

		public static readonly BindableProperty DisclousureImageProperty =
			BindableProperty.Create<ExtendedTextCell,string> (
				p => p.DisclousureImage, string.Empty);

		public string DisclousureImage {
			get { return (string)GetValue (DisclousureImageProperty); }
			set { SetValue (DisclousureImageProperty, value); }
		}
	}
}

