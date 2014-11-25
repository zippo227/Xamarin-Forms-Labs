using System;
#if __UNIFIED__
using UIKit;
using Foundation;
#elif __IOS__
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif

namespace Xamarin.Forms.Labs.iOS
{
    public static class StringExtensions
    {
        public static float StringHeight(this string text, UIFont font, float width)
        {
            var nativeString = new NSString(text);

            var rect = nativeString.GetBoundingRect(
                new System.Drawing.SizeF(width, float.MaxValue),
                NSStringDrawingOptions.UsesLineFragmentOrigin,
                new UIStringAttributes() { Font = font },
                null);

            return (float)rect.Height;
        }
    }
}

