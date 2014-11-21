using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;
using System.Drawing;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, UIWebView>
    {
        //private UIWebView webView;
        private UISwipeGestureRecognizer leftSwipeGestureRecognizer;
        private UISwipeGestureRecognizer rightSwipeGestureRecognizer;

        /// <summary>
        /// The on element changed callback.
        /// </summary>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                var webView = new UIWebView();
                webView.LoadFinished += LoadFinished;
                webView.ShouldStartLoad += this.HandleStartLoad;
                //this.InjectNativeFunctionScript();
                this.SetNativeControl(webView);

                //webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                //webView.ScalesPageToFit = true;

                this.leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => this.Element.OnLeftSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Left
                };

                this.rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(()=> this.Element.OnRightSwipe(this, EventArgs.Empty))
                {
                    Direction = UISwipeGestureRecognizerDirection.Right
                };

                this.Control.AddGestureRecognizer(this.leftSwipeGestureRecognizer);
                this.Control.AddGestureRecognizer(this.rightSwipeGestureRecognizer);
            }

            if (e.NewElement == null)
            {
                this.Control.RemoveGestureRecognizer(this.leftSwipeGestureRecognizer);
                this.Control.RemoveGestureRecognizer(this.rightSwipeGestureRecognizer);
            }

            this.Unbind(e.OldElement);
            this.Bind();
        }

        /// <summary>
        /// Gets the desired size of the view.
        /// </summary>
        /// <returns>The desired size.</returns>
        /// <param name="widthConstraint">Width constraint.</param>
        /// <param name="heightConstraint">Height constraint.</param>
        /// <remarks>
        /// We need to override this method and set the request to 0. Otherwise on view refresh
        /// we will get incorrect view height and might lose the ability to scroll the webview
        /// completely.
        /// </remarks>
        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(Size.Zero, Size.Zero);
        }

        void LoadFinished(object sender, EventArgs e)
        {
            InjectNativeFunctionScript();
            this.Element.OnLoadFinished(sender, e);
        }

        private bool HandleStartLoad(UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType)
        {
            var shouldStartLoad = !this.CheckRequest(request.Url.RelativeString);
            if (shouldStartLoad) 
            {
                this.Element.OnNavigating(new Uri(request.Url.AbsoluteUrl.AbsoluteString));
            }
            return shouldStartLoad;
        }

        partial void Inject(string script)
        {
            InvokeOnMainThread(() => this.Control.EvaluateJavascript(script));
        }

        /* 
         * This is a hack to because the base wasn't working 
         * when within a stacklayout
         */
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            this.Control.ScrollView.Frame = this.Control.Bounds;
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.Control.LoadRequest(new NSUrlRequest(new NSUrl(uri.AbsoluteUri)));
            }
        }

        partial void LoadFromContent(object sender, string contentFullName)
        {
            this.Element.Uri = new Uri(NSBundle.MainBundle.BundlePath + "/" + contentFullName);
        }

        partial void LoadContent(object sender, string contentFullName)
        {
            this.Control.LoadHtmlString(contentFullName, new NSUrl(NSBundle.MainBundle.BundlePath, true));
        }

        partial void LoadFromString(string html)
        {
            this.LoadContent(null, html);
        }
    }
}