namespace XLabs.Platform.Droid.Extensions
{
	using System;

	public static class UriExtensions
    {
        public static Android.Net.Uri ToAndroidUri(this Uri uri)
        {
            return Android.Net.Uri.Parse(uri.AbsoluteUri);
        }

        public static Uri ToSystemUri(this Android.Net.Uri uri)
        {
            return new Uri(uri.ToString());
        }
    }
}