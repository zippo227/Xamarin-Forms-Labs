using WPControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.WP.Controls.Calendar;

[assembly: ExportRenderer(typeof(CalendarView), typeof(CalendarViewRenderer))]
namespace Xamarin.Forms.Labs.WP.Controls.Calendar
{
    public class CalendarViewRenderer : ViewRenderer<CalendarView, WPControls.Calendar>
    {
        public CalendarViewRenderer()
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CalendarView> e)
        {
            base.OnElementChanged(e);
            var calendar = new WPControls.Calendar();
            calendar.DateClicked +=
                (object sender, SelectionChangedEventArgs es) =>
                {
                    Element.NotifyDateSelected(es.SelectedDate);
                };
            this.SetNativeControl(calendar);
        }
    }
}
