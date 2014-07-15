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
                Text = "and From code",
                FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "Courier New"),
                IsUnderline = true,
                FontSize = 22,
            };

            stkRoot.Children.Add(label);
        }
    }
}

