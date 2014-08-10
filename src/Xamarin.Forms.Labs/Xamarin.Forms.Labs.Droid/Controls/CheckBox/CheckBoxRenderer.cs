using System;
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
                e.OldElement.CheckedChanged -= CheckedChanged;
            }

            //this.checkBox = this.Control;
            if (this.Control == null)
            {
                var checkBox = new NativeCheckBox(this.Context);
                checkBox.CheckedChange += checkBox_CheckedChange;

                this.SetNativeControl(checkBox);
            }

            this.Control.Text = e.NewElement.Text;
            this.Control.Checked = e.NewElement.Checked;
            this.Control.SetTextColor(e.NewElement.TextColor.ToAndroid());

            this.Element.CheckedChanged += CheckedChanged;
        }

        private void CheckedChanged(object sender, EventArgs<bool> eventArgs)
        {
            Device.BeginInvokeOnMainThread(() =>
                {
                    this.Control.Text = this.Element.Text;
                    this.Control.Checked = eventArgs.Value;
                });
        }

        void checkBox_CheckedChange(object sender, Android.Widget.CompoundButton.CheckedChangeEventArgs e)
        {
            this.Element.Checked = e.IsChecked;
        }
    }
}