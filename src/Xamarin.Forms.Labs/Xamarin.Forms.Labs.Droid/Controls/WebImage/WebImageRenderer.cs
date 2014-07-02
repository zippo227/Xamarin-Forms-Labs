using Android.Graphics;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Droid.Controls.WebImage;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebImage), typeof(WebImageRenderer))]
namespace Xamarin.Forms.Labs.Droid.Controls.WebImage
{
    public class WebImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);
            var webImage = (Xamarin.Forms.Labs.Controls.WebImage)this.Element;

            var targetImageView = this.Control;

            var image = GetImageFromWeb(webImage.ImageUrl);

            targetImageView.SetImageBitmap(image);

            //SetNativeControl(new ImageView());
        }

        private Bitmap GetImageFromWeb(string url)
        {
            Bitmap imageBitmap = null;
            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = Android.Graphics.BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }
    }
}