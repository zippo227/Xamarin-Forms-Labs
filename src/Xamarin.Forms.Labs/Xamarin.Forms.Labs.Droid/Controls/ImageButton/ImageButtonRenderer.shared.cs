#if __Android__
using Xamarin.Forms.Platform.Android;

namespace Xamarin.Forms.Labs.Droid.Controls.ImageButton
#elif __IOS__
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.Forms.Labs.iOS.Controls.ImageButton
#elif WINDOWS_PHONE
using Xamarin.Forms.Platform.WinPhone;

namespace Xamarin.Forms.Labs.WP8.Controls.ImageButton
#endif
{
    /// <summary>
    /// Draws a button on the Android platform with the image shown in the right 
    /// position with the right size.
    /// </summary>
    public partial class ImageButtonRenderer
    {
        /// <summary>
        /// Returns the proper <see cref="IImageSourceHandler"/> based on the type of <see cref="ImageSource"/> provided.
        /// </summary>
        /// <param name="source">The <see cref="ImageSource"/> to get the handler for.</param>
        /// <returns>The needed handler.</returns>
        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
            {
                returnValue = new ImageLoaderSourceHandler();
            }
            else if (source is FileImageSource)
            {
                returnValue = new FileImageSourceHandler();
            }
            else if (source is StreamImageSource)
            {
                returnValue = new StreamImagesourceHandler();
            }
            return returnValue;
        }
    }
}