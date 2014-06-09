using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XForms.Toolkit.Controls;

namespace XForms.Toolkit.Sample
{	
	public partial class ExtendedLabelPage : ContentPage
	{	
		public ExtendedLabelPage ()
		{
			InitializeComponent ();
			var label = new ExtendedLabel () {
				Text = "and From code",
			};
			label.FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "");
			stkRoot.Children.Add (label);
			var section = new TableSection () {
				new ExtendedTextCell {Text = "Sessions", FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "")},
				new ExtendedTextCell {Text = "Speakers",FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "")},
				new ExtendedTextCell {Text = "Favorites",FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "")},
				new ExtendedTextCell {Text = "Room Plan",FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "")},
				new ExtendedTextCell {Text = "Map",FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "")},
				new ExtendedTextCell {Text = "About",FontName = Device.OnPlatform<String>("Roboto-Light", "fonts/Roboto-Light.ttf", "")},
			};

			var root = new TableRoot () {section} ;

			var tableView = new TableView ()
			{ 
				Root = root,
				Intent = TableIntent.Menu,
			};
			stkRoot.Children.Add (tableView);
			this.BindingContext =  ViewModelLocator.Main;
		}
	
	}
}

