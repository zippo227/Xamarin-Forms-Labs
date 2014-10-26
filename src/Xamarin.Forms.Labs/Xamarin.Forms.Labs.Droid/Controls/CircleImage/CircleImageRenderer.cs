using Android.Graphics;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Droid.Controls.CircleImage;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CircleImage), typeof(CircleImageRenderer))]
namespace Xamarin.Forms.Labs.Droid.Controls.CircleImage
{
    public class CircleImageRenderer : ImageRenderer
    {
        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Image.IsLoadingProperty.PropertyName && !this.Element.IsLoading
                && this.Control.Drawable != null)
            {
                //Should only be true right after an image is loaded
                using (var sourceBitmap = Bitmap.CreateBitmap(this.Control.Drawable.IntrinsicWidth, this.Control.Drawable.IntrinsicHeight, Bitmap.Config.Argb8888))
                {
                    Canvas canvas = new Canvas(sourceBitmap);
                    this.Control.Drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                    this.Control.Drawable.Draw(canvas);
                    this.ReshapeImage(sourceBitmap);                        
                }
            }
        }

        private void ReshapeImage(Bitmap sourceBitmap)
        {
            //May need some scaling code
            if (sourceBitmap != null)
            {
                var sourceRect = GetScaledRect(sourceBitmap);
                var rect = this.GetTargetRect(sourceBitmap);
                using (var output = Bitmap.CreateBitmap(rect.Width(), rect.Height(), Bitmap.Config.Argb8888))
                {
                    var canvas = new Canvas(output);

                    var paint = new Paint();
                    var rectF = new RectF(rect);
                    var roundRx = rect.Width() / 2;
                    var roundRy = rect.Height() / 2;

                    paint.AntiAlias = true;
                    canvas.DrawARGB(0, 0, 0, 0);
                    paint.Color = Android.Graphics.Color.ParseColor("#ff424242");
                    canvas.DrawRoundRect(rectF, roundRx, roundRy, paint);

                    paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.SrcIn));
                    canvas.DrawBitmap(sourceBitmap, sourceRect, rect, paint);

                    this.Control.SetImageBitmap(output);
                    // Forces the internal method of InvalidateMeasure to be called.
                    this.Element.WidthRequest = this.Element.WidthRequest;
                }
            }
        }

        private Rect GetScaledRect(Bitmap sourceBitmap)
        {
            int height = 0;
            int width = 0;
            int top = 0;
            int left = 0;

            switch (this.Element.Aspect)
            {
                case Aspect.AspectFill:
                    height = sourceBitmap.Height;
                    width = sourceBitmap.Width;
                    height = this.MakeSquare(height, ref width);
                    left = (int)((sourceBitmap.Width - width) / 2);
                    top = (int)((sourceBitmap.Height - height) / 2);
                    break;
                case Aspect.Fill:
                    height = sourceBitmap.Height;
                    width = sourceBitmap.Width;
                    break;
                default:
                    height = sourceBitmap.Height;
                    width = sourceBitmap.Width;
                    height = this.MakeSquare(height, ref width);
                    left = (int)((sourceBitmap.Width - width) / 2);
                    top = (int)((sourceBitmap.Height - height) / 2);
                    break;
            }

            var rect = new Rect(left, top, width + left, height + top);

            return rect;
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

        private Rect GetTargetRect(Bitmap sourceBitmap)
        {
            int height = 0;
            int width = 0;

            height = this.Element.HeightRequest > 0
                       ? (int)System.Math.Round(this.Element.HeightRequest, 0)
                       : sourceBitmap.Height; 
            width = this.Element.WidthRequest > 0
                       ? (int)System.Math.Round(this.Element.WidthRequest, 0)
                       : sourceBitmap.Width; 

            // Make Square
            if (height < width)
            {
                width = height;
            }
            else
            {
                height = width;
            }

            var rect = new Rect(0, 0, width, height);

            return rect;
        }
    }
}
