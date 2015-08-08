using Xamarin.Forms;

using XLabs.Forms.Controls;
using WebKit;
using System.Text;
using CoreGraphics;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace XLabs.Forms.Controls
{
	using System;

	using Foundation;
	using UIKit;

	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	using Size = Xamarin.Forms.Size;

	/// <summary>
	/// The hybrid web view renderer.
	/// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, XLabs.Forms.Controls.HybridWebViewRenderer.NativeWebView>, IWKScriptMessageHandler
	{
		//private UIWebView webView;
		private UISwipeGestureRecognizer _leftSwipeGestureRecognizer;
		private UISwipeGestureRecognizer _rightSwipeGestureRecognizer;

        private WKUserContentController userController;

		public HybridWebViewRenderer()
		{

		}

		/// <summary>
		/// The on element changed callback.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
                this.userController = new WKUserContentController();
                var config = new WKWebViewConfiguration()
                {
                    UserContentController = this.userController
                };
                
                var script = new WKUserScript(new NSString(NativeFunctionScript()), WKUserScriptInjectionTime.AtDocumentEnd, false);

                this.userController.AddUserScript(script);

                this.userController.AddScriptMessageHandler(this, "native");

                var webView = new NativeWebView(this.Frame, config);

                webView.NavigationDelegate = new NavDelegate(this);

				//this.InjectNativeFunctionScript();
				SetNativeControl(webView);

				//webView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
				//webView.ScalesPageToFit = true;

				_leftSwipeGestureRecognizer = new UISwipeGestureRecognizer(() => Element.OnLeftSwipe(this, EventArgs.Empty))
				{
					Direction = UISwipeGestureRecognizerDirection.Left
				};

				_rightSwipeGestureRecognizer = new UISwipeGestureRecognizer(()=> Element.OnRightSwipe(this, EventArgs.Empty))
				{
					Direction = UISwipeGestureRecognizerDirection.Right
				};

				Control.AddGestureRecognizer(_leftSwipeGestureRecognizer);
				Control.AddGestureRecognizer(_rightSwipeGestureRecognizer);
			}

			if (e.NewElement == null)
			{
				Control.RemoveGestureRecognizer(_leftSwipeGestureRecognizer);
				Control.RemoveGestureRecognizer(_rightSwipeGestureRecognizer);
			}

			this.Unbind(e.OldElement);
			this.Bind();
		}

		partial void HandleCleanup() 
        {
			if (Control != null) 
            {
				Control.RemoveGestureRecognizer(_leftSwipeGestureRecognizer);
				Control.RemoveGestureRecognizer(_rightSwipeGestureRecognizer);
			}
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
//            Inject(this.NativeFunctionScript());
			Element.OnLoadFinished(sender, e);
		}

		partial void Inject(string script)
        {
            InvokeOnMainThread(() => Control.EvaluateJavaScript(new NSString(script), (r, e) => 
                {
                    if (e != null)
                    //System.Diagnostics.Debug.WriteLine(r);
                    System.Diagnostics.Debug.WriteLine(e);
                }));
		}

		/* 
		 * This is a hack to because the base wasn't working 
		 * when within a stacklayout
		 */
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			Control.ScrollView.Frame = Control.Bounds;
		}

        public void DidReceiveScriptMessage(WebKit.WKUserContentController userContentController, WebKit.WKScriptMessage message)
        {
            this.Element.MessageReceived(message.Body.ToString());
        }

		partial void Load(Uri uri)
		{
			if (uri != null)
			{
				Control.LoadRequest(new NSUrlRequest(new NSUrl(uri.AbsoluteUri)));
			}
		}

		partial void LoadFromContent(object sender, string contentFullName)
		{
			Element.Uri = new Uri(NSBundle.MainBundle.BundlePath + "/" + contentFullName);
		}

		partial void LoadContent(object sender, string contentFullName)
		{
            Control.LoadHtmlString(new NSString(contentFullName), new NSUrl(NSBundle.MainBundle.BundlePath, true));
		}

		partial void LoadFromString(string html)
		{
			this.LoadContent(null, html);
		}

        private string NativeFunctionScript()
        {
            var builder = new StringBuilder();
//            builder.Append("function Native(action, data){ window.webkit.messageHandlers.native.postMessage(action)}");
            builder.Append("function Native(action, data){ ");
            builder.Append("   var send = JSON.stringify({ a: action, d: data });");
//            builder.Append("   var d = (typeof data == 'object') ? JSON.stringify(data) : data;");
            builder.Append("   window.webkit.messageHandlers.native.postMessage(send);");
            builder.Append("}");
            return builder.ToString();
        }

        public class NativeWebView : WKWebView
        {
            public NativeWebView(CGRect frame, WKWebViewConfiguration configuration) : base(frame, configuration){}

            public override WKNavigation LoadRequest(NSUrlRequest request)
            {
                var nav = base.LoadRequest(request);

                return nav;
//                return base.LoadRequest(request);
            }

            public override WKNavigation LoadHtmlString(NSString htmlString, NSUrl baseUrl)
            {
                return base.LoadHtmlString(htmlString, baseUrl);
            }


        }

        private class NavDelegate : WKNavigationDelegate
        {
            private HybridWebViewRenderer renderer;

            public NavDelegate(HybridWebViewRenderer renderer)
            {
                this.renderer = renderer;
            }

            public override void DidFinishNavigation(WKWebView webView, WKNavigation navigation)
            {
                this.renderer.LoadFinished(webView, EventArgs.Empty);
            }
        }
	}
}
