using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace Xamarin.Forms.Labs.Controls
{
    public class Preserve : Attribute
    {

    }
    [Preserve()]
    public partial class AutoCompleteView : ContentView
    {
        public AutoCompleteView()
        {
            InitializeComponent();
            entText.TextChanged += (s, e) =>
            {
                Text = e.NewTextValue;
            };
            btnSearch.Clicked += (s, e) =>
            {
                if (SearchCommand != null && SearchCommand.CanExecute(Text))
                    SearchCommand.Execute(Text);
            };
            lstSugestions.ItemSelected += (s, e) =>
            {
				entText.Text = (e.SelectedItem).ToString();
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
            lstSugestions.ItemsSource = this.AvailableSugestions;
            //lstSugestions.ItemTemplate = this.SugestionItemDataTemplate;
        }

        private void ShowHideListbox(bool show)
        {
            lstSugestions.IsVisible = show;
        }

        public Entry TextEntry
        {
            get
            {
                return entText;
            }
        }

        public ListView ListViewSugestions
        {
            get
            {
                return lstSugestions;
            }
        }

        public ObservableCollection<object> AvailableSugestions
        {
            get;
            private set;
        }


        #region Bindable Properties

        public static readonly BindableProperty SugestionsProperty =
            BindableProperty.Create<AutoCompleteView, ObservableCollection<object>>
        (p => p.Sugestions, null);

        public ObservableCollection<object> Sugestions
        {
            get { return (ObservableCollection<object>)GetValue(SugestionsProperty); }
            set { SetValue(SugestionsProperty, value); }
        }


        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<AutoCompleteView, string>(p => p.Text, "", BindingMode.TwoWay, null,
                new BindableProperty.BindingPropertyChangedDelegate<string>(TextChanged), null, null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        static void TextChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
        {
            var control = (obj as AutoCompleteView);

            control.btnSearch.IsEnabled = !string.IsNullOrEmpty(newPlaceHolderValue);

            if (!string.IsNullOrEmpty(newPlaceHolderValue) && control.Sugestions != null)
            {

				var filteredsugestions = control.Sugestions.Where(x => x.ToString().ToLowerInvariant().Contains(newPlaceHolderValue.ToLowerInvariant()))
					.OrderByDescending(x => x.ToString().ToLowerInvariant().StartsWith(newPlaceHolderValue.ToLowerInvariant())).ToArray();

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


        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create<AutoCompleteView, string>(
                p => p.Placeholder, "", BindingMode.TwoWay, null,
                new BindableProperty.BindingPropertyChangedDelegate<string>(PlaceHolderChanged));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        static void PlaceHolderChanged(BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
        {
            (obj as AutoCompleteView).TextEntry.Placeholder = newPlaceHolderValue;

        }


        public static readonly BindableProperty ShowSearchProperty =
            BindableProperty.Create<AutoCompleteView, bool>(
                p => p.ShowSearchButton, true, BindingMode.TwoWay, null, new BindableProperty.BindingPropertyChangedDelegate<bool>(ShowSearchChanged));

        public bool ShowSearchButton
        {
            get { return (bool)GetValue(ShowSearchProperty); }
            set { SetValue(ShowSearchProperty, value); }
        }

        static void ShowSearchChanged(BindableObject obj, bool oldShowSearchValue, bool newShowSearchValue)
        {
            (obj as AutoCompleteView).btnSearch.IsVisible = newShowSearchValue;

        }


        public static readonly BindableProperty SearchCommandProperty =
            BindableProperty.Create<AutoCompleteView, ICommand>(
                p => p.SearchCommand, null);

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

		public static readonly BindableProperty SelectedCommandProperty =
			BindableProperty.Create<AutoCompleteView, ICommand>(
				p => p.SelectedCommand, null);

		public ICommand SelectedCommand
		{
			get { return (ICommand)GetValue(SelectedCommandProperty); }
			set { SetValue(SelectedCommandProperty, value); }
		}

        public static readonly BindableProperty SugestionItemDataTemplateProperty =
            BindableProperty.Create<AutoCompleteView, DataTemplate>(p => p.SugestionItemDataTemplate, null,
                BindingMode.TwoWay, null,
                new BindableProperty.BindingPropertyChangedDelegate<DataTemplate>(SugestionItemDataTemplateChanged), null, null);

        public DataTemplate SugestionItemDataTemplate
        {
            get { return (DataTemplate)GetValue(SugestionItemDataTemplateProperty); }
            set { SetValue(SugestionItemDataTemplateProperty, value); }
        }

        static void SugestionItemDataTemplateChanged(BindableObject obj, DataTemplate oldShowSearchValue, DataTemplate newShowSearchValue)
        {

            (obj as AutoCompleteView).lstSugestions.ItemTemplate = newShowSearchValue;

        }


        public static readonly BindableProperty SearchBackgroundColorProperty =
            BindableProperty.Create<AutoCompleteView, Color>(p => p.SearchBackgroundColor, Color.Red,
                BindingMode.TwoWay, null,
                new BindableProperty.BindingPropertyChangedDelegate<Color>(SearchBackgroundColorChanged), null, null);

        public Color SearchBackgroundColor
        {
            get { return (Color)GetValue(SearchBackgroundColorProperty); }
            set { SetValue(SearchBackgroundColorProperty, value); }
        }

        static void SearchBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {
            (obj as AutoCompleteView).stkBase.BackgroundColor = newValue;
        }


        public static readonly BindableProperty SugestionBackgroundColorProperty =
            BindableProperty.Create<AutoCompleteView, Color>(p => p.SugestionBackgroundColor, Color.Red,
                BindingMode.TwoWay, null,
                new BindableProperty.BindingPropertyChangedDelegate<Color>(SugestionBackgroundColorChanged), null, null);

        public Color SugestionBackgroundColor
        {
            get { return (Color)GetValue(SugestionBackgroundColorProperty); }
            set { SetValue(SugestionBackgroundColorProperty, value); }
        }

        static void SugestionBackgroundColorChanged(BindableObject obj, Color oldValue, Color newValue)
        {

            (obj as AutoCompleteView).lstSugestions.BackgroundColor = newValue;
        }


        public static readonly BindableProperty ExecuteOnSugestionClickProperty =
            BindableProperty.Create<AutoCompleteView, bool>(p => p.ExecuteOnSugestionClick, false);

        public bool ExecuteOnSugestionClick
        {
            get { return (bool)GetValue(ExecuteOnSugestionClickProperty); }
            set { SetValue(ExecuteOnSugestionClickProperty, value); }
        }



        #endregion
    }
}

