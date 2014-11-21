using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;

namespace XLabs.Forms.Controls.Calendar.MonoDroid.TimesSquare
{
	public class StyleDescriptor
	{
		public Android.Graphics.Color BackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public Android.Graphics.Color DateForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		public Android.Graphics.Color DateBackgroundColor = Color.FromHex("#fff5f7f9").ToAndroid();
		public Android.Graphics.Color InactiveDateForegroundColor = Color.FromHex("#40778088").ToAndroid();
		public Android.Graphics.Color InactiveDateBackgroundColor = Color.FromHex("#fff5f7f9").ToAndroid();
		public Android.Graphics.Color SelectedDateForegroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public Android.Graphics.Color SelectedDateBackgroundColor = Color.FromHex("#ff379bff").ToAndroid();
		public Android.Graphics.Color TitleForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		public Android.Graphics.Color TitleBackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public Android.Graphics.Color TodayForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		public Android.Graphics.Color TodayBackgroundColor = Color.FromHex("#ccffcc").ToAndroid();
		public Android.Graphics.Color DayOfWeekLabelForegroundColor =  Color.FromHex("#ff778088").ToAndroid();
		public Android.Graphics.Color DayOfWeekLabelBackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public Android.Graphics.Color HighlightedDateForegroundColor =  Color.FromHex("#ff778088").ToAndroid();
		public Android.Graphics.Color HighlightedDateBackgroundColor = Color.FromHex("#ccffcc").ToAndroid();
		public Android.Graphics.Color DateSeparatorColor = Color.FromHex("#ffbababa").ToAndroid();
		public Android.Graphics.Typeface MonthTitleFont = null;
		public Android.Graphics.Typeface DateLabelFont = null;
		public bool 	ShouldHighlightDaysOfWeekLabel = false;

	}



}

