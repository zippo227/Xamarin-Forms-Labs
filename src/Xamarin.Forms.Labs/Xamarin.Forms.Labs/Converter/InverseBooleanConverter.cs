using System;

namespace Xamarin.Forms.Labs.Converter
{
	public class InverseBooleanConverter : IValueConverter
	{
		public InverseBooleanConverter ()
		{
		}

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !((bool)value);
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return !((bool)value);
		}
	}
}

