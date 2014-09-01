using System;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Graphics;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Droid;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRender))]
namespace Xamarin.Forms.Labs.Droid
{
    public class ExtendedLabelRender : LabelRenderer
    {
        public ExtendedLabelRender() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedLabel)Element;
            var control = Control;

            UpdateUi(view, control);
        }

        void UpdateUi(ExtendedLabel view, TextView control)
        {
            if (!string.IsNullOrEmpty(view.FontName))
            {
                control.Typeface = TrySetFont(view.FontName);
            }

            //======= This is for backward compatability with obsolete attrbute 'FontNameAndroid' ========
            if (!string.IsNullOrEmpty(view.FontNameAndroid))
            {
                control.Typeface = TrySetFont(view.FontNameAndroid); ;
            }
            //====== End of obsolete section ==========================================================

            if (view.FontSize > 0)
            {
                control.TextSize = (float)view.FontSize;
            }

            if (view.IsUnderline)
            {
                control.PaintFlags = control.PaintFlags | PaintFlags.UnderlineText;
            }

            if (view.IsStrikeThrough)
            {
                control.PaintFlags = control.PaintFlags | PaintFlags.StrikeThruText;
            }
        }

        private Typeface TrySetFont(string fontName)
        {
            try
            {
                return Typeface.CreateFromAsset(Context.Assets, fontName);
            }
            catch (Exception ex)
            {
                Console.Write("not found in assets {0}", ex);

                try
                {
                    return Typeface.CreateFromFile(fontName);
                }
                catch (Exception ex1)
                {
                    Console.Write(ex1);

                    return Typeface.Default;
                }
            }
        }
    }
}

