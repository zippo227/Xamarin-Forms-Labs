using System;
using System.Windows.Media;
using WApplication = System.Windows.Application;

namespace Xamarin.Forms.Labs.WP8
{
	public static class FontExtensions
	{
		public static double GetHeight(this Font font)
		{
			if (font.UseNamedSize)
			{
				switch (font.NamedSize)
				{
					case NamedSize.Micro:
						return (double) WApplication.Current.Resources[(object) "PhoneFontSizeSmall"] - 3.0;
					case NamedSize.Small:
						return (double) WApplication.Current.Resources[(object) "PhoneFontSizeSmall"];
					case NamedSize.Medium:
						return (double) WApplication.Current.Resources[(object) "PhoneFontSizeNormal"];
					case NamedSize.Large:
						return (double) WApplication.Current.Resources[(object) "PhoneFontSizeLarge"];
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			
			return font.FontSize;
		}

		public static FontFamily GetFontFamily(this Font font)
		{
			return string.IsNullOrEmpty (font.FontFamily) ? (FontFamily) WApplication.Current.Resources[(object) "PhoneFontFamilyNormal"] : new FontFamily (font.FontFamily);
		}
	}
}
