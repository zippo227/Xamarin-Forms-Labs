using System.Collections;
using System.Collections.Generic;
namespace Xamarin.Forms.Labs.Charting.Controls
{
    public class Chart : View
    {
        public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(Chart), Color.White, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty SeriesProperty = BindableProperty.Create("Series", typeof(SeriesCollection), typeof(Chart), default(SeriesCollection), BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty WidthProperty = BindableProperty.Create("Width", typeof(float), typeof(Chart), default(float), BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty HeightProperty = BindableProperty.Create("Height", typeof(float), typeof(Chart), default(float), BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty SpacingProperty = BindableProperty.Create("Spacing", typeof(float), typeof(Chart), default(float), BindingMode.OneWay, null, null, null, null);
                
        public Color Color
        {
            get
            {
                return (Color)base.GetValue(Chart.ColorProperty);
            }
            set
            {
                base.SetValue(Chart.ColorProperty, value);
            }
        }

        public SeriesCollection Series
        {
            get
            {
                return (SeriesCollection)base.GetValue(Chart.SeriesProperty);
            }
            set
            {
                base.SetValue(Chart.SeriesProperty, value);
            }
        }

        public float Width
        {
            get
            {
                return (float)base.GetValue(Chart.WidthProperty);
            }
            set
            {
                base.SetValue(Chart.WidthProperty, value);
            }
        }

        public float Height
        {
            get
            {
                return (float)base.GetValue(Chart.HeightProperty);
            }
            set
            {
                base.SetValue(Chart.HeightProperty, value);
            }
        }

        public float Spacing
        {
            get
            {
                return (float)base.GetValue(Chart.SpacingProperty);
            }
            set
            {
                base.SetValue(Chart.SpacingProperty, value);
            }
        }

        public Chart()
        {
            Series = new SeriesCollection();
        }
    }
}
