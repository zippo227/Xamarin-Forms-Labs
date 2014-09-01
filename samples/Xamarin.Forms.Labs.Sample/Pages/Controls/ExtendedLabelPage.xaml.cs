using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.Sample
{
    public partial class ExtendedLabelPage : ContentPage
    {
        public ExtendedLabelPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.Main;

            var label = new ExtendedLabel
            {
                Text = "From code, using Device.OnPlatform, Underlined",
                FontName = "Open 24 Display St.ttf",
                FriendlyFontName = Device.OnPlatform<String>("", "", "Open 24 Display St"),
                IsUnderline = true,
                FontSize = 22,
            };

            var label2 = new ExtendedLabel
            {
                Text = "From code, Strikethrough",
                FontName = "Open 24 Display St.ttf",
                FriendlyFontName = Device.OnPlatform<String>("", "", "Open 24 Display St"),
                IsUnderline = false,
                IsStrikeThrough = true,
                FontSize = 22,
            };

            stkRoot.Children.Add(label);
            stkRoot.Children.Add(label2);
        }
    }
}

