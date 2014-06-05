using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Sample
{	
	public partial class TextToSpeechPage : ContentPage
	{	
		public TextToSpeechPage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}
	}
}

