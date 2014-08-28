using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.iOS;

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