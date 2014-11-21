using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace XLabs.Forms.Controls
{
	/// <summary>
	/// Class AutoCompleteView.
	/// </summary>
	public class AutoCompleteView : ContentView
	{
		/// <summary>
		/// The _ent text
		/// </summary>
		private readonly Entry _entText;
		/// <summary>
		/// The _BTN search
		/// </summary>
		private readonly Button _btnSearch;
		/// <summary>
		/// The _LST sugestions
		/// </summary>
		private readonly ListView _lstSugestions;
		/// <summary>
		/// The _STK base
		/// </summary>
		private readonly StackLayout _stkBase;
		/// <summary>
		/// Initializes a new instance of the <see cref="AutoCompleteView"/> class.
		/// </summary>
		public AutoCompleteView()
		{

			//InitializeComponent();
			_stkBase = new StackLayout();
			var innerLayout = new StackLayout();
			_entText = new Entry()
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Start
			};
			_btnSearch = new Button()
			{
				VerticalOptions = LayoutOptions.Center,
				Text = "Search"
			};

			_lstSugestions = new ListView()
			{
				HeightRequest = 250,
				HasUnevenRows = true
			};

			innerLayout.Children.Add(_entText);
			innerLayout.Children.Add(_btnSearch);
			_stkBase.Children.Add(innerLayout);
			_stkBase.Children.Add(_lstSugestions);

			Content = _stkBase;


			_entText.TextChanged += (s, e) =>
			{
				Text = e.NewTextValue;
			};
			_btnSearch.Clicked += (s, e) =>
			{
				if (SearchCommand != null && SearchCommand.CanExecute(Text))
					SearchCommand.Execute(Text);
			};
			_lstSugestions.ItemSelected += (s, e) =>
			{
				_entText.Text = GetSearchString(e.SelectedItem);

				AvailableSugestions.Clear();
				ShowHideListbox(false);
				SelectedCommand.Execute(e);
				if (ExecuteOnSugestionClick
				   && SearchCommand != null && SearchCommand.CanExecute(Text))
				{
					SearchCommand.Execute(e);
				}

			};
			AvailableSugestions = new ObservableCollection<object>();
			this.ShowHideListbox(false);
			_lstSugestions.ItemsSource = this.AvailableSugestions;
			//lstSugestions.ItemTemplate = this.SugestionItemDataTemplate;
		}

		/// <summary>
		/// Shows the hide listbox.
		/// </summary>
		/// <param name="show">if set to <c>true</c> [show].</param>
		private void ShowHideListbox(bool show)
		{
			_lstSugestions.IsVisible = show;
		}

		/// <summary>
		/// Gets the text entry.
		/// </summary>
		/// <value>The text entry.</value>
		public Entry TextEntry
		{
			get
			{
				return _entText;
			}
		}

		/// <summary>
		/// Gets the ListView sugestions.
		/// </summary>
		/// <value>The ListView sugestions.</value>
		public ListView ListViewSugestions
		{
			get
			{
				return _lstSugestions;
			}
		}

		/// <summary>
		/// Gets the available sugestions.
		/// </summary>
		/// <value>The available sugestions.</value>
		public ObservableCollection<object> AvailableSugestions
		{
			get;
			private set;
		}


		#region Bindable Properties

		/// <summary>
		/// The sugestions property
		/// </summary>
		public static readonly BindableProperty SugestionsProperty =
			BindableProperty.Create<AutoCompleteView, ObservableCollection<object>>
		(p => p.Sugestions, null);

		/// <summary>
		/// Gets or sets the sugestions.
		/// </summary>
		/// <value>The sugestions.</value>
		public ObservableCollection<object> Sugestions
		{
			get { return (ObservableCollection<object>)GetValue(SugestionsProperty); }
			set { SetValue(SugestionsProperty, value); }
		}


		/// <summary>
		/// The text property
		/// </summary>
		public static readonly BindableProperty TextProperty =
			BindableProperty.Create<AutoCompleteView, string>(p => p.Text, "", BindingMode.TwoWay, null,
				new BindableProperty.BindingPropertyChangedDelegate<string>(TextChanged), null, null);

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Texts the changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="oldPlaceHolderValue">The old place holder value.</param>
		/// <param name="newPlaceHolderValue">The new place holder value.</param>
		static void TextChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
		{
			var control = (obj as AutoCompleteView);

			control._btnSearch.IsEnabled = !string.IsNullOrEmpty(newPlaceHolderValue);
			string cleanedNewPlaceHolderValue = Regex.Replace((newPlaceHolderValue ?? "").ToLowerInvariant(), @"\s+", string.Empty);
			if (!string.IsNullOrEmpty(cleanedNewPlaceHolderValue) && control.Sugestions != null)
			{
				var filteredsugestions = control.Sugestions.Where(x =>
				{
					return Regex.Replace(GetSearchString(x).ToLowerInvariant(), @"\s+", string.Empty).Contains(cleanedNewPlaceHolderValue);
				}).OrderByDescending(x =>
				{
					return Regex.Replace(GetSearchString(x).ToLowerInvariant(), @"\s+", string.Empty).StartsWith(cleanedNewPlaceHolderValue);
				}).ToArray();

				control.AvailableSugestions.Clear();

				foreach (var item in filteredsugestions)
				{
					control.AvailableSugestions.Add(item);
				}
				if (control.AvailableSugestions.Count > 0)
				{
					control.ShowHideListbox(true);
				}
			}
			else
			{
				if (control.AvailableSugestions.Count > 0)
				{
					control.AvailableSugestions.Clear();
					control.ShowHideListbox(false);
				}
			}
		}

		/// <summary>
		/// Gets the search string.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <returns>System.String.</returns>
		public static string GetSearchString(object x)
		{
			IAutoCompleteSearchObject itm;
			if ((itm = x as IAutoCompleteSearchObject) != null)
			{
				return itm.StringToSearchBy();
			}
			else
			{
				return x.ToString();
			}
		}
		/// <summary>
		/// The placeholder property
		/// </summary>
		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create<AutoCompleteView, string>(
				p => p.Placeholder, "", BindingMode.TwoWay, null,
				new BindableProperty.BindingPropertyChangedDelegate<string>(PlaceHolderChanged));

		/// <summary>
		/// Gets or sets the placeholder.
		/// </summary>
		/// <value>The placeholder.</value>
		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set { SetValue(PlaceholderProperty, value); }
		}

		/// <summary>
		/// Places the holder changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="oldPlaceHolderValue">The old place holder value.</param>
		/// <param name="newPlaceHolderValue">The new place holder value.</param>
		static void PlaceHolderChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
		{
			(obj as AutoCompleteView).TextEntry.Placeholder = newPlaceHolderValue;

		}


		/// <summary>
		/// The show search property
		/// </summary>
		public static readonly BindableProperty ShowSearchProperty =
			BindableProperty.Create<AutoCompleteView, bool>(
				p => p.ShowSearchButton, true, BindingMode.TwoWay, null, new BindableProperty.BindingPropertyChangedDelegate<bool>(ShowSearchChanged));

		/// <summary>
		/// Gets or sets a value indicating whether [show search button].
		/// </summary>
		/// <value><c>true</c> if [show search button]; otherwise, <c>false</c>.</value>
		public bool ShowSearchButton
		{
			get { return (bool)GetValue(ShowSearchProperty); }
			set { SetValue(ShowSearchProperty, value); }
		}

		/// <summary>
		/// Shows the search changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="oldShowSearchValue">if set to <c>true</c> [old show search value].</param>
		/// <param name="newShowSearchValue">if set to <c>true</c> [new show search value].</param>
		static void ShowSearchChanged(BindableObject obj, bool oldShowSearchValue, bool newShowSearchValue)
		{
			(obj as AutoCompleteView)._btnSearch.IsVisible = newShowSearchValue;

		}


		/// <summary>
		/// The search command property
		/// </summary>
		public static readonly BindableProperty SearchCommandProperty =
			BindableProperty.Create<AutoCompleteView, ICommand>(
				p => p.SearchCommand, null);

		/// <summary>
		/// Gets or sets the search command.
		/// </summary>
		/// <value>The search command.</value>
		public ICommand SearchCommand
		{
			get { return (ICommand)GetValue(SearchCommandProperty); }
			set { SetValue(SearchCommandProperty, value); }
		}

		/// <summary>
		/// The selected command property
		/// </summary>
		public static readonly BindableProperty SelectedCommandProperty =
			BindableProperty.Create<AutoCompleteView, ICommand>(
				p => p.SelectedCommand, null);

		/// <summary>
		/// Gets or sets the selected command.
		/// </summary>
		/// <value>The selected command.</value>
		public ICommand SelectedCommand
		{
			get { return (ICommand)GetValue(SelectedCommandProperty); }
			set { SetValue(SelectedCommandProperty, value); }
		}

		/// <summary>
		/// The sugestion item data template property
		/// </summary>
		public static readonly BindableProperty SugestionItemDataTemplateProperty =
			BindableProperty.Create<AutoCompleteView, DataTemplate>(p => p.SugestionItemDataTemplate, null,
				BindingMode.TwoWay, null,
				new BindableProperty.BindingPropertyChangedDelegate<DataTemplate>(SugestionItemDataTemplateChanged), null, null);

		/// <summary>
		/// Gets or sets the sugestion item data template.
		/// </summary>
		/// <value>The sugestion item data template.</value>
		public DataTemplate SugestionItemDataTemplate
		{
			get { return (DataTemplate)GetValue(SugestionItemDataTemplateProperty); }
			set { SetValue(SugestionItemDataTemplateProperty, value); }
		}

		/// <summary>
		/// Sugestions the item data template changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="oldShowSearchValue">The old show search value.</param>
		/// <param name="newShowSearchValue">The new show search value.</param>
		static void SugestionItemDataTemplateChanged(BindableObject obj, DataTemplate oldShowSearchValue, DataTemplate newShowSearchValue)
		{

			(obj as AutoCompleteView)._lstSugestions.ItemTemplate = newShowSearchValue;

		}


		/// <summary>
		/// The search background color property
		/// </summary>
		public static readonly BindableProperty SearchBackgroundColorProperty =
			BindableProperty.Create<AutoCompleteView, Color>(p => p.SearchBackgroundColor, Color.Red,
				BindingMode.TwoWay, null,
				new BindableProperty.BindingPropertyChangedDelegate<Color>(SearchBackgroundColorChanged), null, null);

		/// <summary>
		/// Gets or sets the color of the search background.
		/// </summary>
		/// <value>The color of the search background.</value>
		public Color SearchBackgroundColor
		{
			get { return (Color)GetValue(SearchBackgroundColorProperty); }
			set { SetValue(SearchBackgroundColorProperty, value); }
		}

		/// <summary>
		/// Searches the background color changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void SearchBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
		{
			(obj as AutoCompleteView)._stkBase.BackgroundColor = newValue;
		}


		/// <summary>
		/// The sugestion background color property
		/// </summary>
		public static readonly BindableProperty SugestionBackgroundColorProperty =
			BindableProperty.Create<AutoCompleteView, Color>(p => p.SugestionBackgroundColor, Color.Red,
				BindingMode.TwoWay, null,
				new BindableProperty.BindingPropertyChangedDelegate<Color>(SugestionBackgroundColorChanged), null, null);

		/// <summary>
		/// Gets or sets the color of the sugestion background.
		/// </summary>
		/// <value>The color of the sugestion background.</value>
		public Color SugestionBackgroundColor
		{
			get { return (Color)GetValue(SugestionBackgroundColorProperty); }
			set { SetValue(SugestionBackgroundColorProperty, value); }
		}

		/// <summary>
		/// Sugestions the background color changed.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		static void SugestionBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
		{

			(obj as AutoCompleteView)._lstSugestions.BackgroundColor = newValue;
		}


		/// <summary>
		/// The execute on sugestion click property
		/// </summary>
		public static readonly BindableProperty ExecuteOnSugestionClickProperty =
			BindableProperty.Create<AutoCompleteView, bool>(p => p.ExecuteOnSugestionClick, false);

		/// <summary>
		/// Gets or sets a value indicating whether [execute on sugestion click].
		/// </summary>
		/// <value><c>true</c> if [execute on sugestion click]; otherwise, <c>false</c>.</value>
		public bool ExecuteOnSugestionClick
		{
			get { return (bool)GetValue(ExecuteOnSugestionClickProperty); }
			set { SetValue(ExecuteOnSugestionClickProperty, value); }
		}



		#endregion
	}

	public interface IAutoCompleteSearchObject
	{
		string StringToSearchBy();
	}
}
