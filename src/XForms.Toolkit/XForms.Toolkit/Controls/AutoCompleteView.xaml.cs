using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Linq;

namespace XForms.Toolkit.Controls
{
	public partial class AutoCompleteView : ContentView
	{
		public AutoCompleteView ()
		{
			InitializeComponent ();
			entText.TextChanged += (s, e) => {
				Text = e.NewTextValue;
			};
			btnSearch.Clicked += (s, e) => {
				if(SearchCommand !=null && SearchCommand.CanExecute(Text))
					SearchCommand.Execute(Text);
			};
			lstSugestions.ItemSelected += (s, e) => {
				entText.Text = (string) e.SelectedItem;
				AvailableSugestions.Clear();
				ShowHideListbox(false);
			};
			AvailableSugestions = new ObservableCollection<string> ();
			this.ShowHideListbox (false);
			lstSugestions.BindingContext = this.AvailableSugestions;
		}

		private void ShowHideListbox(bool show){
			lstSugestions.IsVisible = show;
		}

		public Entry TextEntry {
			get{ 
				return entText;
			}
		}

		public ObservableCollection<string> AvailableSugestions {
			get;
			private set;
		}


		#region Bindable Properties

		public static readonly BindableProperty SugestionsProperty =
			BindableProperty.Create<AutoCompleteView, ObservableCollection<string>> 
		(p => p.Sugestions, null);

		public ObservableCollection<string> Sugestions {
			get { return (ObservableCollection<string>)GetValue (SugestionsProperty); }
			set { SetValue (SugestionsProperty, value); }
		}


		public static readonly BindableProperty TextProperty =
			BindableProperty.Create<AutoCompleteView, string> (p => p.Text, "", BindingMode.TwoWay, null, 
				new	BindableProperty.BindingPropertyChangedDelegate<string> (TextChanged), null, null);

		public string Text {
			get { return (string)GetValue (TextProperty); }
			set { SetValue (TextProperty, value); }
		}

		static void TextChanged (BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
		{
			var control = (obj as AutoCompleteView);

			control.btnSearch.IsEnabled = !string.IsNullOrEmpty (newPlaceHolderValue);

			if (!string.IsNullOrEmpty (newPlaceHolderValue) && control.Sugestions != null) {

				var filteredsugestions = control.Sugestions.Where (x => x.ToLowerInvariant ().Contains (newPlaceHolderValue))
					.OrderByDescending (x => x.ToLowerInvariant ().StartsWith (newPlaceHolderValue)).ToArray ();

				control.AvailableSugestions.Clear ();

				foreach (var item in filteredsugestions) {
					control.AvailableSugestions.Add (item);
				}
				if (control.AvailableSugestions.Count > 0) {
					control.ShowHideListbox (true);
				}
			} else {
				if (control.AvailableSugestions.Count > 0) {
					control.AvailableSugestions.Clear ();
					control.ShowHideListbox (false);
				}
			}
		}

		public static readonly BindableProperty PlaceholderProperty =
			BindableProperty.Create<AutoCompleteView,string> (
				p => p.Placeholder, "", BindingMode.TwoWay, null,
				new BindableProperty.BindingPropertyChangedDelegate<string> (PlaceHolderChanged));

		public string Placeholder {
			get { return (string)GetValue (PlaceholderProperty); }
			set { SetValue (PlaceholderProperty, value); }
		}

		static void PlaceHolderChanged (BindableObject obj, string oldPlaceHolderValue, string newPlaceHolderValue)
		{
			(obj as AutoCompleteView).TextEntry.Placeholder = newPlaceHolderValue;

		}

		public static readonly BindableProperty ShowSearchProperty =
			BindableProperty.Create<AutoCompleteView,bool> (
				p => p.ShowSearchButton, true, BindingMode.TwoWay, null, new BindableProperty.BindingPropertyChangedDelegate<bool> (ShowSearchChanged));

		public bool ShowSearchButton {
			get { return (bool)GetValue (ShowSearchProperty); }
			set { SetValue (ShowSearchProperty, value); }
		}

		static void ShowSearchChanged (BindableObject obj, bool oldShowSearchValue, bool newShowSearchValue)
		{
			(obj as AutoCompleteView).btnSearch.IsVisible = newShowSearchValue;

		}

		public static readonly BindableProperty SearchCommandProperty =
			BindableProperty.Create<AutoCompleteView,ICommand> (
				p => p.SearchCommand, null);

		public ICommand SearchCommand {
			get { return (ICommand)GetValue (SearchCommandProperty); }
			set { SetValue (SearchCommandProperty, value); }
		}

		#endregion
	}
}

