namespace XLabs.Forms.Controls
{
	using System.Drawing;

	using MonoTouch.UIKit;

	/// <summary>
	/// Class NoCaretField.
	/// </summary>
	public class NoCaretField : UITextField
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NoCaretField"/> class.
		/// </summary>
		public NoCaretField() : base(default(RectangleF))
		{
		}
		/// <summary>
		/// Gets the caret rect for position.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <returns>RectangleF.</returns>
		public override RectangleF GetCaretRectForPosition(UITextPosition position)
		{
			return default(RectangleF);
		}
	}
}

