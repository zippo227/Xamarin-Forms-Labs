using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Serialization;

[assembly:
	InternalsVisibleTo("XLabs.Forms.Droid"),
	InternalsVisibleTo("XLabs.Forms.iOS"),
	InternalsVisibleTo("XLabs.Forms.WP8")]

namespace XLabs.Forms.Controls
{
	/// <summary>
	///     The hybrid web view.
	/// </summary>
	public class HybridWebView : View
	{
		/// <summary>
		///     The uri property.
		/// </summary>
		public static readonly BindableProperty UriProperty = BindableProperty.Create<HybridWebView, Uri>(p => p.Uri,
			default(Uri));

		/// <summary>
		///     The source property.
		/// </summary>
		public static readonly BindableProperty SourceProperty =
			BindableProperty.Create<HybridWebView, WebViewSource>(p => p.Source, default(WebViewSource));

		internal EventHandler<string> JavaScriptLoadRequested;
		public EventHandler LeftSwipe;
		internal EventHandler<string> LoadContentRequested;
		public EventHandler LoadFinished;
		internal EventHandler<string> LoadFromContentRequested;
		public EventHandler<EventArgs<Uri>> Navigating;
		public EventHandler RightSwipe;

		/// <summary>
		///     The inject lock.
		/// </summary>
		private readonly object _injectLock = new object();

		/// <summary>
		///     The JSON serializer.
		/// </summary>
		private readonly IStringSerializer _jsonSerializer;

		/// <summary>
		///     The registered actions.
		/// </summary>
		private readonly Dictionary<string, Action<string>> _registeredActions;

		/// <summary>
		///     The registered actions.
		/// </summary>
		private readonly Dictionary<string, Func<string, object[]>> _registeredFunctions;

		/// <summary>
		///     Initializes a new instance of the <see cref="HybridWebView" /> class.
		/// </summary>
		/// <remarks>
		///     HybridWebView will use either <see cref="IJsonSerializer" /> configured
		///     with IoC or if missing it will use <see cref="SystemJsonSerializer" /> by default.
		/// </remarks>
		public HybridWebView()
		{
			if (!Resolver.IsSet || (_jsonSerializer = Resolver.Resolve<IJsonSerializer>()) == null)
			{
				_jsonSerializer = new SystemJsonSerializer();
			}

			_registeredActions = new Dictionary<string, Action<string>>();
			_registeredFunctions = new Dictionary<string, Func<string, object[]>>();
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HybridWebView" /> class.
		/// </summary>
		/// <param name="jsonSerializer">
		///     The JSON serializer.
		/// </param>
		public HybridWebView(IJsonSerializer jsonSerializer)
		{
			_jsonSerializer = jsonSerializer;
			_registeredActions = new Dictionary<string, Action<string>>();
			_registeredFunctions = new Dictionary<string, Func<string, object[]>>();
		}

		/// <summary>
		///     Gets or sets the uri.
		/// </summary>
		public Uri Uri
		{
			get { return (Uri)GetValue(UriProperty); }
			set { SetValue(UriProperty, value); }
		}

		public WebViewSource Source
		{
			get { return (WebViewSource)GetValue(SourceProperty); }
			set { SetValue(SourceProperty, value); }
		}

		/// <summary>
		///     Registers a native callback.
		/// </summary>
		/// <param name="name">
		///     The name.
		/// </param>
		/// <param name="action">
		///     The action.
		/// </param>
		public void RegisterCallback(string name, Action<string> action)
		{
			_registeredActions.Add(name, action);
		}

		/// <summary>
		///     Removes a native callback.
		/// </summary>
		/// <param name="name">
		///     The name of the callback.
		/// </param>
		public bool RemoveCallback(string name)
		{
			return _registeredActions.Remove(name);
		}

		/// <summary>
		/// Registers a native callback and returns data to closure.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="func">The function.</param>
		public void RegisterNativeFunction(string name, Func<string, object[]> func)
		{
			_registeredFunctions.Add(name, func);
		}

		/// <summary>
		///     Removes a native callback function.
		/// </summary>
		/// <param name="name">
		///     The name of the callback.
		/// </param>
		public bool RegisterNativeFunction(string name)
		{
			return _registeredFunctions.Remove(name);
		}

		public void LoadFromContent(string contentFullName)
		{
			var handler = LoadFromContentRequested;
			if (handler != null)
			{
				handler(this, contentFullName);
			}
		}

		public void LoadContent(string content)
		{
			var handler = LoadContentRequested;
			if (handler != null)
			{
				handler(this, content);
			}
		}

		public void InjectJavaScript(string script)
		{
			lock (_injectLock)
			{
				var handler = JavaScriptLoadRequested;
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
				builder.Append(_jsonSerializer.Serialize(parameters[n]));
				if (n < parameters.Length - 1)
				{
					builder.Append(", ");
				}
			}

			builder.Append(");");

			InjectJavaScript(builder.ToString());
		}

		internal bool TryGetAction(string name, out Action<string> action)
		{
			return _registeredActions.TryGetValue(name, out action);
		}

		internal bool TryGetFunc(string name, out Func<string, object[]> func)
		{
			return _registeredFunctions.TryGetValue(name, out func);
		}

		internal void OnLoadFinished(object sender, EventArgs e)
		{
			var handler = LoadFinished;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		internal void OnLeftSwipe(object sender, EventArgs e)
		{
			var handler = LeftSwipe;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		internal void OnRightSwipe(object sender, EventArgs e)
		{
			var handler = RightSwipe;
			if (handler != null)
			{
				handler(this, e);
			}
		}

		internal void OnNavigating(Uri uri)
		{
			var handler = Navigating;
			if (handler != null)
			{
				handler(this, new EventArgs<Uri>(uri));
			}
		}
	}
}