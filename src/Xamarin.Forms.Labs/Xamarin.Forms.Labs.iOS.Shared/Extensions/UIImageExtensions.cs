using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
#if __UNIFIED__
using CoreGraphics;
using Foundation;
using UIKit;
#elif __IOS__
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
#endif
using Xamarin.Forms.Labs.Services;
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.Forms.Labs.iOS
{
    public static class UIImageExtensions
    {
        public static async Task<ImageSource> AddText(this StreamImageSource source, string text, PointF point, Font font, Color color)
        {
            var token = new CancellationTokenSource();
            var stream = await source.Stream(token.Token);
            var image = UIImage.LoadFromData(NSData.FromStream(stream));

            var bytes = image.AddText(text, point, font.ToUIFont(), color.ToUIColor()).AsPNG().ToArray();

            return ImageSource.FromStream(new Func<Stream>(() => new MemoryStream(bytes)));
        }

        public static UIImage AddText(this UIImage image, string text, PointF point, UIFont font, UIColor color, UITextAlignment alignment = UITextAlignment.Left)
        {
            //var labelRect = new RectangleF(point, new SizeF(image.Size.Width - point.X, image.Size.Height - point.Y));
            var h = text.StringHeight(font, (float)image.Size.Width);
            var labelRect = new RectangleF(point, new SizeF((float)image.Size.Width - point.X, h));

            var label = new UILabel(labelRect)
            { 
                Font = font, 
                Text = text,
                TextColor = color,
                TextAlignment = alignment,
                BackgroundColor = UIColor.Clear
            };

            var labelImage = label.ToNativeImage();
            

            using (var context = image.Size.ToBitmapContext())
            {
#if __UNIFIED__
                var rect = new RectangleF(new PointF(0, 0), image.Size.ToSizeF());
#elif __IOS__
                var rect = new RectangleF(new PointF(0, 0), image.Size);
#endif
                context.DrawImage(rect, image.CGImage);
                context.DrawImage(labelRect, labelImage.CGImage);
                context.StrokePath();
                return UIImage.FromImage(context.ToImage());
            }
        }
    }
}