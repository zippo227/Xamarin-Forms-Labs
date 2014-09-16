using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Droid;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Xamarin.Forms.Labs.Droid
{
	public class ExtendedEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEntry)Element;

            SetFont(view);
            SetTextAlignment(view);
            //SetBorder(view);
            SetPlaceholderTextColor(view);
		}

	    protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

            var view = (ExtendedEntry)Element;

            if (e.PropertyName == ExtendedEntry.FontProperty.PropertyName)
                SetFont(view);
            if (e.PropertyName == ExtendedEntry.XAlignProperty.PropertyName)
                SetTextAlignment(view);
            //if (e.PropertyName == ExtendedEntry.HasBorderProperty.PropertyName)
            //    SetBorder(view);
            if (e.PropertyName == ExtendedEntry.PlaceholderTextColorProperty.PropertyName)
                SetPlaceholderTextColor(view);
		}

	    private void SetBorder(ExtendedEntry view)
	    {
            //NotCurrentlySupported: HasBorder peroperty not suported on Android
	    }

	    private void SetTextAlignment(ExtendedEntry view)
	    {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.Gravity = GravityFlags.CenterHorizontal;
                    break;
                case TextAlignment.End:
                    Control.Gravity = GravityFlags.End;
                    break;
                case TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start;
                    break;
            }
        }

	    private void SetFont(ExtendedEntry view)
	    {
	        if (view.Font != Font.Default)
	            Control.TextSize = view.Font.ToScaledPixel();
	    }

	    private void SetPlaceholderTextColor(ExtendedEntry view){
			if(view.PlaceholderTextColor != Color.Default) 
				Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());			
		}

	}
}

