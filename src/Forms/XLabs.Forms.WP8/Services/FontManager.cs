namespace XLabs.Forms.Services
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using Platform.Device;
    using Xamarin.Forms;
    using Application = System.Windows.Application;

    /// <summary>
    /// Class FontManager.
    /// </summary>
    public partial class FontManager : IFontManager
    {
        /// <summary>
        /// The _display
        /// </summary>
        private readonly IDisplay display;

        private readonly Lazy<string[]> fonts = new Lazy<string[]>(() => new []
        {
            "Arial", "Georgia Italic", "Tahoma", "Arial Black", "Lucida Sans Unicode", "Tahoma Bold", "Arial Bold",
            "Malgun Gothic", "Times New Roman", "Arial Italic", "Meiryo UI", "Times New Roman Bold", "Calibri", 
            "Microsoft YaHei", "Times New Roman Italic", "Calibri Bold", "Segoe UI", "Trebuchet MS", "Calibri Italic", 
            "Segoe UI Bold", "Trebuchet MS Bold", "Comic Sans MS", "Segoe WP", "Trebuchet MS Italic", "Comic Sans MS Bold", 
            "Segoe WP Black", "Verdana", "Courier New", "Segoe WP Bold", "Verdana Bold", "Courier New Bold", "Segoe WP Light", 
            "Verdana Italic", "Courier New Italic", "Segoe WP Semibold", "Webdings", "Georgia", "Segoe WP SemiLight", "Wingdings"
        }); 

        /// <summary>
        /// Initializes a new instance of the <see cref="FontManager"/> class.
        /// </summary>
        /// <param name="display">The display.</param>
        public FontManager(IDisplay display)
        {
            this.display = display;
        }

        #region IFontManager Members

        /// <summary>
        /// Gets all available system fonts.
        /// </summary>
        /// <value>The available fonts.</value>
        public IEnumerable<string> AvailableFonts
        {
            get
            {
                // temporary solution until WP8.1 is supported by Forms so we can use
                // https://msdn.microsoft.com/en-us/library/dd756582(v=vs.85).aspx
                return this.fonts.Value;
                //throw new NotImplementedException(); 
            }
        }

        /// <summary>
        /// Gets height for the font.
        /// </summary>
        /// <param name="font">Font whose height is calculated.</param>
        /// <returns>Height of the font in inches.</returns>
        public double GetHeight(Font font)
        {
            var multiplier = Application.Current.Host.Content.ScaleFactor / 100d;
            return multiplier * font.GetHeight() / this.display.Ydpi;
        }

        #endregion


    }
}
