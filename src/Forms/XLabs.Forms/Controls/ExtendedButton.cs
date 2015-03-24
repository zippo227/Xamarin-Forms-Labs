namespace XLabs.Forms.Controls
{
    using Xamarin.Forms;

    /// <summary>
    /// Extends <see cref="Xamarin.Forms.Button"/>.
    /// </summary>
    public class ExtendedButton : Button
    {
        /// <summary>
        /// Bindable property for button text alignment.
        /// </summary>
        public static readonly BindableProperty VerticalAlignmentProperty =
            BindableProperty.Create<ExtendedButton, TextAlignment>(
                p => p.VerticalAlignment, TextAlignment.Center);

        /// <summary>
        /// Bindable property for button text alignment.
        /// </summary>
        public static readonly BindableProperty HorizontalAlignmentProperty =
            BindableProperty.Create<ExtendedButton, TextAlignment>(
                p => p.HorizontalAlignment, TextAlignment.Center);

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public TextAlignment VerticalAlignment
        {
            get { return this.GetValue<TextAlignment>(VerticalAlignmentProperty); }
            set { this.SetValue(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public TextAlignment HorizontalAlignment
        {
            get { return this.GetValue<TextAlignment>(HorizontalAlignmentProperty); }
            set { this.SetValue(HorizontalAlignmentProperty, value); }
        }
    }
}