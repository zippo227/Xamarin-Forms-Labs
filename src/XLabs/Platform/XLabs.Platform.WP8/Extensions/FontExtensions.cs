namespace XLabs.Platform.WP8.Extensions
{
	using System;
	using System.Windows;
	using System.Windows.Media;

	public static class FontExtensions
    {
        public static double GetHeight(this Font font)
        {
            if (font.UseNamedSize)
            {
                switch (font.NamedSize)
                {
                    case NamedSize.Micro:
                        return (double)Application.Current.Resources[(object)"PhoneFontSizeSmall"] - 3.0;
                    case NamedSize.Small:
                        return (double)Application.Current.Resources[(object)"PhoneFontSizeSmall"];
                    case NamedSize.Medium:
                        return (double)Application.Current.Resources[(object)"PhoneFontSizeNormal"];
                    case NamedSize.Large:
                        return (double)Application.Current.Resources[(object)"PhoneFontSizeLarge"];
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            return font.FontSize;
        }

        public static FontFamily GetFontFamily(this Font font)
        {
            return string.IsNullOrEmpty(font.FontFamily) ? (FontFamily)Application.Current.Resources[(object)"PhoneFontFamilyNormal"] : new FontFamily(font.FontFamily);
        }
    }
}
