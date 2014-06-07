using System;
using Xamarin.Forms;

namespace XForms.Toolkit.Controls
{
	public class ExtendedLabel : Label
	{
		public ExtendedLabel ()
		{
		}


		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<ExtendedLabel,double> (
				p => p.FontSize, -1);

		public double FontSize {
			get { return (double)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty,value); }
		}

		public static readonly BindableProperty FontNameAndroidProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontNameAndroid, string.Empty);

		public string FontNameAndroid {
			get { return (string)GetValue (FontNameAndroidProperty); }
			set { SetValue (FontNameAndroidProperty, value); }
		}

		public static readonly BindableProperty FontNameIOSProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontNameIOS, string.Empty);

		public string FontNameIOS {
			get { return (string)GetValue (FontNameIOSProperty); }
			set { SetValue (FontNameIOSProperty, value); }
		}

		public static readonly BindableProperty FontNameWPProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontNameWP, string.Empty);

		public string FontNameWP {
			get { return (string)GetValue (FontNameWPProperty); }
			set { SetValue (FontNameWPProperty, value); }
		}


		public static readonly BindableProperty FontNameProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontName, string.Empty);

		public string FontName {
			get { return (string)GetValue (FontNameProperty); }
			set { SetValue (FontNameProperty, value);
			
			}
		}
	}
}

