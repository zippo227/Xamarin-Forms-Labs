namespace XLabs.Sample.Pages.Services
{
    using System;
    using Forms.Services;
    using Xamarin.Forms;

    using XLabs.Platform.Device;

    /// <summary>
    /// Class FontManagerPage.
    /// </summary>
    public class FontManagerPage : ContentPage
    {
        private const double fontSize = 0.25;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontManagerPage"/> class.
        /// </summary>
        /// <param name="display">The display.</param>
        /// <param name="fontManager">The font manager.</param>
        public FontManagerPage(IDisplay display, IFontManager fontManager)
        {
            var stack = new StackLayout();

            foreach (var namedSize in Enum.GetValues(typeof(NamedSize)))
            {
                var font = Font.SystemFontOfSize((NamedSize)namedSize);

                var height = fontManager.GetHeight(font);

                var heightRequest = display.HeightRequestInInches(height);

                var label = new Label()
                {
                    Font = font,
                    HeightRequest = heightRequest + 10,
                    Text = string.Format("System font {0} is {1:0.000}in tall.", namedSize, height),
                    XAlign = TextAlignment.Center
                };

                stack.Children.Add(label);
            }

            var f = Font.SystemFontOfSize(24);

            var inchFont = fontManager.FindClosest(f.FontFamily, fontSize);

            stack.Children.Add(new Label()
            {
                Text = "The below text should be " + fontSize + "in height from its highest point to lowest.",
                XAlign = TextAlignment.Center
            });


            stack.Children.Add(new Label()
            {
                Text = "FfTtLlGgJjPp",
                TextColor = Color.Lime,
                FontSize = inchFont.FontSize,
//                BackgroundColor = Color.Gray,
//                FontFamily = inchFont.FontFamily,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Start
            });


            stack.Children.Add(new Label()
            {
                Text = fontSize + "in height = SystemFontOfSize(" + inchFont.FontSize + ")",
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.End
            });

            this.Content = stack;
        }
    }
}
