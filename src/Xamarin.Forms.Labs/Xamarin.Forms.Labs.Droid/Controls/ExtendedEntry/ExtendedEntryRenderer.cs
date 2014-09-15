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
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEntry)Element;
			var control = Control;

            SetFont(view);
			SetPlaceholderTextColor(view, control);
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

            var view = (ExtendedEntry)Element;

            if (string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == "Font")
                SetFont(view);

            if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
				SetPlaceholderTextColor((ExtendedEntry)Element, Control);
		}

	    private void SetFont(ExtendedEntry view)
	    {
	        if (view.Font != Font.Default)
	            Control.TextSize = view.Font.ToScaledPixel();
	    }

	    private void SetPlaceholderTextColor(ExtendedEntry view, EditText control){
			if(view.PlaceholderTextColor != Color.Default) 
				control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());			
		}

	}
}

