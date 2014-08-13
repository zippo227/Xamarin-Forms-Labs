using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Charting.Controls
{
    /// <summary>
    /// Contains the logic for drawing a point.
    /// </summary>
    public class DataPoint : Element
    {
        public static readonly BindableProperty LabelProperty = BindableProperty.Create("Label", typeof(string), typeof(DataPoint), String.Empty, BindingMode.OneWay, null, null, null, null);
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(float), typeof(DataPoint), 0.0F, BindingMode.OneWay, null, null, null, null);

        /// <summary>
        /// X-axis label. Only the labels of the first series will be rendered.
        /// </summary>
        public string Label
        {
            get
            {
                return (string)base.GetValue(DataPoint.LabelProperty);
            }
            set
            {
                base.SetValue(DataPoint.LabelProperty, value);
            }
        }

        /// <summary>
        /// Value of the point, used to be drawn.
        /// </summary>
        public float Value
        {
            get
            {
                return (float)base.GetValue(DataPoint.ValueProperty);
            }
            set
            {
                base.SetValue(DataPoint.ValueProperty, value);
            }
        }

        public DataPoint()
        {
        }

        public DataPoint(string label, float value)
        {
            Label = label;
            Value = value;
        }
    }
}
