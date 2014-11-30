using System;
using AG = Android.Graphics;
using Android.Content.Res;
using Xamarin.Forms.Platform.Android;


namespace Xamarin.Forms.Labs.Droid.Controls.Calendar
{
	public class StyleDescriptor
	{
		public AG.Color BackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public AG.Color DateForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		public AG.Color DateBackgroundColor = Color.FromHex("#fff5f7f9").ToAndroid();
		public AG.Color InactiveDateForegroundColor = Color.FromHex("#40778088").ToAndroid();
		public AG.Color InactiveDateBackgroundColor = Color.FromHex("#fff5f7f9").ToAndroid();
		public AG.Color SelectedDateForegroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public AG.Color SelectedDateBackgroundColor = Color.FromHex("#ff379bff").ToAndroid();
		public AG.Color TitleForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		public AG.Color TitleBackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public AG.Color TodayForegroundColor = Color.FromHex("#ff778088").ToAndroid();
		public AG.Color TodayBackgroundColor = Color.FromHex("#ccffcc").ToAndroid();
		public AG.Color DayOfWeekLabelForegroundColor =  Color.FromHex("#ff778088").ToAndroid();
		public AG.Color DayOfWeekLabelBackgroundColor = Color.FromHex("#ffffffff").ToAndroid();
		public AG.Color HighlightedDateForegroundColor =  Color.FromHex("#ff778088").ToAndroid();
		public AG.Color HighlightedDateBackgroundColor = Color.FromHex("#ccffcc").ToAndroid();
		public AG.Color DateSeparatorColor = Color.FromHex("#ffbababa").ToAndroid();
		public Android.Graphics.Typeface MonthTitleFont = null;
		public Android.Graphics.Typeface DateLabelFont = null;
		public bool 	ShouldHighlightDaysOfWeekLabel = false;

	}



}

