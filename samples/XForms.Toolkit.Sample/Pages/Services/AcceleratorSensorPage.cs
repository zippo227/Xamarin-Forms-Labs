using System;
using Xamarin.Forms;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Sample
{
    public class AcceleratorSensorPage : ContentPage
    {
        public AcceleratorSensorPage()
        {
            var device = Resolver.Resolve<IDevice> ();

            if (device.Accelerometer == null)
            {
                this.Content = new Label () 
                {
                    TextColor = Color.Red,
                    Text = "Device does not have accelerometer sensor or it is not enabled."
                };

                return;
            }

            var grid = new StackLayout ();

            var xsensor = new SensorBarView () 
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = this.BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            var ysensor = new SensorBarView ()
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = this.BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };

            var zsensor = new SensorBarView ()
            {
                HeightRequest = 75,
                WidthRequest = 250,
                MinimumHeightRequest = 10,
                MinimumWidthRequest = 50,
                BackgroundColor = this.BackgroundColor
//                VerticalOptions = LayoutOptions.Fill,
//                HorizontalOptions = LayoutOptions.Fill
            };


            grid.Children.Add (new Label () { Text = string.Format ("Accelerometer data for {0}", device.Name) });
            grid.Children.Add (new Label () { Text = "X", XAlign = TextAlignment.Center });
            grid.Children.Add (xsensor);
            grid.Children.Add (new Label () { Text = "Y", XAlign = TextAlignment.Center });
            grid.Children.Add (ysensor);
            grid.Children.Add (new Label () { Text = "Z", XAlign = TextAlignment.Center });
            grid.Children.Add (zsensor);

            device.Accelerometer.ReadingAvailable += (sender, e) => 
            {
                xsensor.CurrentValue = e.Value.X;
                ysensor.CurrentValue = e.Value.Y;
                zsensor.CurrentValue = e.Value.Z;
            };

            this.Content = grid;
        }
    }
}

