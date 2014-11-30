using System;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using System.Drawing;
using System.Linq;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using Xamarin.Forms.Labs.iOS.Controls.BindablePick;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Labs.Controls.BindablePicker), typeof(BindablePickerRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls.BindablePick
{
	public class BindablePickerRenderer : ViewRenderer<Xamarin.Forms.Labs.Controls.BindablePicker, UITextField>
	{
		private class PickerSource : UIPickerViewModel
		{
			private Xamarin.Forms.Labs.Controls.BindablePicker model;
			public event EventHandler ValueChanged;
			public string SelectedItem
			{
				get;
				internal set;
			}
			public int SelectedIndex
			{
				get;
				internal set;
			}
			public PickerSource(Xamarin.Forms.Labs.Controls.BindablePicker model)
			{
				this.model = model;
			}
			public override int GetRowsInComponent(UIPickerView picker, int component)
			{
				if (this.model.Items == null)
				{
					return 0;
				}
				return this.model.Items.Count;
			}
			public override int GetComponentCount(UIPickerView picker)
			{
				return 1;
			}
			public override string GetTitle(UIPickerView picker, int row, int component)
			{
				return this.model.Items[row];
			}
			public override void Selected(UIPickerView picker, int row, int component)
			{
				this.SelectedItem = this.model.Items[row];
				this.SelectedIndex = row;
				EventHandler valueChanged = this.ValueChanged;
				if (valueChanged != null)
				{
					valueChanged.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private UIPickerView picker;
		private UIPopoverController popOver;
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Labs.Controls.BindablePicker> e)
		{
			((ObservableCollection<string>)e.NewElement.Items).CollectionChanged += new NotifyCollectionChangedEventHandler (this.RowsCollectionChanged);
			NoCaretField entry = new NoCaretField {
				BorderStyle = e.NewElement.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None
			};
			entry.Started += new EventHandler (this.OnStarted);
			entry.Ended += new EventHandler (this.OnEnded);
			this.picker = new UIPickerView {
				Source = new BindablePickerRenderer.PickerSource (e.NewElement)
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

			((BindablePickerRenderer.PickerSource)this.picker.Source).ValueChanged += new EventHandler (this.HandleValueChanged);
			base.SetNativeControl (entry);
			this.UpdatePicker ();
		}

		private void OnEnded(object sender, EventArgs eventArgs)
		{
			//base.Element.IsFocused = false;
			if (Device.Idiom != TargetIdiom.Phone) {

			}
		}

		private void OnStarted(object sender, EventArgs eventArgs)
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

		private void RowsCollectionChanged(object sender, EventArgs e)
		{
			this.UpdatePicker();
		}

		private void HandleValueChanged(object sender, EventArgs e)
		{
			base.Element.SelectedIndex = ((BindablePickerRenderer.PickerSource)sender).SelectedIndex;
			base.Control.Text = ((BindablePickerRenderer.PickerSource)sender).SelectedItem;
		}

		private void UpdatePicker()
		{
			base.Control.Placeholder = base.Element.Title;
			base.Control.Text = (base.Element.SelectedIndex <= -1 || base.Element.Items == null) ? "" : base.Element.Items[base.Element.SelectedIndex];
			this.picker.ReloadAllComponents();
			if (base.Element.SelectedIndex > -1 && base.Element.Items != null && Enumerable.Any<string>(base.Element.Items))
			{
				this.picker.Select(base.Element.SelectedIndex, 0, true);
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == Picker.TitleProperty.PropertyName)
			{
				this.UpdatePicker();
			}
			if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
			{
				this.UpdatePicker();
			}
			if (e.PropertyName == BindablePicker.HasBorderProperty.PropertyName) 
			{
				SetBorder (Element as BindablePicker);
			}
		}

		private void SetBorder(BindablePicker view)
		{
			if (view != null)
				Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
		}
	}
}

