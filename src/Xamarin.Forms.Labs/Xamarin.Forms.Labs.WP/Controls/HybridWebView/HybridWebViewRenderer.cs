using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Microsoft.Phone.Controls;
using Xamarin.Forms.Labs.WP8.Controls;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]
namespace Xamarin.Forms.Labs.Controls
{
    public partial class HybridWebViewRenderer : ViewRenderer<HybridWebView, WebBrowser>
    {
        protected WebBrowser webView;

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (this.webView == null)
            {
                this.webView = new WebBrowser();

                this.webView.IsScriptEnabled = true;
                this.webView.Navigating += webView_Navigating;
                this.webView.LoadCompleted += webView_LoadCompleted;
                this.webView.ScriptNotify += WebViewOnScriptNotify;

                this.SetNativeControl(this.webView);
            }

            this.Unbind(e.OldElement);
            this.Bind();
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

        void webView_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            this.InjectNativeFunctionScript();
        }

        void webView_Navigating(object sender, NavigatingEventArgs e)
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
                this.webView.InvokeScript("eval", script);
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
                this.webView.Source = uri;
            }
        }

        partial void LoadFromContent(object sender, string contentFullName)
        {
            this.Element.Uri = new Uri(contentFullName, UriKind.Relative);
        }
    }
}
