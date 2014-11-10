using System;
using System.Linq;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Microsoft.Phone.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The hybrid web view renderer.
    /// </summary>
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebBrowser>
    {
        /// <summary>
        /// The web view.
        /// </summary>
        protected WebBrowser WebView;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (this.WebView == null)
            {
                this.WebView = new WebBrowser { IsScriptEnabled = true };

                this.WebView.Navigating += webView_Navigating;
                this.WebView.LoadCompleted += webView_LoadCompleted;
                this.WebView.ScriptNotify += WebViewOnScriptNotify;

                this.SetNativeControl(this.WebView);
            }

            this.Unbind(e.OldElement);
            this.Bind();
        }

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(Size.Zero, Size.Zero);
        }

        private void WebViewOnScriptNotify(object sender, NotifyEventArgs notifyEventArgs)
        {
            Action<string> action;
            var values = notifyEventArgs.Value.Split('/');
            var name = values.FirstOrDefault();

            if (name != null && this.Element.TryGetAction(name, out action))
            {
                var data = Uri.UnescapeDataString(values.ElementAt(1));
                action.Invoke(data);
            }
        }

       private void webView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.InjectNativeFunctionScript();
        }

       partial void LoadContent(object sender, string contentFullName)
       {
           LoadFromContent(sender, contentFullName);
       }

        private void webView_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.IsAbsoluteUri && this.CheckRequest(e.Uri.AbsoluteUri))
            {
                System.Diagnostics.Debug.WriteLine(e.Uri);
            }
        }

        partial void Inject(string script)
        {
            try
            {
                this.WebView.InvokeScript("eval", script);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        partial void Load(Uri uri)
        {
            if (uri != null)
            {
                this.WebView.Source = uri;
            }
        }

        partial void LoadFromContent(object sender, string contentFullName)
        {
            this.Element.Uri = new Uri(contentFullName, UriKind.Relative);
        }
    }
}
