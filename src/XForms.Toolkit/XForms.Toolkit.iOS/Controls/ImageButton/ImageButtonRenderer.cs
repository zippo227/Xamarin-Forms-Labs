using System.Collections;
using System.Drawing;
using System.Security.Cryptography;
using MonoTouch.UIKit;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Enums;
using XForms.Toolkit.iOS.Controls.ImageButton;


/// WIP - Not yet ready for use.
[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace XForms.Toolkit.iOS.Controls.ImageButton
{
    public class ImageButtonRenderer : ButtonRenderer
    {
        private const int padding = 10;

        protected override void OnModelSet(VisualElement model)
        {
            base.OnModelSet(model);
            var imageButton = model as Toolkit.Controls.ImageButton;
            var targetButton = Control as UIButton;
            if (imageButton != null && targetButton != null && !String.IsNullOrEmpty(imageButton.Image))
            {
                SetImage(imageButton.Image, imageButton.ImageWidthRequest, imageButton.ImageHeightRequest, targetButton);
                var label = new UILabel(new RectangleF(0, 0, 100, 30));
                label.Text = imageButton.Text;
                targetButton.SetTitle("", UIControlState.Normal);
                targetButton.AddSubview(label);
                //targetButton.TitleLabel.TextAlignment = UITextAlignment.Center;


                //switch (imageButton.Orientation)
                //{
                //    case ImageOrientation.ImageToLeft:
                //        AlignToLeft(imageButton.ImageWidthRequest, targetButton);
                //        break;
                //    case ImageOrientation.ImageToRight:
                //        AlignToRight(imageButton.ImageWidthRequest, targetButton);
                //        break;
                //    case ImageOrientation.ImageOnTop:
                //        AlignToTop(imageButton.ImageHeightRequest, targetButton);
                //        break;
                //}
                
                targetButton.SizeToFit();
                //targetButton.ImageEdgeInsets = new UIEdgeInsets(10, 10, 10, ((int)page.WidthRequest - page.ImageWidthRequest));
                //targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
                //targetButton.ImageRectForContentRect(new RectangleF(0, 10, 100, 100));
                //targetButton.TitleRectForContentRect(new RectangleF(20, 10, 50, 20));
            }
        }

        private void AlignToLeft(int widthRequest, UIButton targetButton)
        {
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;

            var titleInsets = new UIEdgeInsets(0, (widthRequest + padding), 0, -1 * (widthRequest + padding));
            var contentInsets = new UIEdgeInsets(0, 15, 0, 15);
            //var imageInsets = new UIEdgeInsets(0, widthRequest, 0, -1 * (targetButton.Bounds.Width - page.ImageWidthRequest));
            //targetButton.ImageEdgeInsets = imageInsets;
            contentInsets.Right += titleInsets.Left - titleInsets.Right;

            targetButton.TitleEdgeInsets = titleInsets;
            targetButton.ContentEdgeInsets = contentInsets;
            targetButton.SizeToFit();
        }

        private void AlignToRight(int widthRequest, UIButton targetButton)
        {
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;

            var titleInsets = new UIEdgeInsets(0, -1 + (2 * widthRequest + padding), 0, (2 * widthRequest + padding));
            var contentInsets = new UIEdgeInsets(0, 15, 0, 15);
            //var imageInsets = new UIEdgeInsets(0, widthRequest, 0, -1 * (targetButton.Bounds.Width - page.ImageWidthRequest));
            //targetButton.ImageEdgeInsets = imageInsets;
            contentInsets.Left += titleInsets.Right - titleInsets.Left;

            targetButton.TitleEdgeInsets = titleInsets;
            targetButton.ContentEdgeInsets = contentInsets;
            targetButton.SizeToFit();
        }

        private void AlignToTop(int heightRequest, UIButton targetButton)
        {
            targetButton.VerticalAlignment = UIControlContentVerticalAlignment.Top;

            var titleInsets = new UIEdgeInsets(heightRequest + padding, 0, -1 * (heightRequest + padding), 0);
            var contentInsets = new UIEdgeInsets(15, 0, 15, 0);
            //var imageInsets = new UIEdgeInsets(0, widthRequest, 0, -1 * (targetButton.Bounds.Width - page.ImageWidthRequest));
            //targetButton.ImageEdgeInsets = imageInsets;
            contentInsets.Bottom += titleInsets.Top - titleInsets.Bottom;
            targetButton.TitleEdgeInsets = titleInsets;
            targetButton.ContentEdgeInsets = contentInsets;
            targetButton.Frame = new RectangleF(0, 0, 10, 10);
            //targetButton.SizeToFit();
        }

        private void SetImage(string imageName, int widthRequest, int heightRequest, UIButton targetButton)
        {
            var image = UIImage.FromBundle(imageName);

            UIGraphics.BeginImageContext(new SizeF(widthRequest, heightRequest));
            image.Draw(new RectangleF(0, 0, widthRequest, heightRequest));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            var resizableImage =
                resultImage.CreateResizableImage(new UIEdgeInsets(0, 0, widthRequest, heightRequest));

            var imageView = new UIImageView(resizableImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal));
            //targetButton.SetImage(resizableImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal),
            //    UIControlState.Normal);
            targetButton.AddSubview(imageView);
        }
    }
}