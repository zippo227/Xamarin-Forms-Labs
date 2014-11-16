namespace XLabs.Platform.iOS.Extensions
{
	public static class LabelExtensions
    {
        public static void AdjustHeight(this Label label)
        {
            label.HeightRequest = label.Text.StringHeight(label.Font.ToUIFont(), (float)label.Width);
        }
    }
}

