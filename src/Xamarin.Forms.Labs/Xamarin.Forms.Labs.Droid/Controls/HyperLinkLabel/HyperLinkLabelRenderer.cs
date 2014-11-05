using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Android.Text.Util;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]

namespace Xamarin.Forms.Labs.Controls
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