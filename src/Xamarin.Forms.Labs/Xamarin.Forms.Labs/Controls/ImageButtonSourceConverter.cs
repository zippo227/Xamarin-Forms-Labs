using System;
using System.Globalization;

namespace Xamarin.Forms.Labs.Controls
{
    public class ImageSourceConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(CultureInfo culture, object value)
        {
           if (value == null)
            {
                return null;
            }

            var str = value as string;
            if (str != null)
            {
                Uri result;
                if (!Uri.TryCreate(str, UriKind.Absolute, out result) || !(result.Scheme != "file"))
                {
                    return ImageSource.FromFile(str);
                }
                return ImageSource.FromUri(result);
            }
            throw new InvalidOperationException(
                string.Format(
                    "Cannot convert \"{0}\" into {1}",
                    new[] { value, typeof(ImageSource) }));
        }
    }
}
