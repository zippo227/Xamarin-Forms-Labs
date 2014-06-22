using Android.Views;
using Android.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Labs.Droid.Controls.Calendar;
using Android.Renderscripts;

[assembly: ExportRenderer (typeof (Xamarin.Forms.Labs.Controls.CalendarView), typeof (CalendarViewRenderer))]
namespace Xamarin.Forms.Labs.Droid.Controls.Calendar
{

	public class CalendarViewRenderer : ViewRenderer<Xamarin.Forms.Labs.Controls.CalendarView,LinearLayout>
    {
		private const string TAG = "Xamarin.Forms.Labs.Controls.Calendar";

        Xamarin.Forms.Labs.Controls.CalendarView _view;
        CalendarPickerView _pickerView;

        public CalendarViewRenderer()
        {
        }

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Labs.Controls.CalendarView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null) {
				_view = e.NewElement;

				LayoutInflater inflatorservice =
					(LayoutInflater)Context.GetSystemService(Android.Content.Context.LayoutInflaterService);
				var containerView =
					(LinearLayout)inflatorservice.Inflate(Resource.Layout.calendar_picker, null, false);

				_pickerView = containerView.FindViewById<CalendarPickerView>(Resource.Id.calendar_view);
				_pickerView.Init(new DateTime(2014, 6, 1), new DateTime(2014, 6, 30))
					.InMode(CalendarPickerView.SelectionMode.Single);

				_pickerView.OnDateSelected += (s, ef) =>
				{
					_view.NotifyDateSelected(ef.SelectedDate);
				};

				SetNativeControl(containerView);
			}

		}
    }
}