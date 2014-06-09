using System;
using Xamarin.Forms;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Sample.Pages.Controls;
using System.Diagnostics;
using System.Collections.Generic;

namespace XForms.Toolkit.Sample
{
	public class App
	{
		public static Page GetMainPage ()
		{	

			var tabPage = new ExtendedTabbedPage () { Title="XForms Toolkit Samples" };
			var mainPage = new NavigationPage (tabPage);

			var controls = new ContentPage () { Title="Controls"};
			controls.Title = "Controls";
			ListView lst = new ListView ();
			lst.ItemsSource = new List<string>() {"Calendar", "AutoComplete"};
			lst.ItemSelected += (sender, e) => {
				switch (e.SelectedItem.ToString()) {
				case "Calendar":
					mainPage.Navigation.PushAsync(new CalendarPage ());
					break;
				case "AutoComplete":
					mainPage.Navigation.PushAsync(new AutoCompletePage ());
					break;
				default:
					break;
				}
			};

			controls.Content = lst;

			var services = new ContentPage () { Title="Services"};
			ListView lstServices = new ListView ();
			lstServices.ItemsSource = new List<string>() {"Text To Speech", "Device Info"};
			lstServices.ItemSelected += (sender, e) => {
				switch (e.SelectedItem.ToString()) {
				case "Text To Speech":
					mainPage.Navigation.PushAsync(new TextToSpeechPage ());
					break;
				case "Device Info":
					mainPage.Navigation.PushAsync(new DeviceInfoPage ());
					break;
				default:
					break;
				}
			};
			services.Content = lstServices;

			var buttons = new CarouselPage ();
			buttons.Title = "Buttons";
			buttons.Children.Add (new ButtonPage ());

			var labels = new CarouselPage ();
			labels.Title = "Labels";
			labels.Children.Add (new ExtendedLabelPage ());

			tabPage.Children.Add (controls);
			tabPage.Children.Add (services);
			tabPage.Children.Add (buttons);
			tabPage.Children.Add (labels);

			tabPage.CurrentPageChanged +=
				() => Debug.WriteLine(string.Format("ExtendedTabbedPage CurrentPageChanged {0}",tabPage.CurrentPage.Title));


			return mainPage;
		}
	}
}

