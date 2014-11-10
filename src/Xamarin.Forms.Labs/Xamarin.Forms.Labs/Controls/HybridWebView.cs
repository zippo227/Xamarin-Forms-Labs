using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using XLabs;
using XLabs.Ioc;
using XLabs.Serialization;

[assembly:
    InternalsVisibleTo("Xamarin.Forms.Labs.Droid"),
    InternalsVisibleTo("Xamarin.Forms.Labs.iOS"),
    InternalsVisibleTo("Xamarin.Forms.Labs.WP8")]

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The hybrid web view.
    /// </summary>
    public class HybridWebView : View
    {
        /// <summary>
        /// The inject lock.
        /// </summary>
        private readonly object injectLock = new object();

        /// <summary>
        /// The JSON serializer.
        /// </summary>
        private readonly IStringSerializer jsonSerializer;

        /// <summary>
        /// The registered actions.
        /// </summary>
        private readonly Dictionary<string, Action<string>> registeredActions;

        /// <summary>
        /// The registered actions.
        /// </summary>
        private readonly Dictionary<string, Func<string, object[]>> registeredFunctions;

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridWebView"/> class.
        /// </summary>
        /// <remarks>HybridWebView will use either <see cref="IJsonSerializer"/> configured
        /// with IoC or if missing it will use <see cref="SystemJsonSerializer"/> by default.</remarks>
        public HybridWebView()
        {
            if (!Resolver.IsSet || (this.jsonSerializer = Resolver.Resolve<IJsonSerializer>()) == null)
            {
                this.jsonSerializer = new SystemJsonSerializer();
            }

            this.registeredActions = new Dictionary<string, Action<string>>();
            registeredFunctions = new Dictionary<string, Func<string, object[]>>();
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
            registeredFunctions = new Dictionary<string, Func<string, object[]>>();
        }

        /// <summary>
        /// The uri property.
        /// </summary>
        public static readonly BindableProperty UriProperty = BindableProperty.Create<HybridWebView, Uri>(p => p.Uri, default(Uri));

        /// <summary>
        /// The source property.
        /// </summary>
        public static readonly BindableProperty SourceProperty = BindableProperty.Create<HybridWebView, WebViewSource>(p => p.Source, default(WebViewSource));

        /// <summary>
        /// Gets or sets the uri.
        /// </summary>
        public Uri Uri
        {
            get { return this.GetValue<Uri>(UriProperty); }
            set { this.SetValue(UriProperty, value); }
        }

        public WebViewSource Source
        {
            get { return this.GetValue<WebViewSource>(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
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

        /// <summary>
        /// Removes a native callback.
        /// </summary>
        /// <param name="name">
        /// The name of the callback.
        /// </param>
        public bool RemoveCallback(string name)
        {
            return this.registeredActions.Remove(name);
        }

        /// <summary>
        /// Registers a native callback and returns data to closure.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="action">
        /// The action.
        /// </param>
        public void RegisterNativeFunction(string name, Func<string, object[]> func)
        {
            this.registeredFunctions.Add(name, func);
        }

        /// <summary>
        /// Removes a native callback function.
        /// </summary>
        /// <param name="name">
        /// The name of the callback.
        /// </param>
        public bool RegisterNativeFunction(string name)
        {
            return this.registeredFunctions.Remove(name);
        }

        public void LoadFromContent(string contentFullName)
        {
            var handler = this.LoadFromContentRequested;
            if (handler != null)
            {
                handler(this, contentFullName);
            }
        }

        public void LoadContent(string content)
        {
            var handler = this.LoadContentRequested;
            if (handler != null)
            {
                handler(this, content);
            }
        }

        public void InjectJavaScript(string script)
        {
            lock (this.injectLock)
            {
                var handler = this.JavaScriptLoadRequested;
                if (handler != null)
                {
                    handler(this, script);
                }
            }
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

        public EventHandler LoadFinished;
        public EventHandler LeftSwipe;
        public EventHandler RightSwipe;
        public EventHandler<EventArgs<Uri>> Navigating;

        internal bool TryGetAction(string name, out Action<string> action)
        {
            return this.registeredActions.TryGetValue(name, out action);
        }

        internal bool TryGetFunc(string name, out Func<string, object[]> func)
        {
            return this.registeredFunctions.TryGetValue(name, out func);
        }

        internal void OnLoadFinished(object sender, EventArgs e)
        {
            var handler = this.LoadFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void OnLeftSwipe(object sender, EventArgs e)
        {
            var handler = this.LeftSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void OnRightSwipe(object sender, EventArgs e)
        {
            var handler = this.RightSwipe;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void OnNavigating(Uri uri)
        {
            var handler = this.Navigating;
            if (handler != null)
            {
                handler(this, new EventArgs<Uri>(uri));
            }
        }

        internal EventHandler<string> JavaScriptLoadRequested;
        internal EventHandler<string> LoadFromContentRequested;
        internal EventHandler<string> LoadContentRequested;
    }
}