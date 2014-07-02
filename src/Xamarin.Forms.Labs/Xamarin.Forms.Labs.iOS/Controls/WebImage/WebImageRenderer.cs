using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls.WebImage;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls.WebImage
{
    public class WebImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            var webImage = (Xamarin.Forms.Labs.Controls.WebImage)this.Element;

            var image = GetImageFromWeb(webImage.ImageUrl);

            SetNativeControl(new UIImageView(image));
        }


        private UIImage GetImageFromWeb(string url)
        {
            using (var webclient = new WebClient())
            {
                var imageBytes = webclient.DownloadData(url);

                return UIImage.LoadFromData(NSData.FromArray(imageBytes));
            }
        }
    }
}