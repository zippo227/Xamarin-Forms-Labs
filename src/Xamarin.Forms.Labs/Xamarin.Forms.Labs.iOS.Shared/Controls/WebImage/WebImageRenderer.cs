#if __UNIFIED__
using Foundation;
using UIKit;
#elif __IOS__
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls.WebImage;
using Xamarin.Forms.Labs.iOS.Services;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls.WebImage
{
    public class WebImageRenderer : ImageRenderer
    {

        /// <summary>
        /// Gets the underlying control typed as an <see cref="WebImage"/>
        /// </summary>
        private Labs.Controls.WebImage WebImage
        {
            get { return (Labs.Controls.WebImage)Element; }
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            UIImage image;
            var networkStatus = Reachability.InternetConnectionStatus();

            var isReachable = networkStatus != NetworkStatus.NotReachable;

            if (isReachable)
            {
                image = GetImageFromWeb(WebImage.ImageUrl);
            }
            else
            {
                image = string.IsNullOrEmpty(WebImage.DefaultImage)
                    ? new UIImage()
                    : UIImage.FromBundle(WebImage.DefaultImage);
            }

            var imageView = new UIImageView(image);

            SetNativeControl(imageView);
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