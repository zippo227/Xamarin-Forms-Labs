
using Android.Graphics.Drawables;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Droid.Controls.ImageButton;
using XForms.Toolkit.Enums;

[assembly: ExportRenderer(typeof(ImageButton), typeof(ImageButtonRenderer))]
namespace XForms.Toolkit.Droid.Controls.ImageButton
{
    /// <summary>
    /// Draws a button on the Android platform with the image shown in the right 
    /// position with the right size.
    /// </summary>
    public class ImageButtonRenderer : ButtonRenderer
    {
        protected override void OnModelChanged(VisualElement oldModel, VisualElement newModel)
        {
            base.OnModelChanged(oldModel, newModel);

            var targetButton = (Android.Widget.Button)Control;

            var model = newModel as Toolkit.Controls.ImageButton;
            if (model != null && !string.IsNullOrEmpty(model.Image))
            {

                SetImageSource(targetButton, model);
            }
        }

        /// <summary>
        /// Sets the image source.
        /// </summary>
        /// <param name="targetButton">The target button.</param>
        /// <param name="model">The model.</param>
        private void SetImageSource(Android.Widget.Button targetButton, Toolkit.Controls.ImageButton model)
        {
            var packageName = Context.PackageName;

            var resId = Resources.GetIdentifier(model.Image, "drawable", packageName);
            if (resId > 0)
            {
                var scaledDrawable = GetScaleDrawableFromResourceId(resId, GetWidth(model.ImageWidthRequest),
                    GetHeight(model.ImageHeightRequest));

                Drawable left = null;
                Drawable right = null;
                Drawable top = null;
                Drawable bottom = null;
                switch (model.Orientation)
                {
                    case (ImageOrientation.ImageToLeft):
                        left = scaledDrawable;
                        break;
                    case (ImageOrientation.ImageToRight):
                        right = scaledDrawable;
                        break;
                    case (ImageOrientation.ImageOnTop):
                        top = scaledDrawable;
                        break;
                    case (ImageOrientation.ImageOnBottom):
                        bottom = scaledDrawable;
                        break;
                }

                targetButton.SetCompoundDrawables(left, top, right, bottom);
            }
        }


        /// <summary>
        /// Called when the underlying model's properties are changed
        /// </summary>
        /// <param name="sender">Model</param>
        /// <param name="e">Event arguments</param>
        protected override void OnHandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnHandlePropertyChanged(sender, e);

            XForms.Toolkit.Controls.ImageButton btn = (XForms.Toolkit.Controls.ImageButton)this.Model;
            if (e.PropertyName == XForms.Toolkit.Controls.ImageButton.ImageProperty.PropertyName)
            {
                  var targetButton = (Android.Widget.Button)Control;
                  SetImageSource(targetButton, btn);
            }
           
        }

        /// <summary>
        /// Returns a <see cref="Drawable"/> with the correct dimensions from an 
        /// Android resource id.
        /// </summary>
        /// <param name="resId">The android resource id to load the drawable from.</param>
        /// <param name="width">The width to scale to.</param>
        /// <param name="height">The height to scale to.</param>
        /// <returns>A scaled <see cref="Drawable"/>.</returns>
        private Drawable GetScaleDrawableFromResourceId(int resId, int width, int height)
        {
            var drawable = Resources.GetDrawable(resId);

            var returnValue = new ScaleDrawable(drawable, 0, width, height).Drawable;
            returnValue.SetBounds(0, 0, width, height);
            return returnValue;
        }

        /// <summary>
        /// Gets the width based on the requested width, if request less than 0, returns 50.
        /// </summary>
        /// <param name="requestedWidth">The requested width.</param>
        /// <returns>The width to use.</returns>
        private int GetWidth(int requestedWidth)
        {
            const int defaultWidth = 50;
            return requestedWidth <= 0 ? defaultWidth : requestedWidth;
        }

        /// <summary>
        /// Gets the height based on the requested height, if request less than 0, returns 50.
        /// </summary>
        /// <param name="requestedHeight">The requested height.</param>
        /// <returns>The height to use.</returns>
        private int GetHeight(int requestedHeight)
        {
            const int defaultHeight = 50;
            return requestedHeight <= 0 ? defaultHeight : requestedHeight;
        }
    }
}