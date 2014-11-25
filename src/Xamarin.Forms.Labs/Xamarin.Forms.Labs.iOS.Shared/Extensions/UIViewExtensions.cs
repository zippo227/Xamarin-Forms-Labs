using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
using Xamarin.Forms.Platform.iOS;

namespace Xamarin.Forms.Labs.iOS
{
    public static class UIViewExtensions
    {
        public static async Task StreamToPng(this UIView view, Stream stream)
        {
            var bytes = view.ToNativeImage().AsPNG().ToArray();

            await stream.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Takes an image of the view.
        /// </summary>
        /// <param name="view">View to process to image.</param>
        /// <returns>A native image of type <see cref="UIImage"/>.</returns>
        public static UIImage ToNativeImage(this UIView view)
        {
            UIGraphics.BeginImageContext(view.Bounds.Size);

            view.Layer.RenderInContext(UIGraphics.GetCurrentContext());

            var image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return image;
        }

#if __UNIFIED__
        public static RectangleF ToRectangleF(this CGRect rect)
        {
            return new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }

        public static SizeF ToSizeF(this CGSize size)
        {
            return new SizeF((float)size.Width, (float)size.Height);
        }

        public static PointF ToPointF(this CGPoint point)
        {
            return new PointF((float)point.X, (float)point.Y);
        }
#endif

#if __UNIFIED__
        public static CGBitmapContext ToBitmapContext(this CGSize size)
#elif __IOS__
        public static CGBitmapContext ToBitmapContext(this System.Drawing.SizeF size)
#endif
        {
            using (var colorSpace = CGColorSpace.CreateDeviceRGB())
            {
                var pixelsWide = (int)size.Width;
                var pixelsHigh = (int)size.Height;

                var bitmapBytesPerRow = (pixelsWide * 4);// 1
                var bitmapByteCount = (bitmapBytesPerRow * pixelsHigh);

                var bitmapData = new byte[bitmapByteCount];

                return new CGBitmapContext(
                        bitmapData,
                        pixelsWide,
                        pixelsHigh,
                        8,      // bits per component
                        bitmapBytesPerRow,
                        colorSpace,
                        CGImageAlphaInfo.PremultipliedLast);
            }

        }
    }
}