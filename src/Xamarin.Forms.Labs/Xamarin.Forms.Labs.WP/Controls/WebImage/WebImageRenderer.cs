using System;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Microsoft.Phone.Controls;
using Xamarin.Forms.Labs.WP8.Controls;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace Xamarin.Forms.Labs.Controls
{
    public class WebImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            var webImage = (WebImage)Element;

            SetNativeControl(GetImageFromWeb(webImage.ImageUrl));
        }

        private System.Windows.Controls.Image GetImageFromWeb(string url)
        {
            var image = new System.Windows.Controls.Image();

            var uri = new Uri(url, UriKind.Absolute);

            image.Source = new BitmapImage(uri);

            return image;
        }
    }
}