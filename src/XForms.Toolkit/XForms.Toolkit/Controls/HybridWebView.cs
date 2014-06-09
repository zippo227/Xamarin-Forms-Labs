using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XForms.Toolkit.Services.Serialization;

namespace XForms.Toolkit.Controls
{
    public class HybridWebView : WebView
    {
        private IJsonConvert jsonConvert;
        private Dictionary<string, Action<string>> registeredActions;

        public HybridWebView(IJsonConvert jsonConvert)
        {
            this.jsonConvert = jsonConvert;
            this.registeredActions = new Dictionary<string, Action<string>>();
        }

        public static readonly BindableProperty UriProperty = BindableProperty.Create<HybridWebView, Uri>(p => p.Uri, default(Uri));

        public Uri Uri
        {
            get { return (Uri)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public void RegisterCallback(string name, Action<string> action)
        {
            this.registeredActions.Add(name, action);
        }

        public bool RemoveCallback(string name)
        {
            return this.registeredActions.Remove(name);
        }

        public void InjectJavaScript(string script)
        {
            var handler = this.JavaScriptLoadRequested;
            if (handler != null)
            {
                handler(this, script);
            }
        }

        public bool TryGetAction(string name, out Action<string> action)
        {
            return this.registeredActions.TryGetValue(name, out action);
        }

        public void CallJsFunction(string funcName, params object[] parameters)
        {
            var builder = new StringBuilder();

            builder.Append(funcName);
            builder.Append("(");

            for (var n = 0; n < parameters.Length; n++)
            {
                builder.Append(this.jsonConvert.ToJson(parameters[n]));
                if (n < parameters.Length - 1)
                {
                    builder.Append(", ");
                }
            }

            builder.Append(");");

            this.InjectJavaScript(builder.ToString());
        }

        public EventHandler<string> JavaScriptLoadRequested;
    }
}
