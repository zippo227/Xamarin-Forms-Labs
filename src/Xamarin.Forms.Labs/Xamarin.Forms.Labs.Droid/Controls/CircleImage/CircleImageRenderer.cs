using System;
using Android.App.Backup;
using Android.Graphics;
using Android.Nfc.CardEmulators;
using Android.Views;
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
        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {

                if ((int)Android.OS.Build.VERSION.SdkInt < 18)
                    SetLayerType(LayerType.Software, null);
            }
        }

        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == Image.IsLoadingProperty.PropertyName && !this.Element.IsLoading
                && this.Control.Drawable != null)
            {
                //Should only be true right after an image is loaded
                if (this.Element.Aspect != Aspect.AspectFit)
                {
                    using (var sourceBitmap = Bitmap.CreateBitmap(this.Control.Drawable.IntrinsicWidth, this.Control.Drawable.IntrinsicHeight, Bitmap.Config.Argb8888))
                    {
                        Canvas canvas = new Canvas(sourceBitmap);
                        this.Control.Drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
                        this.Control.Drawable.Draw(canvas);
                        this.ReshapeImage(sourceBitmap);
                    }
                    
                }
            }
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            if (this.Element.Aspect == Aspect.AspectFit)
            {
                var radius = Math.Min(Width, Height)/2;
                var strokeWidth = 10;
                radius -= strokeWidth/2;

                Path path = new Path();
                path.AddCircle(Width/2, Height/2, radius, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);

                var result = base.DrawChild(canvas, child, drawingTime);

                path.Dispose();

                return result;

            }

            return base.DrawChild(canvas, child, drawingTime);
        }

        private void ReshapeImage(Bitmap sourceBitmap)
        {
            if (sourceBitmap != null)
            {
                var sourceRect = GetScaledRect(sourceBitmap.Height, sourceBitmap.Width);
                var rect = this.GetTargetRect(sourceBitmap.Height, sourceBitmap.Width);
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

                    //this.DrawBorder(canvas, rect.Width(), rect.Height());

                    this.Control.SetImageBitmap(output);
                    // Forces the internal method of InvalidateMeasure to be called.
                    this.Element.WidthRequest = this.Element.WidthRequest;
                }
            }
        }

        private Rect GetScaledRect(int sourceHeight, int sourceWidth)
        {
            int height = 0;
            int width = 0;
            int top = 0;
            int left = 0;

            switch (this.Element.Aspect)
            {
                case Aspect.AspectFill:
                    height = sourceHeight;
                    width = sourceWidth;
                    height = this.MakeSquare(height, ref width);
                    left = (int)((sourceWidth - width) / 2);
                    top = (int)((sourceHeight - height) / 2);
                    break;
                case Aspect.Fill:
                    height = sourceHeight;
                    width = sourceWidth;
                    break;
                case Aspect.AspectFit:
                    height = sourceHeight;
                    width = sourceWidth;
                    height = this.MakeSquare(height, ref width);
                    left = (int)((sourceWidth - width) / 2);
                    top = (int)((sourceHeight - height) / 2);
                    break;
                default:
                    throw new NotImplementedException();
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

        private Rect GetTargetRect(int sourceHeight, int sourceWidth)
        {
            int height = 0;
            int width = 0;

            height = this.Element.HeightRequest > 0
                       ? (int)System.Math.Round(this.Element.HeightRequest, 0)
                       : sourceHeight; 
            width = this.Element.WidthRequest > 0
                       ? (int)System.Math.Round(this.Element.WidthRequest, 0)
                       : sourceWidth; 

            // Make Square
            height = MakeSquare(height, ref width);

            return new Rect(0, 0, width, height);
        }
    }
}
