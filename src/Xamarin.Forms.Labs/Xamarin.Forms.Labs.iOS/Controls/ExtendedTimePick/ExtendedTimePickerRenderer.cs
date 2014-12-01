using System;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using Xamarin.Forms.Labs.iOS.Controls.BindablePick;
using MonoTouch.Foundation;
using System.Drawing;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls.ExtendedTimePick;

[assembly: ExportRenderer(typeof(ExtendedTimePicker), typeof(ExtendedTimePickerRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls.ExtendedTimePick
{
	public class ExtendedTimePickerRenderer : ViewRenderer<ExtendedTimePicker, UITextField>
	{
		UIDatePicker picker;
		UIPopoverController popOver;

		private void SetBorder(ExtendedTimePicker view)
		{
			Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
		}

		//
		// Methods
		//
		private void HandleValueChanged (object sender, EventArgs e)
		{
			base.Element.Time = this.picker.Date.ToDateTime () - new DateTime (1, 1, 1);
		}

		protected override void OnElementChanged (ElementChangedEventArgs<ExtendedTimePicker> e)
		{
			base.OnElementChanged (e);
			NoCaretField entry = new NoCaretField {
				BorderStyle = UITextBorderStyle.RoundedRect
			};
			entry.Started += new EventHandler (this.OnStarted);
			entry.Ended += new EventHandler (this.OnEnded);
			this.picker = new UIDatePicker {
				Mode = UIDatePickerMode.Time,
				TimeZone = new NSTimeZone ("UTC")
			};
			float width = UIScreen.MainScreen.Bounds.Width;
			UIToolbar uIToolbar = new UIToolbar (new RectangleF (0, 0, width, 44)) {
				BarStyle = UIBarStyle.Default,
				Translucent = true
			};
			UIBarButtonItem uIBarButtonItem = new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace);
			UIBarButtonItem uIBarButtonItem2 = new UIBarButtonItem (UIBarButtonSystemItem.Done, delegate (object o, EventArgs a) {
				entry.ResignFirstResponder ();
			});
			uIToolbar.SetItems (new UIBarButtonItem[] {
				uIBarButtonItem,
				uIBarButtonItem2
			}, false);

			if (Device.Idiom == TargetIdiom.Phone) {
				entry.InputView = this.picker;
				entry.InputAccessoryView = uIToolbar;
			} else {
				entry.InputView = new UIView (RectangleF.Empty);
				entry.InputAccessoryView = new UIView (RectangleF.Empty);
			}

			this.picker.ValueChanged += new EventHandler (this.HandleValueChanged);
			base.SetNativeControl (entry);
			this.UpdateTime ();

			var view = (ExtendedTimePicker)Element;
			SetBorder(view);
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == TimePicker.TimeProperty.PropertyName || e.PropertyName == TimePicker.FormatProperty.PropertyName) {
				this.UpdateTime ();
			}

			var view = (ExtendedTimePicker)Element;

			if (e.PropertyName == ExtendedTimePicker.HasBorderProperty.PropertyName)
				SetBorder(view);
		}

		private void OnEnded (object sender, EventArgs eventArgs)
		{
			//base.Element.IsFocused = false;
		}

		private void OnStarted (object sender, EventArgs eventArgs)
		{
			//base.Element.IsFocused = true;

			if (Device.Idiom != TargetIdiom.Phone) {
				var vc = new UIViewController ();
				vc.Add (picker);
				vc.View.Frame = new RectangleF (0, 0, 320, 200);
				vc.PreferredContentSize = new SizeF (320, 200);
				popOver = new UIPopoverController (vc);
				popOver.PresentFromRect(new RectangleF(Control.Frame.Width/2,Control.Frame.Height-3,0,0), Control, UIPopoverArrowDirection.Any, true);
				popOver.DidDismiss += (object s, EventArgs e) => {
					popOver = null;
					Control.ResignFirstResponder();
				};
			}
		}

		private void UpdateTime ()
		{
			this.picker.Date = new DateTime (1, 1, 1).Add (base.Element.Time).ToNSDate ();
			base.Control.Text = DateTime.Today.Add (base.Element.Time).ToString (base.Element.Format);
		}
	}
}

