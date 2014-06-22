using System;
using Xamarin.Forms;

namespace Xamarin.Forms.Labs.Controls
{
    public class SensorBarView : View
    {
        public SensorBarView()
        {
        }

        public static readonly BindableProperty PositiveColorProperty = BindableProperty.Create<SensorBarView,Color>(p => p.PositiveColor, Color.Green);
        public static readonly BindableProperty NegativeColorProperty = BindableProperty.Create<SensorBarView,Color>(p => p.NegativeColor, Color.Red);
        public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create<SensorBarView,double>(p => p.CurrentValue, 0);
        public static readonly BindableProperty LimitProperty = BindableProperty.Create<SensorBarView,double>(p => p.Limit, 1);
            
        public double CurrentValue
        {
            get 
            { 
                return (double)GetValue(CurrentValueProperty); 
            }

            set 
            { 
                if (Math.Abs(value) <= this.Limit)
                {
                    SetValue(CurrentValueProperty,value);
                }
            }
        }

        public double Limit
        {
            get { return (double)GetValue (LimitProperty); }
            set { SetValue (LimitProperty,value); }
        }
            
        public Color PositiveColor
        {
            get { return (Color)GetValue (PositiveColorProperty); }
            set { SetValue (PositiveColorProperty,value); }
        }
            
        public Color NegativeColor
        {
            get { return (Color)GetValue (NegativeColorProperty); }
            set { SetValue (NegativeColorProperty,value); }
        }
    }
}

