using Xamarin.Forms.Labs.Enums;

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// Creates a button with text and an image.
    /// The image can be on the left, above, on the right or below the text.
    /// </summary>
    public class ImageButton : Button
    {
        /// <summary>
        /// Backing field for the Image property.
        /// </summary>
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create<ImageButton, string>(
                p => p.Image, default(string));

        /// <summary>
        /// Gets or sets the name of the image without path or file type information.
        /// Android: There should be a drawable resource with the same name
        /// iOS: There should be an image in the Resources folder with a build action of BundleResource.
        /// Windows Phone: There should be an image in the Images folder with a type of '.png' and build action set to resource.
        /// </summary>
        /// <value>
        /// The Orientation property gets/sets the value of the backing field, ImageProperty.
        /// </value> 
        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        /// <summary>
        /// Backing field for the orientation property.
        /// </summary>
        public static readonly BindableProperty OrientationProperty =
            BindableProperty.Create<ImageButton, ImageOrientation>(
                p => p.Orientation, ImageOrientation.ImageToLeft);

        /// <summary>
        /// Gets or sets The orientation of the image relative to the text.
        /// </summary> 
        /// <value>
        /// The Orientation property gets/sets the value of the backing field, OrientationProperty.
        /// </value> 
        public ImageOrientation Orientation
        {
            get { return (ImageOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Backing field for the image height property.
        /// </summary>
        public static readonly BindableProperty ImageHeightRequestProperty =
            BindableProperty.Create<ImageButton, int>(
                p => p.ImageHeightRequest, default(int));

        /// <summary>
        /// Gets or sets the requested height of the image.  If less than or equal to zero than a 
        /// height of 50 will be used.
        /// </summary>
        /// <value>
        /// The ImageHeightRequest property gets/sets the value of the backing field, ImageHeightRequestProperty.
        /// </value> 
        public int ImageHeightRequest
        {
            get { return (int)GetValue(ImageHeightRequestProperty); }
            set { SetValue(ImageHeightRequestProperty, value); }
        }

        /// <summary>
        /// Backing field for the image width property.
        /// </summary>
        public static readonly BindableProperty ImageWidthRequestProperty =
           BindableProperty.Create<ImageButton, int>(
               p => p.ImageWidthRequest, default(int));

        /// <summary>
        /// Gets or sets the requested width of the image.  If less than or equal to zero than a 
        /// width of 50 will be used.
        /// </summary>
        /// <value>
        /// The ImageHeightRequest property gets/sets the value of the backing field, ImageHeightRequestProperty.
        /// </value> 
        public int ImageWidthRequest
        {
            get { return (int)GetValue(ImageWidthRequestProperty); }
            set { SetValue(ImageWidthRequestProperty, value); }
        }
    }
}