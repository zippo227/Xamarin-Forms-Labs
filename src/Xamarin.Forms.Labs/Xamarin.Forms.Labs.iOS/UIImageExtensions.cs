using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.Forms.Labs.iOS
{
    public static class UIImageExtensions
    {
        public static async Task<ImageSource> AddText(this StreamImageSource source, string text, PointF point, Font font)
        {
            var token = new CancellationTokenSource();
            var stream = await source.Stream(token.Token);
            var image = UIImage.LoadFromData(NSData.FromStream(stream));

            var bytes = image.AddText(text, point, font.ToUIFont()).AsPNG().ToArray();

            return ImageSource.FromStream(new Func<Stream>(() => new MemoryStream(bytes)));
        }

        public static UIImage AddText(this UIImage image, string text, PointF point, UIFont font)
        {
            //var cx = image.Size.ToBitmapContext();

            //var imageView = new UIImageView(image);
            //imageView.DrawString(text, point, font);
            //return imageView.ToNativeImage();

            var label = new UILabel(new RectangleF(point, new SizeF(image.Size.Width - point.X, image.Size.Height - point.Y)))
            { 
                Font = font, 
                Text = text, 
                TextColor = UIColor.Red 
            };

            var labelImage = label.ToNativeImage();
            

            using (var context = image.Size.ToBitmapContext())
            {
                //context.TranslateCTM(0, 0);

                var rect = new RectangleF(new PointF(0, 0), image.Size);
                context.DrawImage(rect, image.CGImage);

                context.DrawImage(label.Bounds, labelImage.CGImage);
                //context.SetRGBFillColor(.5f, .5f, .5f, 1);
                //context.SetFont(CGFont.CreateWithFontName(font.Name));
                //context.SetFontSize(font.PointSize);

                //var ns = new NSString(text);

                //context.SetTextDrawingMode(CGTextDrawingMode.Fill);
                //context.ShowTextAtPoint(point.X, point.Y, text);

                context.StrokePath();

                return UIImage.FromImage(context.ToImage());
            }
        }
    }
}