using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Controls
{
	/// <summary>
	/// Class ExtendedLabel.
	/// </summary>
	public class ExtendedLabel : Label
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtendedLabel"/> class.
		/// </summary>
		public ExtendedLabel ()
		{
		}


		/// <summary>
		/// The font size property
		/// </summary>
		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create<ExtendedLabel,double> (
				p => p.FontSize, -1);

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>The size of the font.</value>
		public double FontSize {
			get { return (double)GetValue (FontSizeProperty); }
			set { SetValue (FontSizeProperty,value); }
		}

		/// <summary>
		/// The font name android property
		/// </summary>
		public static readonly BindableProperty FontNameAndroidProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontNameAndroid, string.Empty);

		/// <summary>
		/// Gets or sets the font name android.
		/// </summary>
		/// <value>The font name android.</value>
		public string FontNameAndroid {
			get { return (string)GetValue (FontNameAndroidProperty); }
			set { SetValue (FontNameAndroidProperty, value); }
		}

		/// <summary>
		/// The font name ios property
		/// </summary>
		public static readonly BindableProperty FontNameIOSProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontNameIOS, string.Empty);

		/// <summary>
		/// Gets or sets the font name ios.
		/// </summary>
		/// <value>The font name ios.</value>
		public string FontNameIOS {
			get { return (string)GetValue (FontNameIOSProperty); }
			set { SetValue (FontNameIOSProperty, value); }
		}

		/// <summary>
		/// The font name wp property
		/// </summary>
		public static readonly BindableProperty FontNameWPProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontNameWP, string.Empty);

		/// <summary>
		/// Gets or sets the font name wp.
		/// </summary>
		/// <value>The font name wp.</value>
		public string FontNameWP {
			get { return (string)GetValue (FontNameWPProperty); }
			set { SetValue (FontNameWPProperty, value); }
		}


		/// <summary>
		/// The font name property
		/// </summary>
		public static readonly BindableProperty FontNameProperty =
			BindableProperty.Create<ExtendedLabel,string> (
				p => p.FontName, string.Empty);

		/// <summary>
		/// Gets or sets the name of the font.
		/// </summary>
		/// <value>The name of the font.</value>
		public string FontName {
			get { return (string)GetValue (FontNameProperty); }
			set { SetValue (FontNameProperty, value);
			
			}
		}
	}
}

