using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace XForms.Toolkit.Sample
{	
	public partial class DeviceInfoPage : ContentPage
	{	
		public DeviceInfoPage ()
		{
			InitializeComponent ();
			BindingContext = ViewModelLocator.Main;
		}

		protected override void OnAppearing ()
		{
			//start the timer on the VM, ideally we would have access to this event on the VM
			ViewModelLocator.Main.StartTimer ();
			base.OnAppearing ();
		}
	}
}

