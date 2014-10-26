using System;
using System.ComponentModel;
using System.Drawing;
using MonoTouch.UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls.CircleImage;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls.CircleImage
{
    public class CircleImageRenderer : ImageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.OldElement != null || Element == null)
                return;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Control == null) return;

            if (e.PropertyName == Image.IsLoadingProperty.PropertyName
                && !this.Element.IsLoading && this.Control.Image != null)
            {
                //Should only be true right after an image is loaded
                int height = 0;
                int width = 0;
                int top = 0;
                int left = 0;

                switch (this.Element.Aspect)
                {
                    case Aspect.AspectFill:
                        height = (int)this.Control.Image.Size.Height;
                        width = (int)this.Control.Image.Size.Width;
                        height = this.MakeSquare(height, ref width);
                        left = (((int)this.Control.Image.Size.Width - width) / 2);
                        top = (((int)this.Control.Image.Size.Height - height) / 2);
                        break;
                    case Aspect.Fill:
                        height = (int)this.Control.Image.Size.Height;
                        width = (int)this.Control.Image.Size.Width;
                        break;
                    case Aspect.AspectFit:
                        height = (int)this.Control.Image.Size.Height;
                        width = (int)this.Control.Image.Size.Width;
                        height = this.MakeSquare(height, ref width);
                        left = (((int)this.Control.Image.Size.Width - width) / 2);
                        top = (((int)this.Control.Image.Size.Height - height) / 2);
                        break;
                }

                UIImage image = this.Control.Image;
                var clipRect = new RectangleF(0, 0, width, height);
                var scaled = image.Scale(new SizeF(width, height));
                UIGraphics.BeginImageContextWithOptions(new SizeF(width, height), false, 0f);
                UIBezierPath.FromRoundedRect(clipRect, Math.Max(width, height) / 2).AddClip();

                scaled.Draw(new RectangleF(0, 0, scaled.Size.Width, scaled.Size.Height));
                UIImage final = UIGraphics.GetImageFromCurrentImageContext();
                UIGraphics.EndImageContext();
                this.Control.Image = final;
            }
        }

        private int MakeSquare(int height, ref int width)
        {
            if (height < width)
            {
                width = height;
            }
            else
            {
                height = width;
            }
            return height;
        }
    }
}