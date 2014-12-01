using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
    public class ExtendedSwitch : Switch
    {
        /// <summary>
        /// Identifies the Switch tint color bindable property.
        /// </summary>
        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create<ExtendedSwitch, Color>(
                p => p.TintColor, Color.Black);

        public Color TintColor
        {
            get
            {
                return this.GetValue<Color>(TintColorProperty);
            }

            set
            {
                this.SetValue(TintColorProperty, value);
            }
        }
    }
}
