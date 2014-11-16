namespace XLabs.Platform.iOS.Services
{
	using System.Collections.Generic;

	using MonoTouch.UIKit;

	using XLabs.Platform.Device;
	using XLabs.Platform.iOS.Extensions;
	using XLabs.Platform.Services;

	public partial class FontManager : IFontManager
    {
        private readonly IDisplay display;

        public FontManager(IDisplay display)
        {
            this.display = display;
        }

        #region IFontManager Members

        public IEnumerable<string> AvailableFonts
        {
            get
            {
                return UIFont.FamilyNames;
            }
        }

        public double GetHeight(Font font)
        {
            var height = (double)string.Empty.StringHeight(font.ToUIFont(), float.MaxValue);

            return height * this.display.ScreenHeightInches() / this.display.Height;
        }

        #endregion
    }
}