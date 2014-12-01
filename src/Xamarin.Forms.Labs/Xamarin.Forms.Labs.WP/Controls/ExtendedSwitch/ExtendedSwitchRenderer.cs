using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Microsoft.Phone.Controls.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.WP8.Controls;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace Xamarin.Forms.Labs.WP8.Controls
{
    public class ExtendedSwitchRenderer : ViewRenderer<ExtendedSwitch, Border>
    {
        private ToggleSwitchButton toggleSwitch;

        protected override void OnElementChanged(ElementChangedEventArgs<ExtendedSwitch> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null)
            {
                this.SetNativeControl(new Border()
                {
                    Child = this.toggleSwitch = new ToggleSwitchButton() { HorizontalAlignment = System.Windows.HorizontalAlignment.Right }
                });

                this.toggleSwitch.Checked += (s, a) => this.Element.IsToggled = true;
                this.toggleSwitch.Unchecked += (s, a) => this.Element.IsToggled = false;
                this.SetTintColor(this.Element.TintColor);
            }

            if (e.OldElement != null)
            {
                e.OldElement.Toggled -= Element_Toggled;
            }

            if (e.NewElement != null)
            {
                this.toggleSwitch.IsChecked = e.NewElement.IsToggled;
                this.Element.Toggled += Element_Toggled;
            }
        }

        protected override void UpdateNativeWidget()
        {
            base.UpdateNativeWidget();
            this.toggleSwitch.IsChecked = this.Element.IsToggled;
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == "TintColor")
            {
                this.SetTintColor(this.Element.TintColor);
            }
        }


        private void SetTintColor(Color color)
        {
            this.toggleSwitch.SwitchForeground = color.ToBrush();
            //this.toggleSwitch.Background = color.ToBrush();
            //this.toggleSwitch.BorderBrush = color.ToBrush();
            //this.toggleSwitch.Foreground = color.ToBrush();
        }

        private void Element_Toggled(object sender, ToggledEventArgs e)
        {
            this.toggleSwitch.IsChecked = this.Element.IsToggled;
        }

        private void Control_ValueChanged(object sender, EventArgs e)
        {
            this.Element.IsToggled = this.toggleSwitch.IsChecked.Value;
        }
    }
}
