using System;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XLabs.Sample
{
	public class ExtendedTimePickerPage : ContentPage
	{
		public ExtendedTimePickerPage()
		{
			var timePicker = new ExtendedTimePicker();

			timePicker.Time = new TimeSpan(11, 12, 0);
			timePicker.MinimumTime = new TimeSpan(10, 10, 0);
			timePicker.MaximumTime = new TimeSpan(18, 40, 0);

			Content = new StackLayout
			{
				Children =
				{
					timePicker
				}
			};
		}
	}
}

