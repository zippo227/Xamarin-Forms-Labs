using System;
using System.ComponentModel;
using System.Drawing;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
    /// <summary>
    /// A renderer for the ExtendedEntry control.
    /// </summary>
    public class ExtendedEntryRenderer : EntryRenderer
    {
        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedEntry)Element;

            SetFont(view);
            SetTextAlignment(view);
            SetBorder(view);

            ResizeHeight();
        }

        /// <summary>
        /// The on element property changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (ExtendedEntry)Element;

            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "Font")
                SetFont(view);

            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "XAlign")
                SetTextAlignment(view);

            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "HasBorder")
                SetBorder(view);

            ResizeHeight();
        }

        private void SetTextAlignment(ExtendedEntry view)
        {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        private void SetFont(ExtendedEntry view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(17f);
        }

        private void SetBorder(ExtendedEntry view)
        {
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
        }

        private void ResizeHeight()
        {
            if (Element.HeightRequest >= 0) return;

            var height = Math.Max(Bounds.Height,
                new UITextField {Font = Control.Font}.IntrinsicContentSize.Height);

            Control.Frame = new RectangleF(0.0f, 0.0f, (float) Element.Width, height);

            Element.HeightRequest = height;
        }
    }
}