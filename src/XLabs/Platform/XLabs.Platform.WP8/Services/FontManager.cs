namespace XLabs.Platform.WP8.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Media;

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
                return Fonts.SystemTypefaces.Select(a =>
                    {
                        GlyphTypeface typeFace = null;
                        a.TryGetGlyphTypeface(out typeFace);
                        return typeFace;
                    }).Where(a => a != null).Select(a => a.FontFileName);
                //throw new NotImplementedException(); 
            }
        }

        public double GetHeight(Font font)
        {
            double multiplier = (double)System.Windows.Application.Current.Host.Content.ScaleFactor / 100d;
            return multiplier * font.GetHeight() / this.display.Ydpi;
        }

        #endregion


    }
}
