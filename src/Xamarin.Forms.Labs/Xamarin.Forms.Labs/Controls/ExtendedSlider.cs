using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Controls
{
    public class ExtendedSlider : Slider
    {
        public static readonly BindableProperty CurrentStepValueProperty =
                                BindableProperty.Create<ExtendedSlider, double>(p => p.StepValue, 1.0f);

        public double StepValue
        {
            get { return (double)GetValue(CurrentStepValueProperty); }

            set { SetValue(CurrentStepValueProperty, value); }
        }

        public ExtendedSlider()
        {
            ValueChanged += OnSliderValueChanged;
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);

            Value = newStep * StepValue;
        }
    }
}
