using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using System.ComponentModel;
using System.Drawing;
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
                var label = (UILabel)Control;
                label.TextColor = UIColor.Blue;
                label.BackgroundColor = UIColor.Clear;
                label.UserInteractionEnabled = true;
                var tapXamarin = new UITapGestureRecognizer();

                tapXamarin.AddTarget(() =>
                {
                    var hyperLinkLabel = base.Element as HyperLinkLabel;
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(GetNavigationUri(hyperLinkLabel.NavigateUri)));
                });

                tapXamarin.NumberOfTapsRequired = 1;
                tapXamarin.DelaysTouchesBegan = true;
                label.AddGestureRecognizer(tapXamarin);
            }
        }

        private string GetNavigationUri(string uri)
        {
            if (uri.Contains("@") && !uri.StartsWith("mailto:"))
            {
                return string.Format("{0}{1}", "mailto:", uri);
            }
            else if (uri.StartsWith("www."))
            {
                return string.Format("{0}{1}", @"http://", uri);
            }
            return uri;
        }
    }
}