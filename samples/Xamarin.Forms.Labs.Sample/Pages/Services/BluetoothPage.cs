using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Services;

namespace Xamarin.Forms.Labs.Sample.Pages.Services
{
    public class BluetoothPage : ContentPage
    {
        public BluetoothPage()
        {
            var device = Resolver.Resolve<IDevice>();
            var bt = device.BluetoothHub;

            var stack = new StackLayout()
                {
                    
                };

            var button = new Button()
                {
                    Text = "Open BT settings"
                };

            button.SetBinding(Button.CommandProperty, "OpenSettings");
        }
    }
}
