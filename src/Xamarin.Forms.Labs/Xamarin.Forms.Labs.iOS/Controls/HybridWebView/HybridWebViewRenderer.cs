using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, UIWebView>
    {
        private UIWebView webView;

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            this.webView = new UIWebView();

            this.webView.ShouldStartLoad += this.HandleStartLoad;
            this.InjectNativeFunctionScript();
            this.SetNativeControl(this.webView);

            this.Initialize();
        }

        private bool HandleStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            return !this.CheckRequest(request.Url.RelativeString);
        }

        partial void Inject(string script)
        {
            this.webView.EvaluateJavascript(script);
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.webView.LoadRequest(new NSUrlRequest(new NSUrl(uri.AbsoluteUri)));
            }
        }
    }
}