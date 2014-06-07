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
		}
	
	}
}

