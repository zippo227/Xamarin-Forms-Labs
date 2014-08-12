using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Microsoft.Phone.Controls;
using Xamarin.Forms.Labs.WP8.Controls;

[assembly: ExportRenderer(typeof(ExtendedLabel), typeof(ExtendedLabelRenderer))]

namespace Xamarin.Forms.Labs.Controls
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

            UpdateUi(view, Control);
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
        private static void UpdateUi(ExtendedLabel view, TextBlock control)
        {
            if (!string.IsNullOrEmpty(view.FontName))
            {
                control.FontFamily = new FontFamily(view.FontName);
                control.FontSize = (view.FontSize > 0) ? (float)view.FontSize : 12.0f;
            }

            if (view.IsUnderline)
            {
                control.TextDecorations = TextDecorations.Underline;
            }

            if (view.IsStrikeThrough)
            {
                // TODO: When StrikeThrough support is added
            }
        }
    }
}

