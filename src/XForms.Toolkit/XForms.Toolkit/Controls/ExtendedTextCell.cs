using System;
using Xamarin.Forms;

namespace XForms.Toolkit.Controls
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
	}
}

