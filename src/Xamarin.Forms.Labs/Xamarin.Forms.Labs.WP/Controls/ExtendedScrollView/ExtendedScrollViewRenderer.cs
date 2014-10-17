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
    public class ExtendedScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
        {
            base.OnElementChanged(e);

			LayoutUpdated += (sender, ev) =>
			{
				var scrollView = (ExtendedScrollView)Element;
				var bounds = new Rectangle(Control.HorizontalOffset, Control.VerticalOffset, Control.ScrollableWidth, Control.ScrollableHeight);
				scrollView.UpdateBounds(bounds);
			};

            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;

            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }

        double EPSILON = 0.1;

        protected void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ExtendedScrollView.PositionProperty.PropertyName)
            {
                var scrollView = (ExtendedScrollView)Element;
                var position = scrollView.Position;

				if (Math.Abs(Control.VerticalOffset - position.Y) < EPSILON
					&& Math.Abs(Control.HorizontalOffset - position.X) < EPSILON)
                    return;

                Control.ScrollToVerticalOffset(position.Y);
                Control.UpdateLayout();
            }
        }

    }
}

