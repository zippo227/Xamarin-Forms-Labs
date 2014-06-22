using Xamarin.Forms;
using Xamarin.Forms.Labs.iOS.Controls.Calendar;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Platform.iOS;
using escoz;
using System;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace Xamarin.Forms.Labs.iOS.Controls.Calendar
{
	public class CalendarViewRenderer : ViewRenderer<CalendarView,CalendarMonthView>
	{
		CalendarView _view;
		public CalendarViewRenderer()
		{
		}
		protected override void OnElementChanged (ElementChangedEventArgs<CalendarView> e)
		{
			base.OnElementChanged (e);
			_view = Element;
					
						var calendarView = new CalendarMonthView(DateTime.Now, true);
			
						calendarView.OnDateSelected += (date) =>
						{
							_view.NotifyDateSelected(date);
						};
			
						base.SetNativeControl(calendarView);
		}
    }
}