using System;
using System.Collections.Generic;
using System.ComponentModel;
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

[assembly: ExportRenderer(typeof(ExtendedScrollView), typeof(ExtendedScrollViewRenderer))]

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The extended label renderer.
    /// </summary>
    public class ExtendedScrollViewRenderer : ScrollViewRenderer
    {
        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
        {
            base.OnElementChanged(e);

            var scrollView = (ExtendedScrollView)Element;

            scrollView.Scrolled += (sender, ev) =>
            {
                scrollView.UpdateBounds(scrollView.Bounds);
                scrollView.Position = scrollView.Bounds.Location;
            };

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }

        double EPSILON = 0.1;

        protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExtendedScrollView.PositionProperty.PropertyName)
            {
                var scrollView = (ExtendedScrollView)Element;
                var position = scrollView.Position;

                if (Math.Abs(scrollView.Bounds.Location.Y - position.Y) < EPSILON
                    && Math.Abs(scrollView.Bounds.Location.X - position.X) < EPSILON)
                {
                    return;
                }

                Control.ScrollToVerticalOffset(position.Y);
                Control.UpdateLayout();

                //this.ScrollRectToVisible(
                //    new RectangleF((float)position.X, (float)position.Y, Bounds.Width, Bounds.Height), scrollView.AnimateScroll);
            }
        }

    }
}

