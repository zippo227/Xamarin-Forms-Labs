using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Droid;
using System;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace Xamarin.Forms.Labs.Droid
{
	public class ExtendedEntryRenderer : EntryRenderer
	{
        private const int MIN_DISTANCE = 10;
        private float downX, downY, upX, upY;

		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			var view = (ExtendedEntry)Element;

            SetFont(view);
            SetTextAlignment(view);
            //SetBorder(view);
            SetPlaceholderTextColor(view);

            if (e.NewElement == null)
            {
                this.Touch -= HandleTouch;
            }

            if (e.OldElement == null)
            {
                this.Touch += HandleTouch;
            }
		}

        void HandleTouch (object sender, Android.Views.View.TouchEventArgs e)
        {
            var element = this.Element as ExtendedEntry;
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    downX = e.Event.GetX();
                    downY = e.Event.GetY();
                    return;
                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                case MotionEventActions.Move:
                    upX = e.Event.GetX();
                    upY = e.Event.GetY();

                    float deltaX = downX - upX;
                    float deltaY = downY - upY;

                        // swipe horizontal?
                    if(Math.Abs(deltaX) > Math.Abs(deltaY))
                    {
                        if(Math.Abs(deltaX) > MIN_DISTANCE)
                        {
                                // left or right
                            if(deltaX < 0) { element.OnRightSwipe(this, EventArgs.Empty); return; }
                            if(deltaX > 0) { element.OnLeftSwipe(this, EventArgs.Empty); return; }
                        }
                        else 
                        {
                            Android.Util.Log.Info("ExtendedEntry", "Horizontal Swipe was only " + Math.Abs(deltaX) + " long, need at least " + MIN_DISTANCE);
                            return; // We don't consume the event
                        }
                    }
                    // swipe vertical?
//                    else 
//                    {
//                        if(Math.abs(deltaY) > MIN_DISTANCE){
//                            // top or down
//                            if(deltaY < 0) { this.onDownSwipe(); return true; }
//                            if(deltaY > 0) { this.onUpSwipe(); return true; }
//                        }
//                        else {
//                            Log.i(logTag, "Vertical Swipe was only " + Math.abs(deltaX) + " long, need at least " + MIN_DISTANCE);
//                            return false; // We don't consume the event
//                        }
//                    }

                    return;
            }
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
                case Xamarin.Forms.TextAlignment.Center:
                    Control.Gravity = GravityFlags.CenterHorizontal;
                    break;
                case Xamarin.Forms.TextAlignment.End:
                    Control.Gravity = GravityFlags.End;
                    break;
                case Xamarin.Forms.TextAlignment.Start:
                    Control.Gravity = GravityFlags.Start;
                    break;
            }
        }

	    private void SetFont(ExtendedEntry view)
	    {
			if(view.Font != Font.Default) {
				Control.TextSize = view.Font.ToScaledPixel();
				Control.Typeface = view.Font.ToExtendedTypeface(Context);
			}
	    }

	    private void SetPlaceholderTextColor(ExtendedEntry view){
			if(view.PlaceholderTextColor != Color.Default) 
				Control.SetHintTextColor(view.PlaceholderTextColor.ToAndroid());			
		}
	}
}

