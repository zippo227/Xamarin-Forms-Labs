using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Mvvm;

namespace Xamarin.Forms.Labs.Sample
{
    public partial class MvvmSamplePage : BaseView
    {
        public MvvmSamplePage()
		{
			InitializeComponent ();
			BindingContext = new MvvmSampleViewModel ();

            Icon = Device.OnPlatform("services1_32.png", "services1_32.png", "Images/services1_32.png");
		}
    }
}

