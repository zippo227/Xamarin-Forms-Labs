using MonoTouch.UIKit;
using System;
using System.Drawing;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Enums;
using XForms.Toolkit.iOS.Controls.ImageButton;

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace XForms.Toolkit.iOS.Controls.ImageButton
{
    public class ImageButtonRenderer : ButtonRenderer
    {
        private const int controlPadding = 2;
		private  Toolkit.Controls.ImageButton ImageButton { get { return (XForms.Toolkit.Controls.ImageButton) Element; } }
        private const string iPad = "iPad";
        private const string iPhone = "iPhone";

		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);
			var imageButton = this.ImageButton;
			var targetButton = Control as UIButton;
			if (imageButton != null && targetButton != null && !String.IsNullOrEmpty(imageButton.Image))
			{
				SetImage(imageButton.Image, imageButton.ImageWidthRequest, imageButton.ImageHeightRequest, targetButton);

				switch (imageButton.Orientation)
				{
				case ImageOrientation.ImageToLeft:
					AlignToLeft(targetButton);
					break;
				case ImageOrientation.ImageToRight:
					AlignToRight(imageButton.ImageWidthRequest, targetButton);
					break;
				case ImageOrientation.ImageOnTop:
                    AlignToTop(imageButton.ImageHeightRequest, imageButton.ImageWidthRequest, targetButton);
					break;
				case ImageOrientation.ImageOnBottom:
                    AlignToBottom(imageButton.ImageHeightRequest, imageButton.ImageWidthRequest, targetButton);
					break;
				}                
			}
		}

        private void AlignToLeft(UIButton targetButton)
        {
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Left;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Left;

            var titleInsets = new UIEdgeInsets(0, controlPadding, 0, -1 * (controlPadding));
            targetButton.TitleEdgeInsets = titleInsets;
        }

        private void AlignToRight(int widthRequest, UIButton targetButton)
        {
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Right;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Right;

            var titleInsets = new UIEdgeInsets(0, -1 * (widthRequest + controlPadding), 0, (widthRequest + controlPadding));

            targetButton.TitleEdgeInsets = titleInsets;
            var imageInsets = new UIEdgeInsets(0, widthRequest, 0, -1 * widthRequest);
            targetButton.ImageEdgeInsets = imageInsets;
            //targetButton.SizeToFit();
        }

        private void AlignToTop(int heightRequest, int widthRequest, UIButton targetButton)
        {
            targetButton.VerticalAlignment = UIControlContentVerticalAlignment.Top;
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Center;
            targetButton.TitleLabel.Text = "Microsoft";
            targetButton.SizeToFit();

            var titleWidth = targetButton.TitleLabel.IntrinsicContentSize.Width;

            UIEdgeInsets titleInsets;
            UIEdgeInsets imageInsets;

            if (UIDevice.CurrentDevice.Model.Contains(iPad))
            {
                titleInsets = new UIEdgeInsets(heightRequest, Convert.ToInt32(-1 * widthRequest / 2), -1 * heightRequest, Convert.ToInt32(widthRequest / 2));
                imageInsets = new UIEdgeInsets(0, Convert.ToInt32(titleWidth / 2), 0, -1 * Convert.ToInt32(titleWidth / 2));
            }
            else
            {
                titleInsets = new UIEdgeInsets(heightRequest, Convert.ToInt32(-1 * widthRequest / 2), -1 * heightRequest, Convert.ToInt32(widthRequest / 2));
                imageInsets = new UIEdgeInsets(0, titleWidth / 2, 0, -1 * titleWidth / 2);
            }
            targetButton.TitleEdgeInsets = titleInsets;
            targetButton.ImageEdgeInsets = imageInsets;
        }

        private void AlignToBottom(int heightRequest, int widthRequest, UIButton targetButton)
        {
            targetButton.VerticalAlignment = UIControlContentVerticalAlignment.Bottom;
            targetButton.HorizontalAlignment = UIControlContentHorizontalAlignment.Center;
            targetButton.TitleLabel.TextAlignment = UITextAlignment.Center;
            targetButton.SizeToFit();
            var titleWidth = targetButton.TitleLabel.IntrinsicContentSize.Width;

            UIEdgeInsets titleInsets;
            UIEdgeInsets imageInsets;

            if (UIDevice.CurrentDevice.Model.Contains(iPad))
            {
                titleInsets = new UIEdgeInsets(-1 * heightRequest, Convert.ToInt32(-1 * widthRequest / 2), heightRequest,
                    Convert.ToInt32(widthRequest / 2));
                imageInsets = new UIEdgeInsets(0, titleWidth / 2, 0, -1 * titleWidth / 2);
            }
            else
            {
                titleInsets = new UIEdgeInsets(-1 * heightRequest, -1 * widthRequest, heightRequest, titleWidth);
                imageInsets = new UIEdgeInsets(0, 0, 0, 0);                
            }
            targetButton.TitleEdgeInsets = titleInsets;
            targetButton.ImageEdgeInsets = imageInsets;
        }

        private void SetImage(string imageName, int widthRequest, int heightRequest, UIButton targetButton)
        {
            var image = UIImage.FromBundle(imageName);

            UIGraphics.BeginImageContext(new SizeF(widthRequest, heightRequest));
            image.Draw(new RectangleF(0, 0, widthRequest, heightRequest));
            var resultImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            var resizableImage = resultImage.CreateResizableImage(new UIEdgeInsets(0, 0, widthRequest, heightRequest));

            targetButton.SetImage(resizableImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal),
                UIControlState.Normal);
        }
    }
}