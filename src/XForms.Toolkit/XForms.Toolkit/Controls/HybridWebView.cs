using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XForms.Toolkit.Services;
using XForms.Toolkit.Services.Serialization;

namespace XForms.Toolkit.Controls
{
    public class HybridWebView : WebView
    {
        private readonly IStringSerializer jsonSerializer;
        private readonly Dictionary<string, Action<string>> registeredActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridWebView"/> class.
        /// </summary>
        public HybridWebView() : this(Resolver.Resolve<IJsonSerializer>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridWebView"/> class.
        /// </summary>
        /// <param name="jsonSerializer">
        /// The JSON serializer.
        /// </param>
        public HybridWebView(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
            this.registeredActions = new Dictionary<string, Action<string>>();
        }

        /// <summary>
        /// The uri property.
        /// </summary>
        public static readonly BindableProperty UriProperty = BindableProperty.Create<HybridWebView, Uri>(p => p.Uri, default(Uri));

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public Uri Uri
        {
            get { return (Uri)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        /// <summary>
        /// Registers a native callback.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
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
                builder.Append(this.jsonSerializer.Serialize(parameters[n]));
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
