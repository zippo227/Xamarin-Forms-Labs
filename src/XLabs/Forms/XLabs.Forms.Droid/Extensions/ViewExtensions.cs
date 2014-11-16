namespace XLabs.Forms.Extensions
{
	using System.IO;
	using System.Threading.Tasks;

	using Android.Graphics;

	/// <summary>
	/// Class ViewExtensions.
	/// </summary>
	public static class ViewExtensions
	{
		/// <summary>
		/// To the bitmap.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <returns>Android.Graphics.Bitmap.</returns>
		public static Android.Graphics.Bitmap ToBitmap(this Android.Views.View view)
		{
			var bitmap = Bitmap.CreateBitmap(view.Width, view.Height, Bitmap.Config.Argb8888);
			using (var c = new Canvas(bitmap))
			{
				view.Draw(c);
			}

			return bitmap;
		}

		/// <summary>
		/// Streams to PNG.
		/// </summary>
		/// <param name="view">The view.</param>
		/// <param name="stream">The stream.</param>
		/// <returns>Task.</returns>
		public static async Task StreamToPng(this Android.Views.View view, Stream stream)
		{
			await view.ToBitmap().CompressAsync(Bitmap.CompressFormat.Png, 100, stream);
		}
	}
}