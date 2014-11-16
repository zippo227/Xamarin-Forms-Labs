namespace XLabs.Platform.Droid.Extensions
{
	using System.IO;
	using System.Threading.Tasks;

	using Android.Graphics;

	public static class ViewExtensions
    {
        public static Android.Graphics.Bitmap ToBitmap(this Android.Views.View view)
        {
            var bitmap = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888);
            using (var c = new Canvas(bitmap))
            {
                view.Draw(c);
            }

            return bitmap;
        }

        public static async Task StreamToPng(this Android.Views.View view, Stream stream)
        {
            await view.ToBitmap().CompressAsync(Bitmap.CompressFormat.Png, 100, stream);
        }
    }
}