using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Labs.Controls;
using System.ComponentModel;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.iOS.Controls.ExtendedDatePick;
using Xamarin.Forms.Labs.iOS.Controls.BindablePick;
using MonoTouch.Foundation;
using System.Drawing;

[assembly: ExportRenderer(typeof(ExtendedDatePicker), typeof(ExtendedDatePickerRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls.ExtendedDatePick
{
	public class ExtendedDatePickerRenderer : ViewRenderer<ExtendedDatePicker, UITextField>
	{
		UIDatePicker picker;
		UIPopoverController popOver;

		public ExtendedDatePickerRenderer ()
		{
		}

		private void SetBorder(ExtendedDatePicker view)
		{
			Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
		}

		//
		// Methods
		//
		private void HandleValueChanged (object sender, EventArgs e)
		{
			base.Element.Date = this.picker.Date.ToDateTime ();
		}

		protected override void OnElementChanged (ElementChangedEventArgs<ExtendedDatePicker> e)
		{
			base.OnElementChanged (e);
			NoCaretField entry = new NoCaretField {
				BorderStyle = UITextBorderStyle.RoundedRect
			};
			entry.Started += new EventHandler (this.OnStarted);
			entry.Ended += new EventHandler (this.OnEnded);
			this.picker = new UIDatePicker {
				Mode = UIDatePickerMode.Date,
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

			base.SetNativeControl (entry);
			this.UpdateDateFromModel (false);
			this.UpdateMaximumDate ();
			this.UpdateMinimumDate ();
			this.picker.ValueChanged += new EventHandler (this.HandleValueChanged);

			var view = (ExtendedDatePicker)Element;

			SetBorder(view);
		}

		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == DatePicker.DateProperty.PropertyName || e.PropertyName == DatePicker.FormatProperty.PropertyName) {
				this.UpdateDateFromModel (true);
				return;
			}
			if (e.PropertyName == DatePicker.MinimumDateProperty.PropertyName) {
				this.UpdateMinimumDate ();
				return;
			}
			if (e.PropertyName == DatePicker.MaximumDateProperty.PropertyName) {
				this.UpdateMaximumDate ();
			}

			var view = (ExtendedDatePicker)Element;

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

		private void UpdateDateFromModel (bool animate)
		{
			this.picker.SetDate (base.Element.Date.ToNSDate (), animate);
			base.Control.Text = base.Element.Date.ToString (base.Element.Format);
		}

		private void UpdateMaximumDate ()
		{
			this.picker.MaximumDate = base.Element.MaximumDate.ToNSDate ();
		}

		private void UpdateMinimumDate ()
		{
			this.picker.MinimumDate = base.Element.MinimumDate.ToNSDate ();
		}
	}
}

