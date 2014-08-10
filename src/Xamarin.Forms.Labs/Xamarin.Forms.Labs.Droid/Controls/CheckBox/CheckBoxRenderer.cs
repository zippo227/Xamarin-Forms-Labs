using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Droid.Controls;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer(typeof(CheckBox), typeof(CheckBoxRenderer))]

namespace Xamarin.Forms.Labs.Droid.Controls
{
    using NativeCheckBox = Android.Widget.CheckBox;

    public class CheckBoxRenderer : ViewRenderer<CheckBox, Android.Widget.CheckBox>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<CheckBox> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= ElementOnPropertyChanged;
            }

            if (this.Control == null)
            {
                var checkBox = new NativeCheckBox(this.Context);
                checkBox.CheckedChange += checkBox_CheckedChange;

                this.SetNativeControl(checkBox);
            }

            this.Control.Text = e.NewElement.Text;
            this.Control.Checked = e.NewElement.Checked;
            this.Control.SetTextColor(e.NewElement.TextColor.ToAndroid());

            this.Element.PropertyChanged += ElementOnPropertyChanged;
        }

        //private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        //{
        //    Device.BeginInvokeOnMainThread(() =>
        //        {
        //            this.Control.Text = this.Element.Text;
        //            this.Control.Checked = eventArgs.Value;
        //        });
        //}

        void checkBox_CheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            switch (propertyChangedEventArgs.PropertyName)
            {
                case "Checked":
                    this.Control.Text = this.Element.Text;
                    this.Control.Checked = this.Element.Checked;
                    break;
                case "TextColor":
                    this.Control.SetTextColor(this.Element.TextColor.ToAndroid());
                    break;
                case "CheckedText":
                case "UncheckedText":
                    this.Control.Text = this.Element.Text;
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Property change for {0} has not been implemented.", propertyChangedEventArgs.PropertyName);
                    break;
            }
        }
    }
}