using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Platform.Android;

namespace Xamarin.Forms.Labs.Services
{
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
