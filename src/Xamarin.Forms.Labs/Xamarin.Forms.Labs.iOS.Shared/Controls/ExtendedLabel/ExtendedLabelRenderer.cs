#if __UNIFIED__
using UIKit;
using Foundation;
#elif __IOS__
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
    /// <summary>
    /// The extended label renderer.
    /// </summary>
    public class ExtendedLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            var view = (ExtendedLabel)Element;

            UpdateUi(view, this.Control);
        }

        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="view">
        /// The view.
        /// </param>
        /// <param name="control">
        /// The control.
        /// </param>
        private static void UpdateUi(ExtendedLabel view, UILabel control)
        {
            if (view.FontSize > 0)
            {
                control.Font = UIFont.FromName(control.Font.Name,(float)view.FontSize);
            }

            if (!string.IsNullOrEmpty(view.FontName))
            {
                string fontName = view.FontName;
                //if extension given then remove it for iOS
                if (fontName.LastIndexOf(".", System.StringComparison.Ordinal) == fontName.Length - 4)
                {
                    fontName = fontName.Substring(0, fontName.Length - 4);
                }

                var font = UIFont.FromName(
                    fontName, control.Font.PointSize);

                if (font != null)
                {
                    control.Font = font;
                }
            }

            //======= This is for backward compatability with obsolete attrbute 'FontNameIOS' ========
            if (!string.IsNullOrEmpty(view.FontNameIOS))
            {
                var font = UIFont.FromName(
                    view.FontNameIOS,
                   (view.FontSize > 0) ? (float)view.FontSize : 12.0f);

                if (font != null)
                {
                    control.Font = font;
                }
            }
            //====== End of obsolete section ==========================================================

            var attrString = new NSMutableAttributedString(control.Text);

            if (view.IsUnderline)
            {
                //control.AttributedText = new NSAttributedString(
                //    control.Text,
                //    underlineStyle: NSUnderlineStyle.Single);

                attrString.AddAttribute(UIStringAttributeKey.UnderlineStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), new NSRange(0, attrString.Length));
            }

            if (view.IsStrikeThrough)
            {
                attrString.AddAttribute(UIStringAttributeKey.StrikethroughStyle, NSNumber.FromInt32((int)NSUnderlineStyle.Single), new NSRange(0, attrString.Length));
            }

            control.AttributedText = attrString;
        }
    }
}

