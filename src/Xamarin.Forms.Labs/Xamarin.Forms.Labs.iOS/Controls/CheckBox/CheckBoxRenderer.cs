using System;
using System.ComponentModel;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.iOS.Controls;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace Xamarin.Forms.Labs.iOS.Controls
{
    /// <summary>
    /// The check box renderer for iOS.
    /// </summary>
    public class CheckBoxRenderer : ViewRenderer<CheckBox, CheckBoxView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= ElementOnPropertyChanged;
            }

            this.BackgroundColor = this.Element.BackgroundColor.ToUIColor();

            if (this.Control == null)
            {
                var checkBox = new CheckBoxView(this.Bounds);
                checkBox.TouchUpInside += (s, args) => this.Element.Checked = this.Control.Checked;

                this.SetNativeControl(checkBox);
            }

            this.Control.CheckedTitle = string.IsNullOrEmpty(e.NewElement.CheckedText) ? e.NewElement.DefaultText : e.NewElement.CheckedText;
            this.Control.UncheckedTitle = string.IsNullOrEmpty(e.NewElement.UncheckedText) ? e.NewElement.DefaultText : e.NewElement.UncheckedText;
            this.Control.Checked = e.NewElement.Checked;
            this.Control.SetTitleColor(e.NewElement.TextColor.ToUIColor(), UIControlState.Normal);
            this.Control.SetTitleColor(e.NewElement.TextColor.ToUIColor(), UIControlState.Selected);

            this.Element.PropertyChanged += ElementOnPropertyChanged;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Checked":
                    this.Control.Checked = this.Element.Checked;
                    break;
                case "TextColor":
                    this.Control.SetTitleColor(this.Element.TextColor.ToUIColor(), UIControlState.Normal);
                    this.Control.SetTitleColor(this.Element.TextColor.ToUIColor(), UIControlState.Selected);
                    break;
                case "CheckedText":
                    this.Control.CheckedTitle = string.IsNullOrEmpty(this.Element.CheckedText) ? this.Element.DefaultText : this.Element.CheckedText;
                    break;
                case "UncheckedText":
                    this.Control.UncheckedTitle = string.IsNullOrEmpty(this.Element.UncheckedText) ? this.Element.DefaultText : this.Element.UncheckedText;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", propertyChangedEventArgs.PropertyName);
                    break;        
            }
        }

        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.Control.Checked = eventArgs.Value;
            });
        }
    }
}