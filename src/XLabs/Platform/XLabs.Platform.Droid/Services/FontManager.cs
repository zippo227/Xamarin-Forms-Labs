namespace XLabs.Platform.Droid.Services
{
	using System;
	using System.Collections.Generic;

	using XLabs.Platform.Device;
	using XLabs.Platform.Droid.Device;
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
            get { throw new NotImplementedException(); }
        }

        public double GetHeight(Font font)
        {
            var scaled = font.ToScaledPixel();
            return scaled *  Display.Metrics.Density / display.Ydpi;
        }

        #endregion
    }
}
