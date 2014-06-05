using System;
using Xamarin.Forms;
using XForms.Toolkit.Controls;
using XForms.Toolkit.Sample.Pages.Controls;
using System.Diagnostics;

namespace XForms.Toolkit.Sample
{
	public class App
	{
		public static Page GetMainPage ()
		{	
			var mainPage = new ExtendedTabbedPage ();
			mainPage.CurrentPageChanged +=
				() => Debug.WriteLine(string.Format("ExtendedTabbedPage CurrentPageChanged {0}",mainPage.CurrentPage.Title));

			var controls = new CarouselPage ();
			controls.Title = "Controls";
			controls.Children.Add (new CalendarPage ());

			var services = new CarouselPage ();
			services.Title = "Services";
			services.Children.Add (new TextToSpeechPage ());

			var buttons = new CarouselPage ();
			buttons.Title = "Buttons";
			buttons.Children.Add (new ButtonPage ());

			mainPage.Children.Add (controls);
			mainPage.Children.Add (services);
			mainPage.Children.Add (buttons);

			return mainPage;
		}
	}
}

