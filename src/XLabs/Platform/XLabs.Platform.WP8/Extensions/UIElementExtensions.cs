namespace XLabs.Platform.WP8.Extensions
{
	using System.IO;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media.Imaging;

	public static class UIElementExtensions
    {
        public static Task StreamToJpeg(this UIElement view, Stream stream)
        {
            return Task.Run(() => view.ToBitmap().SaveJpeg(stream, (int)view.RenderSize.Width, (int)view.RenderSize.Height, 0, 100));
        }

        public static WriteableBitmap ToBitmap(this UIElement view)
        {
            return new WriteableBitmap(view, null);
        }
    }
}
