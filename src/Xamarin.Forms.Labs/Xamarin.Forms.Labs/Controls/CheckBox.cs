using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Labs.Controls
{
    /// <summary>
    /// The check box.
    /// </summary>
    public class CheckBox : View
    {
        /// <summary>
        /// The width request in inches property.
        /// </summary>
        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create<CheckBox, bool>(
                p => p.Checked, false);

        /// <summary>
        /// The checked text property.
        /// </summary>
        public static readonly BindableProperty CheckedTextProperty =
            BindableProperty.Create<CheckBox, string>(
                p => p.CheckedText, string.Empty);

        /// <summary>
        /// The unchecked text property.
        /// </summary>
        public static readonly BindableProperty UncheckedTextProperty =
            BindableProperty.Create<CheckBox, string>(
                p => p.UncheckedText, string.Empty);

        /// <summary>
        /// The default text property.
        /// </summary>
        public static readonly BindableProperty DefaultTextProperty =
            BindableProperty.Create<CheckBox, string>(
                p => p.Text, string.Empty);

        /// <summary>
        /// Identifies the TextColor bindable property.
        /// </summary>
        /// 
        /// <remarks/>
        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create<CheckBox, Color>(
                p => p.TextColor, Color.Black);


        /// <summary>
        /// The checked changed event.
        /// </summary>
        public EventHandler<EventArgs<bool>> CheckedChanged;
 
        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        public bool Checked
        {
            get
            {
                return this.GetValue<bool>(CheckedProperty);
            }

            set
            {
                this.SetValue(CheckedProperty, value);
                var eventHandler = this.CheckedChanged;
                if (eventHandler != null)
                {
                    eventHandler.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the checked text.
        /// </summary>
        /// <value>The checked state.</value>
        /// <remarks>
        /// Overwrites the default text property if set when checkbox is checked.
        /// </remarks>
        public string CheckedText
        {
            get
            {
                return this.GetValue<string>(CheckedTextProperty);
            }

            set
            {
                this.SetValue(CheckedTextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is checked.
        /// </summary>
        /// <value>The checked state.</value>
        /// <remarks>
        /// Overwrites the default text property if set when checkbox is checked.
        /// </remarks>
        public string UncheckedText
        {
            get
            {
                return this.GetValue<string>(UncheckedTextProperty);
            }

            set
            {
                this.SetValue(UncheckedTextProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string DefaultText
        {
            get
            {
                return this.GetValue<string>(DefaultTextProperty);
            }

            set
            {
                this.SetValue(DefaultTextProperty, value);
            }
        }

        public Color TextColor 
        {
            get
            {
                return this.GetValue<Color>(TextColorProperty);
            }

            set
            {
                this.SetValue(TextColorProperty, value);
            }
        }
        public string Text
        {
            get
            {
                return this.Checked
                           ? (string.IsNullOrEmpty(this.CheckedText) ? this.DefaultText : this.CheckedText)
                           : (string.IsNullOrEmpty(this.UncheckedText) ? this.DefaultText : this.UncheckedText);
            }
        }
    }
}
