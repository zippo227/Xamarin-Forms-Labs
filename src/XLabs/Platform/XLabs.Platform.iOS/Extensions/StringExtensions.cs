namespace XLabs.Platform.Extensions
{
	using System.Drawing;

	using MonoTouch.Foundation;
	using MonoTouch.UIKit;

	/// <summary>
	/// Class StringExtensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Strings the height.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="width">The width.</param>
		/// <returns>System.Single.</returns>
		public static float StringHeight(this string text, UIFont font, float width)
		{
			var nativeString = new NSString(text);

			var rect = nativeString.GetBoundingRect(
				new SizeF(width, float.MaxValue),
				NSStringDrawingOptions.UsesLineFragmentOrigin,
				new UIStringAttributes { Font = font },
				null);

			return rect.Height;
		}
	}
}