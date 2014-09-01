using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Labs.Controls;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Droid;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Xamarin.Forms.Labs.Droid
{
	public class ExtendedEntryRenderer : EntryRenderer
	{
		public ExtendedEntryRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEntry)Element;
			var control = Control;

			SetPlaceholderTextColor(view, control);

		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if(e.PropertyName == ExtendedEntry.BackgroundColorProperty.PropertyName){
				SetPlaceholderTextColor((ExtendedEntry)Element, Control);
			}
		}


		private void SetPlaceholderTextColor(ExtendedEntry e, EditText editText){
			if(e.PlaceholderTextColor != Color.Default) {
				editText.SetHintTextColor(e.PlaceholderTextColor.ToAndroid());
				//editText.SetHintTextColor(e.PlaceholderTextColor.ToAndroid);
			}
		}




	}
}

