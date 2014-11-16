namespace XLabs.Platform.WP8.Extensions
{
	using Microsoft.Phone.Controls;

	public static class OrientationExtensions
    {
        public static Orientation ToOrientation(this PageOrientation orientation)
        {
            return (Orientation)((int)orientation);
        }

        public static PageOrientation ToPageOrientation(this Orientation orientation)
        {
            return (PageOrientation)((int)orientation);
        }
    }
}
