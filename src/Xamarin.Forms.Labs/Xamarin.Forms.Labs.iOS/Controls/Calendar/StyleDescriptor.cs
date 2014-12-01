using System;
using MonoTouch.UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;

namespace Xamarin.Forms.Labs.iOS.Controls.Calendar
{
	public class StyleDescriptor
	{
		public UIColor BackgroundColor = Color.FromHex("#ffffffff").ToUIColor();
		public UIColor DateForegroundColor = UIColor.FromRGBA(0.275f, 0.341f, 0.412f, 1f);
		public UIColor DateBackgroundColor = UIColor.White;
		public UIColor InactiveDateForegroundColor = UIColor.LightGray;
		public UIColor InactiveDateBackgroundColor = UIColor.White;
		public UIColor SelectedDateForegroundColor = Color.FromHex("#ffffffff").ToUIColor();
		public UIColor SelectedDateBackgroundColor = Color.FromHex("#ff379bff").ToUIColor();
		public UIColor TitleForegroundColor = UIColor.DarkGray;
		public UIColor TitleBackgroundColor = UIColor.LightGray;
		public UIColor TodayForegroundColor = UIColor.Red;//Color.FromHex("#ff778088").ToUIColor();
		public UIColor TodayBackgroundColor = UIColor.DarkGray;//Color.FromHex("#ccffcc").ToUIColor();
		public UIColor DayOfWeekLabelForegroundColor =  UIColor.White;
		public UIColor DayOfWeekLabelBackgroundColor = UIColor.LightGray;
		public UIColor HighlightedDateForegroundColor =  Color.FromHex("#ff778088").ToUIColor();
		public UIColor HighlightedDateBackgroundColor = Color.FromHex("#ccffcc").ToUIColor();
		public UIColor DateSeparatorColor = Color.FromHex("#ffbababa").ToUIColor();
		public CalendarView.BackgroundStyle SelectionBackgroundStyle = CalendarView.BackgroundStyle.Fill;
		public CalendarView.BackgroundStyle TodayBackgroundStyle = CalendarView.BackgroundStyle.Fill;
		public UIFont DateLabelFont = UIFont.BoldSystemFontOfSize(10);
		public UIFont MonthTitleFont = UIFont.BoldSystemFontOfSize(16);
		public bool 	ShouldHighlightDaysOfWeekLabel = false;

	}
}

