using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Phone.SecureElement;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Enums;
using XForms.Toolkit.WP.Controls.ImageButton;
using Image = System.Windows.Controls.Image;
using TextAlignment = System.Windows.TextAlignment;

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace XForms.Toolkit.WP.Controls.ImageButton
{
    /// <summary>
    /// Draws a button on the Windows Phone platform with the image shown in the right 
    /// position with the right size.
    /// </summary>
    public class ImageButtonRenderer : ButtonRenderer
    {
        private Image CurrentImage;
        /// <summary>
        /// Handles the initial drawing of the button.
        /// </summary>
        /// <param name="e">Information on the <see cref="ImageButton"/>.</param> 
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            var sourceButton = this.Element as Toolkit.Controls.ImageButton;
            var targetButton = this.Control;
            if (sourceButton != null && targetButton != null && !String.IsNullOrEmpty(sourceButton.Image))
            {
                var stackPanel = new StackPanel();
                stackPanel.Orientation = (sourceButton.Orientation == ImageOrientation.ImageOnTop ||
                    sourceButton.Orientation == ImageOrientation.ImageOnBottom) ?
                    Orientation.Vertical : Orientation.Horizontal;

                CurrentImage = GetImage(sourceButton.Image, sourceButton.ImageHeightRequest, sourceButton.ImageWidthRequest);
                SetImageMargin(CurrentImage, sourceButton.Orientation);

                var label = new TextBlock();
                label.TextAlignment = GetTextAlignment(sourceButton.Orientation);
                label.FontSize = 16;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.Text = sourceButton.Text;

                if (sourceButton.Orientation == ImageOrientation.ImageToLeft)
                {
                    targetButton.HorizontalContentAlignment = HorizontalAlignment.Left;
                }
                else if (sourceButton.Orientation == ImageOrientation.ImageToRight)
                {
                    targetButton.HorizontalContentAlignment = HorizontalAlignment.Right;
                }
                
                if (sourceButton.Orientation == ImageOrientation.ImageOnTop ||
                    sourceButton.Orientation == ImageOrientation.ImageToLeft)
                {
                    CurrentImage.HorizontalAlignment = HorizontalAlignment.Left;
                    stackPanel.Children.Add(CurrentImage);
                    stackPanel.Children.Add(label);
                }
                else
                {
                    stackPanel.Children.Add(label);
                    stackPanel.Children.Add(CurrentImage);
                }

                targetButton.Content = stackPanel;
            }

        }

        /// <summary>
        /// Returns the alignment of the label on the button depending on the orientation.
        /// </summary>
        /// <param name="imageOrientation">The orientation to use.</param>
        /// <returns>The alignment to use for the text.</returns>
        private TextAlignment GetTextAlignment(ImageOrientation imageOrientation)
        {
            TextAlignment returnValue;
            switch (imageOrientation)
            {
                case ImageOrientation.ImageToLeft:
                    returnValue = TextAlignment.Left;
                    break;
                case ImageOrientation.ImageToRight:
                    returnValue = TextAlignment.Right;
                    break;
                default:
                    returnValue = TextAlignment.Center;
                    break;
            }
            return returnValue;
        }

        /// <summary>
        /// Called when the underlying model's properties are changed
        /// </summary>
        /// <param name="sender">Model</param>
        /// <param name="e">Event arguments</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Toolkit.Controls.ImageButton.ImageProperty.PropertyName)
            {
                var sourceButton = this.Element as Toolkit.Controls.ImageButton;
                if (sourceButton != null && !String.IsNullOrEmpty(sourceButton.Image))
                {
                    CurrentImage = GetImage(sourceButton.Image, sourceButton.ImageHeightRequest, sourceButton.ImageWidthRequest);
                    SetImageMargin(CurrentImage, sourceButton.Orientation);
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="Image"/> of type .png that is a resource in the Windows Phone project
        /// and stored in the images folder.
        /// </summary>
        /// <param name="imageName">The name of the image to return.  Should be the resource name without the .png extension.</param>
        /// <param name="height">The height for the image (divides by 2 for the Windows Phone platform).</param>
        /// <param name="width">The width for the image (divides by 2 for the Windows Phone platform).</param>
        /// <returns></returns>
        private Image GetImage(string imageName, int height, int width)
        {
            var image = new Image();
            var uri = new Uri("images/" + imageName + ".png", UriKind.Relative);
            var bmp = new BitmapImage(uri);
            image.Source = bmp;
            image.Height = height / 2;
            image.Width = width / 2;
            return image;
        }

        /// <summary>
        /// Sets a margin of 10 between the image and the text.
        /// </summary>
        /// <param name="image">The image to add a margin to.</param>
        /// <param name="orientation">The orientation of the image on the button.</param>
        private void SetImageMargin(Image image, ImageOrientation orientation)
        {
            const int defaultMargin = 10;
            int left = 0;
            int top = 0;
            int right = 0;
            int bottom = 0;

            switch (orientation)
            {
                case ImageOrientation.ImageToLeft:
                    right = defaultMargin;
                    break;
                case ImageOrientation.ImageOnTop:
                    bottom = defaultMargin;
                    break;
                case ImageOrientation.ImageToRight:
                    left = defaultMargin;
                    break;
                case ImageOrientation.ImageOnBottom:
                    top = defaultMargin;
                    break;
            }

            image.Margin = new System.Windows.Thickness(left, top, right, bottom);
        }
    }
}
