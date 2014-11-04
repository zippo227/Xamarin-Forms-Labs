using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using MonoTouch.UIKit;
using System.Drawing;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(SegmentedControlView), typeof(SegmentedControlViewRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
    public class SegmentedControlViewRenderer : ViewRenderer<SegmentedControlView, UISegmentedControl>
    {
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.ValueChanged -= HandleControlValueChanged;
            }

            base.Dispose(disposing);
        }

        private void HandleControlValueChanged(object sender, EventArgs e)
        {
            Element.SelectedItem = Control.SelectedSegment;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "SelectedItem")
            {
                Control.SelectedSegment = Element.SelectedItem;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SegmentedControlView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                return;
            }

            var native = new UISegmentedControl(RectangleF.Empty);
            var segments = e.NewElement.SegmentsItens.Split(';');

            for (var i = 0; i < segments.Length; i++)
            {
                native.InsertSegment(segments[i], i, false);
            }

            native.TintColor = e.NewElement.TintColor.ToUIColor();
            native.SelectedSegment = e.NewElement.SelectedItem;

            SetNativeControl(native);

            Control.ValueChanged += HandleControlValueChanged;
        }
    }
}

