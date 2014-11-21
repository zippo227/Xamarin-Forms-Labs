using Android.Text.Util;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XLabs.Forms.Controls.HyperLinkLabel;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]

namespace XLabs.Forms.Controls.HyperLinkLabel
{
    public class HyperLinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {

                var nativeEditText = (global::Android.Widget.TextView)Control;

                Linkify.AddLinks(nativeEditText, MatchOptions.All);
            }
        }
    }
}